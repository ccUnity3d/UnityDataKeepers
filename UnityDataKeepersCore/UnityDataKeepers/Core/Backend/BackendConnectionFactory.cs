namespace UnityDataKeepersCore.Core.Backend
{
    public static class BackendConnectionFactory
    {
        public static IDataLoader GetHtmlLoader()
        {
            return new HtmlLoader();
        }

        public static IBackendConnector GetGoogleBackendConnector(string backendId, string mainKeeperId, string roleName, string accessKey)
        {
            return new GoogleBackendConnector(backendId, mainKeeperId, roleName, accessKey);
        }
    }
}