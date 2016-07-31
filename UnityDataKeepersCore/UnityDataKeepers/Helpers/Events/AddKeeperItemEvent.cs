using UnityDataKeepersCore.Core.Model;
using UnityEngine.Events;

namespace UnityDataKeepersCore.Core.Events
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
