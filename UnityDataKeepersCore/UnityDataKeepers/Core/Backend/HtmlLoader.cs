#define DOT_NET_WWW

using System.Net;

namespace UnityDataKeepersCore.Core.Backend
{
    internal class HtmlLoader: IBackendConnection
    {
        public string LoadFromUrl(string url)
        {
#if DOT_NET_WWW
            using (WebClient client = new WebClient())
            {
                var htmlCode = client.DownloadString(url);
                return htmlCode;
            }
#else
            throw new NotImplementedException("Unity WWW not suported yet...");
#endif
        }
    }
}
