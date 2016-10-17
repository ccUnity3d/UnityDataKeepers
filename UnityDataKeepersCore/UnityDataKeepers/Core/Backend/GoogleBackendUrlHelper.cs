namespace UnityDataKeepersCore.Core.Backend
{
    internal static class GoogleBackendUrlHelper
    {
        public static string GetHandshakeUrl(string backendId, string mainKeeperId, string roleName, string accessKey)
        {
            return string.Format("https://script.google.com/macros/s/{0}/exec?role={1}&accesKey={2}&mainKeeper={3}&query=ping",
                backendId,
                roleName,
                accessKey,
                mainKeeperId);
        }

        public static bool IsHandshakeRequest(string request)
        {
            return request.Equals("pong");
        }
    }
}