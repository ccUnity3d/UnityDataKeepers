/*
 * Used http://www.codeproject.com/Articles/685310/Simple-and-fast-CSV-library-in-Csharp
 * Pascal Ganaye, thank for good job! :)
 */

#define NET_3_5

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#if !NET_3_5
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
#endif

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers.CsvFiles
{
    internal class CsvFile : IDisposable
    {
        #region Static Members
        public static CsvDefinition DefaultCsvDefinition { get; set; }
        public static bool UseLambdas { get; set; }
        public static bool UseTasks { get; set; }
        public static bool FastIndexOfAny { get; set; }

        static CsvFile()
        {
            // Choosing default Field Separator is a hard decision
            // In theory the C of CSV means Comma separated 
            // In Windows though many people will try to open the csv with Excel which is horrid with real CSV.
            // As the target for this library is Windows we go for Semi Colon.
            DefaultCsvDefinition = new CsvDefinition
            {
                EndOfLine = "\r\n",
                FieldSeparator = ',',
                TextQualifier = '"'
            };
            UseLambdas = true;
            UseTasks = true;
            FastIndexOfAny = true;
        }

        #endregion

        internal protected Stream BaseStream;
        protected static DateTime DateTimeZero = new DateTime();


        public static IEnumerable<T> Read<T>(CsvSource csvSource) where T : new()
        {
            var csvFileReader = new CsvFileReader<T>(csvSource);
            return (IEnumerable<T>)csvFileReader;
        }

        public char FieldSeparator { get; private set; }
        public char TextQualifier { get; private set; }
        public IEnumerable<String> Columns { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // overriden in derived classes
        }
    }

    internal class CsvFile<T> : CsvFile
    {
        private readonly char fieldSeparator;
        private readonly string fieldSeparatorAsString;
        private readonly char[] invalidCharsInFields;
        private readonly StreamWriter streamWriter;
        private readonly char textQualifier;
        private readonly String[] columns;
        private Func<T, object>[] getters;
        readonly bool[] isInvalidCharInFields;
#if !NET_3_5
        private int linesToWrite;
        private readonly BlockingCollection<string> csvLinesToWrite = new BlockingCollection<string>(5000);
        private readonly Thread writeCsvLinesTask;
        private Task addAsyncTask;
#endif

        public CsvFile(CsvDestination csvDestination)
            : this(csvDestination, null)
        {
        }

        public CsvFile()
        {
        }

        public CsvFile(CsvDestination csvDestination, CsvDefinition csvDefinition)
        {
            if (csvDefinition == null)
                csvDefinition = DefaultCsvDefinition;
            this.columns = (csvDefinition.Columns ?? InferColumns(typeof(T))).ToArray();
            this.fieldSeparator = csvDefinition.FieldSeparator;
            this.fieldSeparatorAsString = this.fieldSeparator.ToString(CultureInfo.InvariantCulture);
            this.textQualifier = csvDefinition.TextQualifier;
            this.streamWriter = csvDestination.StreamWriter;

            this.invalidCharsInFields = new[] { '\r', '\n', this.textQualifier, this.fieldSeparator };
            this.isInvalidCharInFields = new bool[256];

            foreach (var c in this.invalidCharsInFields)
            {
                this.isInvalidCharInFields[c] = true;
            }
            this.WriteHeader();

            this.CreateGetters();
#if !NET_3_5
            if (CsvFile.UseTasks)
            {
                writeCsvLinesTask = new Thread((o) => this.WriteCsvLines());
                writeCsvLinesTask.Start();
            }
            this.addAsyncTask = Task.Factory.StartNew(() => { });

#endif

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
#if !NET_3_5
                addAsyncTask.Wait();
                if (csvLinesToWrite != null)
                {
                    csvLinesToWrite.CompleteAdding();
                }
                if (writeCsvLinesTask != null)
                    writeCsvLinesTask.Join();
#endif
                this.streamWriter.Close();
            }
        }

        protected static IEnumerable<string> InferColumns(Type recordType)
        {
            var columns = recordType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.GetIndexParameters().Length == 0
                    && pi.GetSetMethod() != null
                    && !Attribute.IsDefined(pi, typeof(CsvIgnorePropertyAttribute)))
                .Select(pi => pi.Name)
                .Concat(recordType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(fi => !Attribute.IsDefined(fi, typeof(CsvIgnorePropertyAttribute)))
                    .Select(fi => fi.Name))
                .ToList();
            return columns;
        }

#if !NET_3_5
        private void WriteCsvLines()
        {
            int written = 0;
            foreach (var csvLine in csvLinesToWrite.GetConsumingEnumerable())
            {
                this.streamWriter.WriteLine(csvLine);
                written++;
            }
            Interlocked.Add(ref this.linesToWrite, -written);
        }
#endif


        public void Append(T record)
        {

            if (CsvFile.UseTasks)
            {
#if !NET_3_5

                var linesWaiting = Interlocked.Increment(ref this.linesToWrite);
                Action<Task> addRecord = (t) =>
                {
                    var csvLine = this.ToCsv(record);
                    this.csvLinesToWrite.Add(csvLine);
                };

                if (linesWaiting < 10000)
                    this.addAsyncTask = this.addAsyncTask.ContinueWith(addRecord);
                else
                    addRecord(null);
#else
                throw new NotImplementedException("Tasks");
#endif
            }
            else
            {
                var csvLine = this.ToCsv(record);
                this.streamWriter.WriteLine(csvLine);
            }
        }

        private static Func<T, object> FindGetter(string c, bool staticMember)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase | (staticMember ? BindingFlags.Static : BindingFlags.Instance);
            Func<T, object> func = null;
            PropertyInfo pi = typeof(T).GetProperty(c, flags);
            FieldInfo fi = typeof(T).GetField(c, flags);

            if (CsvFile.UseLambdas)
            {
                Expression expr = null;
                ParameterExpression parameter = Expression.Parameter(typeof(T), "r");
                Type type = null;

                if (pi != null)
                {
                    type = pi.PropertyType;
                    expr = Expression.Property(parameter, pi.Name);
                }
                else if (fi != null)
                {
                    type = fi.FieldType;
                    expr = Expression.Field(parameter, fi.Name);
                }
                if (expr != null)
                {
                    Expression<Func<T, object>> lambda;
                    if (type.IsValueType)
                    {
                        lambda = Expression.Lambda<Func<T, object>>(Expression.TypeAs(expr, typeof(object)), parameter);
                    }
                    else
                    {
                        lambda = Expression.Lambda<Func<T, object>>(expr, parameter);
                    }
                    func = lambda.Compile();
                }
            }
            else
            {
                if (pi != null)
                    func = o => pi.GetValue(o, null);
                else if (fi != null)
                    func = o => fi.GetValue(o);
            }
            return func;
        }

        private void CreateGetters()
        {
            var list = new List<Func<T, object>>();

            foreach (var columnName in columns)
            {
                Func<T, Object> func = null;
                var propertyName = (columnName.IndexOf(' ') < 0 ? columnName : columnName.Replace(" ", ""));
                func = FindGetter(columnName, false) ?? FindGetter(columnName, true);

                list.Add(func);
            }
            this.getters = list.ToArray();
        }

        private string ToCsv(T record)
        {
            if (record == null)
                throw new ArgumentException("Cannot be null", "record");

            string[] csvStrings = new string[getters.Length];

            for (int i = 0; i < getters.Length; i++)
            {
                var getter = getters[i];
                object fieldValue = getter == null ? null : getter(record);
                csvStrings[i] = this.ToCsvString(fieldValue);
            }
            return string.Join(this.fieldSeparatorAsString, csvStrings);

        }

        private string ToCsvString(object o)
        {
            if (o != null)
            {
                string valueString = o as string ?? Convert.ToString(o, CultureInfo.CurrentUICulture);
                if (RequiresQuotes(valueString))
                {
                    var csvLine = new StringBuilder();
                    csvLine.Append(this.textQualifier);
                    foreach (char c in valueString)
                    {
                        if (c == this.textQualifier)
                            csvLine.Append(c); // double the double quotes
                        csvLine.Append(c);
                    }
                    csvLine.Append(this.textQualifier);
                    return csvLine.ToString();
                }
                else
                    return valueString;
            }
            return string.Empty;
        }

        private bool RequiresQuotes(string valueString)
        {
            if (CsvFile.FastIndexOfAny)
            {
                var len = valueString.Length;
                for (int i = 0; i < len; i++)
                {
                    char c = valueString[i];
                    if (c <= 255 && this.isInvalidCharInFields[c])
                        return true;
                }
                return false;
            }
            else
            {
                return valueString.IndexOfAny(this.invalidCharsInFields) >= 0;
            }
        }

        private void WriteHeader()
        {
            var csvLine = new StringBuilder();
            for (int i = 0; i < this.columns.Length; i++)
            {
                if (i > 0)
                    csvLine.Append(this.fieldSeparator);
                csvLine.Append(this.ToCsvString(this.columns[i]));
            }
            this.streamWriter.WriteLine(csvLine.ToString());
        }
    }

    // 2013-11-29 Version 1
    // 2014-01-06 Version 2: add CoryLuLu suggestions
}