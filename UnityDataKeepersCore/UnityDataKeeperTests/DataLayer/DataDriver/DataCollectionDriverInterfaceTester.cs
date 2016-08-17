using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeeperTests.DataLayer.DataDriver
{
    public class DataCollectionDriverInterfaceTester<TDriver, TItem>
        where TDriver : IDataCollectionDriver<TItem>
        where TItem : class, IDataItem, new()
    {

        public void IsEmptyAndInInitialState(TDriver driver)
        {
            Assert.IsNotNull(driver, "driver can't be null at initial state");
            Assert.AreEqual(0,
                driver.Count(),
                "driver haven't items at initial state");
            Assert.IsNotNull(driver.GetAll(),
                "driver need to return empty collection (not null)");
            Assert.AreEqual(0,
                driver.GetAll().Count(),
                "driver need to return empty collection");
        }

        public void AddTest_Good(TDriver driver)
        {
            var count = driver.Count();
            Assert.IsTrue(driver.Add(new TItem()),
                "driver need to return true at adding");
            Assert.AreEqual(count + 1,
                driver.Count(),
                "count need increment when adding");
        }

        public void AddTest_AddNull(TDriver driver)
        {
            var count = driver.Count();
            Assert.IsFalse(driver.Add((TItem)null),
                "return 'true' when added null - need false");
            Assert.AreEqual(count,
                driver.Count(),
                "count need have initial state when adding 'null' item");
        }

        public void AddTest_AddItemTwice(TDriver driver)
        {
            var count = driver.Count();
            var item = new TItem();
            var item2 = new TItem();
            Assert.IsTrue(driver.Add(item));
            Assert.IsTrue(driver.Add(item2));
            Assert.AreEqual(count + 2, driver.Count());
        }

        public void AddMultipleTest_Good(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list = Enumerable.Repeat(0, addCount).Select(i => new TItem());
            Assert.AreEqual(addCount,
                driver.Add(list),
                "driver need to return new elements count whem adding");
            Assert.AreEqual(count + addCount,
                driver.Count(),
                "driver items count need be increased to elements count whet collection added");
        }

        public void AddMultipleTest_AddNullItems(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list =
                Enumerable.Repeat(0, addCount)
                    .Select(i => new TItem())
                    .Select((item, n) => n%2 == 0
                        ? null
                        : item).ToList();
            var goodCount = list.Count(i => i != null);
            Assert.AreEqual(goodCount,
                driver.Add(list),
                "need to return count of added items (null items don't need to calculated)");
            Assert.AreEqual(count + goodCount,
                driver.Count(),
                "need to return count of added items (null items don't need to calculated)");
        }

        public void AddMultipleTest_AddNullCollection(TDriver driver)
        {
            var count = driver.Count();
            Assert.AreEqual(0,
                driver.Add((IEnumerable<TItem>)null),
                "need retirn 0 when trying to add 'null' collection");
            Assert.AreEqual(count,
                driver.Count(),
                "count don't need to change when added 'null' collection");
        }

        public void AddMultipleTest_DoubleAddItems(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list =
                Enumerable.Repeat(0, addCount)
                    .Select(i => new TItem())
                    .Select((item, n) => n%2 == 0
                        ? null
                        : item).ToList();
            var goodCount = list.Count(i => i != null);
            Assert.AreEqual(goodCount, driver.Add(list));
            Assert.AreEqual(0, driver.Add(list));
            Assert.AreEqual(count + goodCount,
                driver.Count(),
                "don't need to add same elements");
        }

        public void GetByHashTest_Good(TDriver driver)
        {
            var item = new TItem();
            driver.Add(item);
            Assert.AreEqual(item,
                driver.GetByGuid(item.Guid),
                "need to return previous added item");
        }

        public void GetByHashTest_PushNull(TDriver driver)
        {
            var item = new TItem();
            Assert.AreEqual(null,
                driver.GetByGuid(item.Guid),
                "need to return null if item not in collection");
        }

        public void RemoveTest_Good(TDriver driver)
        {
            var item = new TItem();
            driver.Add(item);
            var count = driver.Count();
            Assert.IsTrue(driver.Remove(item),
                "need to return true when removed item");
            Assert.AreEqual(count - 1,
                driver.Count(),
                "need decrease items count when removing");
        }

        public void RemoveTest_DoubleRemove(TDriver driver)
        {
            var item = new TItem();
            driver.Add(item);
            var count = driver.Count();
            Assert.IsTrue(driver.Remove(item),
                "need to return true when removed item");
            Assert.IsFalse(driver.Remove(item),
                "need to return false when don't removed item");
            Assert.AreEqual(count - 1,
                driver.Count(),
                "need decrease items count when removing");
        }

        public void RemoveTest_PushNull(TDriver driver)
        {
            Assert.IsFalse(driver.Remove((TItem)null),
                "need to return false when argument is null");
        }

        public void RemoveMultipleTest_Good(TDriver driver)
        {
            var itemsCount = 500;
            var items =
                Enumerable.Repeat(0, itemsCount)
                    .Select(i => new TItem())
                    .ToList();
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(itemsCount, driver.Remove(items));
            Assert.AreEqual(count - itemsCount, driver.Count());
        }

        public void RemoveMultipleTest_PushNullItems(TDriver driver)
        {
            var itemsCount = 500;
            var items =
                Enumerable.Repeat(0, itemsCount)
                    .Select(i => new TItem())
                    .Select((i, n) => n%2 == 0
                        ? null
                        : i).ToList();
            var goodCount = items.Count(i => i != null);
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(goodCount, driver.Remove(items),
                "need break null items");
            Assert.AreEqual(count - goodCount, driver.Count());
        }

        public void RemoveMultipleTest_DoubleRemove(TDriver driver)
        {
            var itemsCount = 500;
            var items =
                Enumerable.Repeat(0, itemsCount)
                    .Select(i => new TItem())
                    .ToList();
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(itemsCount, driver.Remove(items),
                "need break null items");
            Assert.AreEqual(0, driver.Remove(items),
                "need not existing items");
            Assert.AreEqual(count - itemsCount, driver.Count());
        }

        public void RemoveMultipleTest_PushNullCollection(TDriver driver)
        {
            var count = driver.Count();
            Assert.AreEqual(0, driver.Remove((IEnumerable<TItem>)null),
                "need return zero when added null collection");
            Assert.AreEqual(count, driver.Count());
        }

        public void CountTest(TDriver driver)
        {
            var item = new TItem();
            Assert.AreEqual(driver.Count(), driver.GetAll().Count());
            driver.Add(item);
            Assert.AreEqual(driver.Count(), driver.GetAll().Count());
            driver.Remove(item);
            Assert.AreEqual(driver.Count(), driver.GetAll().Count());
        }

        public void GetAllTest(TDriver driver)
        {
            IsEmptyAndInInitialState(driver);
            var itemsCount = 500;
            var items =
                Enumerable.Repeat(0, itemsCount)
                    .Select(i => new TItem())
                    .ToList();
            driver.Add(items);
            var all = driver.GetAll().ToArray();
            Assert.AreEqual(items.Count,all.Count());
            for (var i = 0; i < all.Count(); i++)
            {
                Assert.IsTrue(all.Any(item => item.Equals(items[i])));
            }
        }

        public void ClearTest(TDriver driver)
        {
            driver.Clear();
            Assert.AreEqual(0, driver.Count());
            var itemsCount = 500;
            var items = Enumerable.Repeat(new TItem(), itemsCount).ToList();
            driver.Add(items);
            driver.Clear();
            Assert.AreEqual(0, driver.Count());
        }

        public void UpdateTest_SimpleUpdate(TDriver driver, Func<TItem, TItem> updateFunction)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            item = updateFunction(item);
            Assert.IsTrue(driver.Update(item));
            Assert.AreEqual(item,driver.GetByGuid(item.Guid), "item isn't updated");
        }

        public void UpdateTest_PushNull(TDriver driver)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            Assert.IsFalse(driver.Update(null), "need return 'false' when item is null");
            Assert.AreEqual(item, driver.GetByGuid(item.Guid), "item was updated");
        }

        public void UpdateTest_UpdateNotMidifiedItem(TDriver driver)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            Assert.IsTrue(driver.Update(item));
            Assert.AreEqual(item, driver.GetByGuid(item.Guid));
        }

        public void UpdateTest_UpdateItemNotInCollection(TDriver driver)
        {
            var item = new TItem();
            Assert.IsFalse(driver.Update(item),"item isn't at collection - need return false");
            Assert.AreNotEqual(item, driver.GetByGuid(item.Guid), "'update' added item to collection, don't need to do this");
        }

        public void UniqueHashesSimpleAdd(TDriver driver, int addCount)
        {
            for (var i = 0; i < addCount; i++)
            {
                driver.Add(new TItem());
            }
            Assert.AreEqual(driver.Count(),
                driver.GetAll().Distinct().Count());
        }

        public void UniqueHashesListAdd(TDriver driver, int addCount)
        {
            var list =
                Enumerable.Repeat(0, addCount)
                    .Select(
                        i => new TItem())
                    .ToArray();
            Assert.AreEqual(list.Length, driver.Add(list));
            var uniqueCount = driver.GetAll().Select(i=>i.Guid).Distinct().Count();
            Assert.AreEqual(driver.Count(),uniqueCount);
        }

        public void Add_Samereferences(TDriver driver, int addCount)
        {
            var list = Enumerable.Repeat(new TItem(), addCount).ToArray();
            Assert.IsTrue(driver.Add(list[0]));
            for (var i = 1; i < list.Length; i++)
            {
                var item = list[i];
                Assert.IsFalse(driver.Add(item));
            }
        }

        public void AddMultiple_SameReferences(TDriver driver, int addCount)
        {
            var list = Enumerable.Repeat(new TItem(), addCount).ToArray();
            Assert.AreEqual(1, driver.Add(list));
        }
    }
}