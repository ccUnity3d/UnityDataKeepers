using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.Backend;

namespace UnityDataKeeperTests.Backend
{
    [TestClass]
    public class HtmlLoaderTests
    {
        [TestMethod]
        public void GetLoader()
        {
            var loader = BackendConnectionFactory.GetHtmlLoader();
            Assert.IsNotNull(loader);
        }

        [TestMethod]
        public void GetTextFromUrl()
        {
            var con = BackendConnectionFactory.GetHtmlLoader();
            var data = con.LoadFromUrl("http://stackoverflow.com/");
            Console.WriteLine(data);
            Assert.IsNotNull(data);
        }
    }
}
