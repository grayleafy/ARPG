using System;
using System.Collections;
using System.Collections.Generic;



[Serializable]
public class ItemInfo
{
    public int id;

    public Item GetItem()
    {
        var data = DataManager.GetInstance().GetData();

        Item item = null;

        item = data.weapons.Find(x => x.id == id);
        if (item == null)
        {
            item = data.usableItems.Find(x => x.id == id);
        }

        return item;
    }
}


[Serializable]
public class CharacterBag
{
    public List<ItemInfo> weapons = new();

    public List<ItemInfo> usableItems = new();
}

