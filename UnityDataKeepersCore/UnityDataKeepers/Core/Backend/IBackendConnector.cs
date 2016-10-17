using System.Collections.Generic;

namespace UnityDataKeepersCore.Core.Backend
{
    public interface IBackendConnector
    {
        bool IsConnected { get; }

        string BackendId { get; }

        string MainKeeperId { get; }

        string RoleName { get; }

        string AccessKey { get; }

        List<KeeperSignature> GetSignatures();

        void Connect();
    }
}