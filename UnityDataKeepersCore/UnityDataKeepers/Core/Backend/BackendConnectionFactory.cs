namespace UnityDataKeepersCore.Core.Backend
{
    public static class BackendConnectionFactory
    {
        public static IBackendConnection GetHtmlLoader()
        {
            return new HtmlLoader();
        }
    }
}