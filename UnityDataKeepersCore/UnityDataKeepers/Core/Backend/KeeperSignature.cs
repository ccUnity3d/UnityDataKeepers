using System.Collections.Generic;

namespace UnityDataKeepersCore.Core.Backend
{
    public class KeeperSignature
    {
        public string KeeperName { get; }

        public List<KeeperSignatureField> Fields { get; }
    }
}