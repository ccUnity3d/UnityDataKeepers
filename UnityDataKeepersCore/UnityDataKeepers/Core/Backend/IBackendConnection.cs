namespace UnityDataKeepersCore.Core.Backend
{
    public interface IBackendConnection
    {
        string LoadFromUrl(string url);
    }
}