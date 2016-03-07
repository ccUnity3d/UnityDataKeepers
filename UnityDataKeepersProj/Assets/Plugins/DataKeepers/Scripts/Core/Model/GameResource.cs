public class GameResource : KeeperItem
{
    public static GameResource Dummy
    {
        get { return new GameResource() { Id = "default"}; }
    }

    public string Path = "Icons/Default";
    public string Description = "Empty";
}