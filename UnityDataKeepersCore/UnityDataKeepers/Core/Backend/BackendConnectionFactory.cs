namespace UnityDataKeepersCore.Core.Backend
{
    public static class BackendConnectionFactory
    {
        public static IDataLoader GetHtmlLoader()
        {
            return new HtmlLoader();
        }

        public static IBackendConnector GetBackendConnector(string backendId, string mainKeeperId, string roleName, string accessKey)
        {
            return new BackendConnector(backendId, mainKeeperId, roleName, accessKey);
        }
    }
}