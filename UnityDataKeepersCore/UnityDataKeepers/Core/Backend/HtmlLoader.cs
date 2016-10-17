#define DOT_NET_WWW

using System;
using System.Net;

namespace UnityDataKeepersCore.Core.Backend
{
    internal class HtmlLoader: IDataLoader
    {
        public string LoadFromUrl(string url)
        {
#if DOT_NET_WWW
            using (WebClient client = new WebClient())
            {
                try
                {
                    var htmlCode = client.DownloadString(url);
                    return htmlCode;
                }
                catch (System.Net.WebException e)
                {
                    return string.Format("Loading failed, status {0}", e.Status);
                }
            }
#else
            throw new NotImplementedException("Unity WWW not suported yet...");
#endif
        }
    }
}
