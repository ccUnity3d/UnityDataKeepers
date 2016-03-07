using UnityEngine;

public class GameResourcesKeeper : Keeper<GameResourcesKeeper, GameResource>
{
    protected override bool EnableSelfLoading
    {
        get { return true; }
    }

    protected override bool EnableStaticLoading
	{
		get { return true; }
	}

    public Sprite GetSpriteById(string Id)
    {
        var resource = GetById(Id);
        return resource == null ? Resources.Load<Sprite>("Icons/Default") : Resources.Load<Sprite>(resource.Path);
    }
}