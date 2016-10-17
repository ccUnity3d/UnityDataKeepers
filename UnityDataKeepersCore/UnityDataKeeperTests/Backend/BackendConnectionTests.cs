using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDataKeepersCore.Core.Backend;

namespace UnityDataKeeperTests.Backend
{
    [TestClass]
    public class BackendConnectionTests
    {
        // handshake:
        // https://script.google.com/macros/s/AKfycbwUC5g50TQqolQUi6jxp_v1MNOwM3zLh3DrNeGJmUd7GmXLj84/exec?role=Dev&accesKey=key1234567894&mainKeeper=1YAl_iXCqnSYOoiRcUBBc7idT4mmQDG811bIt3dPD8-4&query=ping

        private const string BackendId = "AKfycbwUC5g50TQqolQUi6jxp_v1MNOwM3zLh3DrNeGJmUd7GmXLj84";
        private const string MainKeeperId = "1YAl_iXCqnSYOoiRcUBBc7idT4mmQDG811bIt3dPD8-4";
        private const string RoleName = "Dev";
        private const string AccessKey = "key1234567894";

        [TestMethod]
        public void GetConnector()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector(BackendId, MainKeeperId, RoleName, AccessKey);
            Assert.IsNotNull(connector);
        }

        [TestMethod]
        public void TryConnect()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector(BackendId, MainKeeperId, RoleName, AccessKey);
            Assert.IsFalse(connector.IsConnected);
            connector.Connect();
            Assert.IsTrue(connector.IsConnected);
        }

        [TestMethod]
        public void Connect_BadBackendId()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector("ksjdfg", MainKeeperId, RoleName, AccessKey);
            Assert.IsFalse(connector.IsConnected);
            connector.Connect();
            Assert.IsFalse(connector.IsConnected);
        }

        [TestMethod]
        public void Connect_BadKeeperId()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector(BackendId, "sadf", RoleName, AccessKey);
            Assert.IsFalse(connector.IsConnected);
            connector.Connect();
            Assert.IsFalse(connector.IsConnected);
        }

        [TestMethod]
        public void Connect_BadRole()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector(BackendId, MainKeeperId, "akljsfg", AccessKey);
            Assert.IsFalse(connector.IsConnected);
            connector.Connect();
            Assert.IsFalse(connector.IsConnected);
        }

        [TestMethod]
        public void Connect_BadAccessKey()
        {
            var connector = BackendConnectionFactory.GetGoogleBackendConnector(BackendId, MainKeeperId, RoleName, "lskdfjga");
            Assert.IsFalse(connector.IsConnected);
            connector.Connect();
            Assert.IsFalse(connector.IsConnected);
        }
    }
}
