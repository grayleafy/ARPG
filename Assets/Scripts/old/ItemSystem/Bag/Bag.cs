using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] public List<ItemInfo> consumables = new List<ItemInfo>();
    [SerializeField] public List<ItemInfo> equips = new List<ItemInfo>();
    [SerializeField] public List<ItemInfo> props = new List<ItemInfo>();
}
