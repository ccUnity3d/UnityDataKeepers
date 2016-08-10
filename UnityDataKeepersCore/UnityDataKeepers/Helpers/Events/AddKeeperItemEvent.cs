using UnityDataKeepersCore.Core.TMP.Model;
using UnityEngine.Events;

namespace UnityDataKeepersCore.Helpers.Events
{
    public class AddKeeperItemEvent : UnityEvent<IDataKeeperItem>
    {
    }

    public class DeleteKeeperItemEvent : UnityEvent<IDataKeeperItem>
    {
    }

    public class UpdateKeeperItemEvent : UnityEvent<IDataKeeperItem>
    {
        
    }
}
