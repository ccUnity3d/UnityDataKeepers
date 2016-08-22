using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeeperTests.DummyObjects;

namespace UnityDataKeeperTests.DataLayer.DataDriver
{
    [TestClass]
    public class SessionDataDriverTests
    {
        [TestMethod]
        public void IsInInitialState()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.IsEmptyAndInInitialState(driver);
        }

        [TestMethod]
        public void AddTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest_Good(driver);
        }

        [TestMethod]
        public void AddTest_AddNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest_AddNull(driver);
        }

        [TestMethod]
        public void AddTest_AddItemTwice()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddTest_AddItemTwice(driver);
        }

        [TestMethod]
        public void AddMultipleTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest_Good(driver);
        }

        [TestMethod]
        public void AddMultipleTest_AddNullItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest_AddNullItems(driver);
        }

        [TestMethod]
        public void AddMultipleTest_AddNullCollection()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest_AddNullCollection(driver);
        }

        [TestMethod]
        public void AddMultipleTest_DoubleAddItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultipleTest_DoubleAddItems(driver);
        }

        [TestMethod]
        public void GetByHashTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.GetByHashTest_Good(driver);
        }

        [TestMethod]
        public void GetByHashTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.GetByHashTest_PushNull(driver);
        }

        [TestMethod]
        public void RemoveTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest_Good(driver);
        }

        [TestMethod]
        public void RemoveTest_DoubleRemove()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest_DoubleRemove(driver);
        }

        [TestMethod]
        public void RemoveTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveTest_PushNull(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest_Good()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest_Good(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest_PushNullItems()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest_PushNullItems(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest_DoubleRemove()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest_DoubleRemove(driver);
        }

        [TestMethod]
        public void RemoveMultipleTest_PushNullCollection()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.RemoveMultipleTest_PushNullCollection(driver);
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
        public void UpdateTest_SimpleUpdate()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest_SimpleUpdate(driver,
                item =>
                {
                    item.DummyProperty += "test";
                    return item;
                });
        }

        [TestMethod]
        public void UpdateTest_PushNull()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest_PushNull(driver);
        }

        [TestMethod]
        public void UpdateTest_UpdateNotMidifiedItem()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest_UpdateNotMidifiedItem(driver);
        }

        [TestMethod]
        public void UpdateTest_UpdateItemNotInCollection()
        {

            var tester =
                new DataCollectionDriverInterfaceTester
                    <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UpdateTest_UpdateItemNotInCollection(driver);
        }

        [TestMethod]
        public void IsReadonlyTest()
        {
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            // session is useless if it can't be rewritten
            Assert.IsFalse(driver.IsNotStorable);
        }

        [TestMethod]
        public void UniqueHashesSimpleAdd()
        {
            var tester =
               new DataCollectionDriverInterfaceTester
                   <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UniqueHashesSimpleAdd(driver,1000);
        }

        [TestMethod]
        public void UniqueHashesListAdd()
        {
            var tester =
               new DataCollectionDriverInterfaceTester
                   <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.UniqueHashesListAdd(driver,1000);
        }
        
        [TestMethod]
        public void Add_Samereferences()
        {
            var tester =
               new DataCollectionDriverInterfaceTester
                   <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.Add_Samereferences(driver, 1000);
        }
        
        [TestMethod]
        public void AddMultiple_SameReferences()
        {
            var tester =
               new DataCollectionDriverInterfaceTester
                   <IDataCollectionDriver<DummyDataItem>, DummyDataItem>();
            var driver =
                DataCollectionDriverFactory.CreateSessionDataDriver<DummyDataItem>
                    ();
            tester.AddMultiple_SameReferences(driver, 1000);
        }
    }
}