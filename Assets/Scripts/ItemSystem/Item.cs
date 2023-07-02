using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string name;
    public int id;

    Sprite _icon = null;
    [HideInInspector]
    [JsonIgnore]
    public Sprite icon
    {
        get
        {
            if (_icon == null)
            {
                _icon = ResMgr.GetInstance().Load<Sprite>(iconName);
            }
            return _icon;
        }
    }
    public string iconName;
    [SerializeField, JsonProperty] string toolTip;
    public virtual string ToolTip
    {
        get
        {
            return toolTip;
        }
    }


    public int purchasePrice;
}
