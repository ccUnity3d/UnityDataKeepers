using System.Collections.Generic;

namespace UnityDataKeepersCore.Core.Backend
{
    internal class GoogleBackendConnector: IBackendConnector
    {
        public bool IsConnected { get; private set; }

        public string BackendId { get; }

        public string MainKeeperId { get; }

        public string RoleName { get; }

        public string AccessKey { get; }

        private IDataLoader _dataLoader;

        public GoogleBackendConnector(string backendId, string mainKeeperId, string roleName, string accessKey)
        {
            BackendId = backendId;
            MainKeeperId = mainKeeperId;
            RoleName = roleName;
            AccessKey = accessKey;
            IsConnected = false;
        }
        
        public List<KeeperSignature> GetSignatures()
        {
            if (!IsConnected)
                Connect();
            throw new System.NotImplementedException();
        }

        public void Connect()
        {
            var connectUrl = GoogleBackendUrlHelper.GetHandshakeUrl(BackendId, MainKeeperId, RoleName, AccessKey);
            var res = MakeQuery(connectUrl);
            if (GoogleBackendUrlHelper.IsHandshakeRequest(res))
                IsConnected = true;
        }

        private string MakeQuery(string connectUrl)
        {
            if (_dataLoader == null)
                _dataLoader = BackendConnectionFactory.GetHtmlLoader();
            return _dataLoader.LoadFromUrl(connectUrl);
        }
    }
}