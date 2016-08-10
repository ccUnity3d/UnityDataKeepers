using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeeperTests.DummyObjects;

namespace UnityDataKeeperTests.DataLayer.DataDriver
{
    [TestClass]
    class CsvDataCollectionDriverTest
    {
        private string CreateGoodCsvFile()
        {
            
        }

        [TestMethod]
        public void IsInInitialState()
        {
            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.IsInInitialState(driver);
        }

        [TestMethod]
        public void AddTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest1(driver);
        }

        [TestMethod]
        public void AddTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest2(driver);
        }

        [TestMethod]
        public void AddTest3()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest3(driver);
        }

        [TestMethod]
        public void AddMultipleTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest1(driver);
        }

        [TestMethod]
        public void AddMultipleTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest2(driver);
        }

        [TestMethod]
        public void AddMultipleTest3()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest3(driver);
        }

        [TestMethod]
        public void AddMultipleTest4()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest4(driver);
        }

        [TestMethod]
        public void GetByHashTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.GetByHashTest1(driver);
        }

        [TestMethod]
        public void GetByHashTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.GetByHashTest2(driver);
        }

        [TestMethod]
        public void RemoveTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest1(driver);
        }

        [TestMethod]
        public void RemoveTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest2(driver);
        }

        [TestMethod]
        public void RemoveTest3()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest3(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest1(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest2(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest3()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest3(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest4()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest4(driver);
        }

        [TestMethod]
        public void CountTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.CountTest(driver);
        }

        [TestMethod]
        public void GetAllTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.GetAllTest(driver);
        }

        [TestMethod]
        public void ClearTest()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.ClearTest(driver);
        }

        [TestMethod]
        public void UpdateTest1()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest1(driver,
                item =>
                {
                    item.DummyProperty += "test";
                    return item;
                });
        }

        [TestMethod]
        public void UpdateTest2()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest2(driver);
        }

        [TestMethod]
        public void UpdateTest3()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest3(driver);
        }

        [TestMethod]
        public void UpdateTest4()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest4(driver);
        }
    }
}
