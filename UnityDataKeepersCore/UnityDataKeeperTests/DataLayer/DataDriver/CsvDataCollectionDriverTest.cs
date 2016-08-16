using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeeperTests.DummyObjects;

namespace UnityDataKeeperTests.DataLayer.DataDriver
{
    [TestClass]
    public class CsvDataCollectionDriverTest
    {
        private static readonly Random Rnd = new Random();

        private const string GoodCsvText =
            "StringProperty,IntProperty,FloatProperty,EnumField,DateTimeField,TimeSpanField\nsimle text,1,\"1,2\",Field1,12.08.2016 6:15:58,1\ntext,1,\"1,1568\",Field2,12.08.2016 5:13:45,2\n\"\"\"text\"\"\",1,\"1,1136\",OtherField,12.08.2016 4:11:33,1m\n\"text,text\",1,\"1,0704\",Field3,12.08.2016 3:09:20,15m\ntext\text,1,\"1,0272\",Field4,12.08.2016 2:07:08,120m\n@@@,1,\"0,984\",OtherField,12.08.2016 1:04:55,1s\n##,1,\"0,9408\",Field5,12.08.2016 0:02:43,15s\n 'text',1,\"0,8976\",Field6,11.08.2016 23:00:30,120s\n    ,1,\"0,8544\",OtherField,11.08.2016 21:58:18,1h\n,1,\"0,8112\",Field7,11.08.2016 20:56:05,15h\nlkjkjsdflhksdf,1,\"0,768\",Field8,11.08.2016 19:53:53,120h\n12321text,1,\"0,7248\",OtherField,11.08.2016 18:51:40,1d\n123,1,\"0,6816\",Field9,11.08.2016 17:49:28,15d\n\"0,6384\",1,\"0,6384\",Field10,11.08.2016 16:47:15,120d\n\"42593,65598\",1,\"0,5952\",OtherField,11.08.2016 15:45:03,1m\n120h,1,\"0,552\",Field11,11.08.2016 14:42:50,15m\n1d,1,\"0,5088\",Field12,11.08.2016 13:40:38,120m\n15d,1,\"0,4656\",OtherField,11.08.2016 12:38:25,3\n120d,1,\"0,4224\",Field13,11.08.2016 11:36:13,4\n1m,1,\"0,3792\",Field14,11.08.2016 10:34:00,1m\nsimle text,1,\"0,336\",OtherField,11.08.2016 9:31:48,15m\ntext,1,\"0,2928\",Field15,11.08.2016 8:29:35,120m\n\"\"\"text\"\"\",1,\"0,2496\",Field16,11.08.2016 7:27:23,1s\n\"text,text\",1,\"0,2064\",OtherField,11.08.2016 6:25:11,15s\ntext\text,1,\"0,1632\",Field17,11.08.2016 5:22:58,120s\n@@@,1,\"0,12\",Field18,11.08.2016 4:20:46,1h\n##,1,\"0,0768\",OtherField,11.08.2016 3:18:33,15h\n 'text',1,\"0,0336\",Field19,11.08.2016 2:16:21,120h\n    ,1,\"-0,0096\",Field20,11.08.2016 1:14:08,1d";

        private static string CreateGoodCsvFile()
        {
            var rndFileName = string.Concat(DateTime.UtcNow.Ticks.ToString(),
                Rnd.NextDouble().ToString(CultureInfo.InvariantCulture),
                ".csv");
            var file = File.CreateText(rndFileName);
            file.Write(GoodCsvText);
            file.Close();
            return rndFileName;
        }

        #region BaseTests

        [TestMethod]
        public void IsInInitialState()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false, false))
                {
                    tester.IsEmptyAndInInitialState(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddTest_Good(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddTest_AddNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddTest_AddNull(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddTest_AddItemTwice()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddTest_AddItemTwice(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddMultipleTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddMultipleTest_Good(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddMultipleTest_AddNullItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddMultipleTest_AddNullItems(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddMultipleTest_AddNullCollection()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddMultipleTest_AddNullCollection(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddMultipleTest_DoubleAddItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddMultipleTest_DoubleAddItems(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void GetByHashTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.GetByHashTest_Good(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void GetByHashTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.GetByHashTest_PushNull(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveTest_Good(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveTest_DoubleRemove()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveTest_DoubleRemove(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveTest_PushNull(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveMultipleTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveMultipleTest_Good(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveMultipleTest_PushNullItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveMultipleTest_PushNullItems(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveMultipleTest_DoubleRemove()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveMultipleTest_DoubleRemove(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void RemoveMultipleTest_PushNullCollection()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.RemoveMultipleTest_PushNullCollection(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void CountTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.CountTest(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void GetAllTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false, false))
                {
                    tester.GetAllTest(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void ClearTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.ClearTest(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UpdateTest_SimpleUpdate()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UpdateTest_SimpleUpdate(driver,
                        item =>
                        {
                            item.StringProperty += "test";
                            return item;
                        });
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UpdateTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UpdateTest_PushNull(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UpdateTest_UpdateNotMidifiedItem()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UpdateTest_UpdateNotMidifiedItem(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UpdateTest_UpdateItemNotInCollection()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UpdateTest_UpdateItemNotInCollection(driver);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UniqueHashesSimpleAdd()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UniqueHashesSimpleAdd(driver, 1000);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void UniqueHashesListAdd()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.UniqueHashesListAdd(driver, 1000);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void Add_Samereferences()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.Add_Samereferences(driver, 1000);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddMultiple_SameReferences()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    tester.AddMultiple_SameReferences(driver, 1000);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        #endregion

#region CSVTests

        [TestMethod]
        public void SimpleReadTest()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<CsvTestsDummyCollectionItem>,
                        CsvTestsDummyCollectionItem>();
            var fileName = CreateGoodCsvFile();
            try
            {
                using (var driver =
                    DataCollectionDriverFactory
                        .CreateCsvDataDriver<CsvTestsDummyCollectionItem>
                        (fileName, false))
                {
                    var neededStrings =
                        driver.GetAll()
                            .Where(i => i.StringProperty.Equals("simle text")).ToList();
                    Assert.IsNotNull(neededStrings);
                    Assert.AreEqual(2,neededStrings.Count);
                    Assert.AreEqual(1.2f,neededStrings[0].FloatProperty);
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }
#endregion
    }
}