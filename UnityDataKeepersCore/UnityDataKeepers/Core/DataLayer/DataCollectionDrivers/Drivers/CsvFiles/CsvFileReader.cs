/*
 * Thanks Pascal Ganaye for great job! :)
 * coperight: http://www.codeproject.com/Articles/685310/Simple-and-fast-CSV-library-in-Csharp
 */

#define NET_3_5

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers.CsvFiles
{
    internal class CsvFileReader<T> : CsvFile, IEnumerable<T>, IEnumerator<T>
        where T : new()
    {
        private readonly Dictionary<Type, List<Action<T, String>>> allSetters = new Dictionary<Type, List<Action<T, String>>>();
        private string[] columns;
        private char curChar;
        private int len;
        private string line;
        private int pos;
        private T record;
        private readonly char fieldSeparator;
        private readonly TextReader textReader;
        private readonly char textQualifier;
        private readonly StringBuilder parseFieldResult = new StringBuilder();

        public CsvFileReader(CsvSource csvSource)
            : this(csvSource, null)
        {
        }

        public CsvFileReader(CsvSource csvSource, CsvDefinition csvDefinition)
        {
            var streamReader = csvSource.TextReader as StreamReader;
            if (streamReader != null)
                this.BaseStream = streamReader.BaseStream;
            if (csvDefinition == null)
                csvDefinition = DefaultCsvDefinition;
            this.fieldSeparator = csvDefinition.FieldSeparator;
            this.textQualifier = csvDefinition.TextQualifier;

            this.textReader = csvSource.TextReader;// new FileStream(csvSource.TextReader, FileMode.Open);

            this.ReadHeader(csvDefinition.Header);

        }

        public T Current
        {
            get { return this.record; }
        }

        public bool Eof
        {
            get { return this.line == null; }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.textReader.Dispose();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            this.ReadNextLine();
            if (this.line == null && (this.line = this.textReader.ReadLine()) == null)
            {
                this.record = default(T);
            }
            else
            {
                this.record = new T();
                Type recordType = typeof(T);
                List<Action<T, String>> setters;
                if (!this.allSetters.TryGetValue(recordType, out setters))
                {
                    setters = this.CreateSetters();
                    this.allSetters[recordType] = setters;
                }

                var fieldValues = new List<string>();
                for (int i = 0; i < setters.Count; i++)
                {
                    fieldValues.Add(this.ParseField());
                    if (this.curChar == this.fieldSeparator)
                        this.NextChar();
                    else
                        break;
                }

                if (fieldValues.Count != setters.Count)
                    throw new ArgumentException();

                for (int i = 0; i < setters.Count; i++)
                {
                    var setter = setters[i];
                    if (setter != null)
                    {
                        setter(this.record, fieldValues[i]);
                    }
                }
            }
            return (this.record != null);
        }


        public void Reset()
        {
            throw new NotImplementedException("Cannot reset CsvFileReader enumeration.");
        }

        private static Action<T, string> EmitSetValueAction(MemberInfo mi, Func<string, object> func)
        {
            ParameterExpression paramExpObj = Expression.Parameter(typeof(object), "obj");
            ParameterExpression paramExpT = Expression.Parameter(typeof(T), "instance");

            {
                var pi = mi as PropertyInfo;
                if (pi != null)
                {
#if !NET_3_5
                    if (CsvFile.UseLambdas)
                    {
                        var callExpr = Expression.Call(
                            paramExpT,
                            pi.GetSetMethod(),
                            Expression.ConvertChecked(paramExpObj, pi.PropertyType));
                        var setter = Expression.Lambda<Action<T, object>>(
                            callExpr,
                            paramExpT,
                            paramExpObj).Compile();
                        return (o, s) => setter(o, func(s));
                    }
#endif
                    return (o, v) => pi.SetValue(o, (object)func(v), null);

                }
            }
            {
                var fi = mi as FieldInfo;
                if (fi != null)
                {
#if !NET_3_5
                    if (CsvFile.UseLambdas)
                    {
                        //ParameterExpression valueExp = Expression.Parameter(typeof(string), "value");
                        var valueExp = Expression.ConvertChecked(paramExpObj, fi.FieldType);

                        // Expression.Property can be used here as well
                        MemberExpression fieldExp = Expression.Field(paramExpT, fi);
                        BinaryExpression assignExp = Expression.Assign(fieldExp, valueExp);

                        var setter = Expression.Lambda<Action<T, object>>
                            (assignExp, paramExpT, paramExpObj).Compile();

                        return (o, s) => setter(o, func(s));
                    }
#endif
                    return ((o, v) => fi.SetValue(o, func(v)));
                }
            }
            throw new NotImplementedException();
        }

        private static Action<T, string> FindSetter(string c, bool staticMember)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase | (staticMember ? BindingFlags.Static : BindingFlags.Instance);
            Action<T, string> action = null;
            PropertyInfo pi = typeof(T).GetProperty(c, flags);
            if (pi != null)
            {
                var pFunc = StringToObject(pi.PropertyType);
                action = EmitSetValueAction(pi, pFunc);
            }
            FieldInfo fi = typeof(T).GetField(c, flags);
            if (fi != null)
            {
                var fFunc = StringToObject(fi.FieldType);
                action = EmitSetValueAction(fi, fFunc);
            }
            return action;
        }

        private static Func<string, object> StringToObject(Type propertyType)
        {
            if (propertyType == typeof (string))
                return (s) => s ?? string.Empty;

            if (propertyType == typeof (int))
                return (s) => string.IsNullOrEmpty(s)
                    ? 0
                    : int.Parse(s);

            if (propertyType == typeof (DateTime))
                return (s) => string.IsNullOrEmpty(s)
                    ? DateTimeZero
                    : DateTime.Parse(s);

            if (propertyType == typeof (float))
                return (s) => string.IsNullOrEmpty(s)
                    ? 0
                    : float.Parse(s);

            if (propertyType.IsEnum)
                return (s) => string.IsNullOrEmpty(s)
                    ? 0
                    : Enum.Parse(propertyType, s);

            if (propertyType == typeof(TimeSpan))
                return (s) => string.IsNullOrEmpty(s)
                    ? TimeSpan.Zero
                    : ParsTimespan(s);

            throw new NotImplementedException();
        }

        private static TimeSpan ParsTimespan(string s)
        {
            var t = TimeSpan.Zero;
//            if (TimeSpan.TryParse(s, out t))
//                return t;
            var lastStart = 0;
            var pos = 0;
            for (; pos < s.Length; pos++)
            {
                switch (s[pos])
                {
                    case ' ':
                        lastStart = pos+1;
                        break;
                        
                    case 's':
                        t = t.Add(TimeSpan.FromSeconds(int.Parse(s.Substring(lastStart, pos - lastStart))));
                        lastStart = pos+1;
                        break;

                    case 'm':
                        t = t.Add(TimeSpan.FromMinutes(int.Parse(s.Substring(lastStart, pos - lastStart))));
                        lastStart = pos + 1;
                        break;

                    case 'h':
                        t = t.Add(TimeSpan.FromHours(int.Parse(s.Substring(lastStart, pos - lastStart))));
                        lastStart = pos + 1;
                        break;

                    case 'd':
                        t = t.Add(TimeSpan.FromDays(int.Parse(s.Substring(lastStart, pos - lastStart))));
                        lastStart = pos + 1;
                        break;
                }
            }
            if (pos > lastStart)
            {
                var value = 0;
                if (int.TryParse(s.Substring(lastStart, pos - lastStart), out value))
                {
                    t = t.Add(TimeSpan.FromSeconds(value));
                }
                else
                {
                    t = TimeSpan.Parse(s);
                }

            }
            return t;
        }

        private List<Action<T, string>> CreateSetters()
        {
            var list = new List<Action<T, string>>();
            for (int i = 0; i < this.columns.Length; i++)
            {
                string columnName = this.columns[i];
                Action<T, string> action = null;
                if (columnName.IndexOf(' ') >= 0)
                    columnName = columnName.Replace(" ", "");
                action = FindSetter(columnName, false) ?? FindSetter(columnName, true);

                list.Add(action);
            }
            return list;
        }

        private void NextChar()
        {
            if (this.pos < this.len)
            {
                this.pos++;
                this.curChar = this.pos < this.len ? this.line[this.pos] : '\0';
            }
        }

        private void ParseEndOfLine()
        {
            throw new NotImplementedException();
        }


        private string ParseField()
        {
            parseFieldResult.Length = 0;
            if (this.line == null || this.pos >= this.len)
                return null;
            while (this.curChar == ' ' || this.curChar == '\t')
            {
                this.NextChar();
            }
            if (this.curChar == this.textQualifier)
            {
                this.NextChar();
                while (this.curChar != 0)
                {
                    if (this.curChar == this.textQualifier)
                    {
                        this.NextChar();
                        if (this.curChar == this.textQualifier)
                        {
                            this.NextChar();
                            parseFieldResult.Append(this.textQualifier);
                        }
                        else
                            return parseFieldResult.ToString();
                    }
                    else if (this.curChar == '\0')
                    {
                        if (this.line == null)
                            return parseFieldResult.ToString();
                        this.ReadNextLine();
                    }
                    else
                    {
                        parseFieldResult.Append(this.curChar);
                        this.NextChar();
                    }
                }
            }
            else
            {
                while (this.curChar != 0 && this.curChar != this.fieldSeparator && this.curChar != '\r' && this.curChar != '\n')
                {
                    parseFieldResult.Append(this.curChar);
                    this.NextChar();
                }
            }
            return parseFieldResult.ToString();
        }

        private void ReadHeader(string header)
        {
            if (header == null)
            {
                this.ReadNextLine();
            }
            else
            {
                // we read the first line from the given header
                this.line = header;
                this.pos = -1;
                this.len = this.line.Length;
                this.NextChar();
            }

            var readColumns = new List<string>();
            string columnName;
            while ((columnName = this.ParseField()) != null)
            {
                readColumns.Add(columnName);
                if (this.curChar == this.fieldSeparator)
                    this.NextChar();
                else
                    break;
            }
            this.columns = readColumns.ToArray();
        }

        private void ReadNextLine()
        {
            this.line = this.textReader.ReadLine();
            this.pos = -1;
            if (this.line == null)
            {
                this.len = 0;
                this.curChar = '\0';
            }
            else
            {
                this.len = this.line.Length;
                this.NextChar();
            }
        }
    }
}