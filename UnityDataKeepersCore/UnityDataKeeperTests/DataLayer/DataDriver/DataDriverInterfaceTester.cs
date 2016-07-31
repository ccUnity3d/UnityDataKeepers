using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver;
using Random = UnityEngine.Random;

namespace UnityDataKeeperTests.BlsackBox
{
    public class DataDriverInterfaceTester<TDriver, TItem>
        where TDriver : IRevisionDataDriver<TItem>
        where TItem : class, IDataItem, IComparer<TItem>, new()
    {

        public void IsInInitialState(TDriver driver)
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

        public void AddTest1(TDriver driver)
        {
            var count = driver.Count();
            Assert.IsTrue(driver.Add(new TItem()),
                "driver need to return true at adding");
            Assert.AreEqual(count + 1,
                driver.Count(),
                "count need increment when adding");
        }

        public void AddTest2(TDriver driver)
        {
            var count = driver.Count();
            Assert.IsFalse(driver.Add((TItem)null),
                "return 'true' when added null - need false");
            Assert.AreEqual(count,
                driver.Count(),
                "count need have initial state when adding 'null' item");
        }

        public void AddTest3(TDriver driver)
        {
            var count = driver.Count();
            var item = new TItem();
            Assert.IsTrue(driver.Add(item),
                "driver need to return true at adding");
            Assert.IsFalse(driver.Add(item),
                "driver need to return false when same element adding");
            Assert.AreEqual(count + 1,
                driver.Count(),
                "count need increment for 1 when adding few same items");
        }

        public void AddMultipleTest1(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list = Enumerable.Repeat(new TItem(), addCount);
            Assert.AreEqual(addCount,
                driver.Add(list),
                "driver need to return new elements count whem adding");
            Assert.AreEqual(count + addCount,
                driver.Count(),
                "driver items count need be increased to elements count whet collection added");
        }

        public void AddMultipleTest2(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list =
                Enumerable.Repeat(new TItem(), addCount)
                    .Select((item, n) => n % 2 == 0
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

        public void AddMultipleTest3(TDriver driver)
        {
            var count = driver.Count();
            Assert.AreEqual(0,
                driver.Add((IEnumerable<TItem>)null),
                "need retirn 0 when trying to add 'null' collection");
            Assert.AreEqual(count,
                driver.Count(),
                "count don't need to change when added 'null' collection");
        }

        public void AddMultipleTest4(TDriver driver)
        {
            var count = driver.Count();
            var addCount = 500;
            var list =
                Enumerable.Repeat(new TItem(), addCount)
                    .Select((item, n) => n % 2 == 0
                        ? null
                        : item).ToList();
            var goodCount = list.Count(i => i != null);
            Assert.AreEqual(goodCount,
                driver.Add(list),
                "need to return count of added items (null items don't need to calculated)");
            Assert.AreEqual(0,
                driver.Add(list),
                "don't need to add same elements");
            Assert.AreEqual(count + goodCount,
                driver.Count(),
                "don't need to add same elements");
        }

        public void GetByHashTest1(TDriver driver)
        {
            var item = new TItem();
            driver.Add(item);
            Assert.AreEqual(item,
                driver.GetByHash(item.Hash),
                "need to return previous added item");
        }

        public void GetByHashTest2(TDriver driver)
        {
            var item = new TItem();
            Assert.AreEqual(null,
                driver.GetByHash(item.Hash),
                "need to return null if item not in collection");
        }

        public void RemoveTest1(TDriver driver)
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

        public void RemoveTest2(TDriver driver)
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

        public void RemoveTest3(TDriver driver)
        {
            Assert.IsFalse(driver.Remove((TItem)null),
                "need to return false when argument is null");
        }

        public void RemoveMultipleTest1(TDriver driver)
        {
            var itemsCount = 500;
            var items = Enumerable.Repeat(new TItem(), itemsCount).ToList();
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(itemsCount, driver.Remove(items));
            Assert.AreEqual(count - itemsCount, driver.Count());
        }

        public void RemoveMultipleTest2(TDriver driver)
        {
            var itemsCount = 500;
            var items = Enumerable.Repeat(new TItem(), itemsCount).Select((i, n) => n % 2 == 0 ? null : i).ToList();
            var goodCount = items.Count(i => i != null);
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(goodCount, driver.Remove(items),
                "need break null items");
            Assert.AreEqual(count - goodCount, driver.Count());
        }

        public void RemoveMultipleTest3(TDriver driver)
        {
            var itemsCount = 500;
            var items = Enumerable.Repeat(new TItem(), itemsCount).ToList();
            driver.Add(items);
            var count = driver.Count();
            Assert.AreEqual(itemsCount, driver.Remove(items),
                "need break null items");
            Assert.AreEqual(0, driver.Remove(items),
                "need not existing items");
            Assert.AreEqual(count - itemsCount, driver.Count());
        }

        public void RemoveMultipleTest4(TDriver driver)
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
            IsInInitialState(driver);
            var itemsCount = 500;
            var items = Enumerable.Repeat(new TItem(), itemsCount).ToList();
            driver.Add(items);
            var all = driver.GetAll().ToArray();
            Assert.AreEqual(items.Count,all.Count());
            for (var i = 0; i < all.Count(); i++)
            {
                Assert.AreEqual(items[i], all[i]);
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

        public void UpdateTest1(TDriver driver, Func<TItem, TItem> updateFunction)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            item = updateFunction(item);
            Assert.IsTrue(driver.Update(item));
            Assert.AreEqual(item,driver.GetByHash(item.Hash), "item isn't updated");
        }

        public void UpdateTest2(TDriver driver)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            Assert.IsFalse(driver.Update(null), "need return 'false' when item is null");
            Assert.AreEqual(item, driver.GetByHash(item.Hash), "item was updated");
        }

        public void UpdateTest3(TDriver driver)
        {
            var item = new TItem();
            Assert.IsTrue(driver.Add(item), "can't add test item to collection");
            Assert.IsTrue(driver.Update(item));
            Assert.AreEqual(item, driver.GetByHash(item.Hash));
        }

        public void UpdateTest4(TDriver driver)
        {
            var item = new TItem();
            Assert.IsFalse(driver.Update(item),"item isn't at collection - need return false");
            Assert.AreNotEqual(item, driver.GetByHash(item.Hash), "'update' added item to collection, don't need to do this");
        }
    }
}