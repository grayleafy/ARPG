using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon : Item
{
    //[SerializeField] public string name;
    //[SerializeField] public int id;
    [SerializeField] public string prefabName;

    [SerializeField]
    public WeaponType type;
    public enum WeaponType
    {
        MainWeapon,
        SecondaryWeapon
    }

    [SerializeReference, SubclassSelector] public BT.BehaviorTreeNode attackBehavior;
    [SerializeReference, SubclassSelector] public BT.BehaviorTreeNode aimBehavior;
    [SerializeReference, SubclassSelector] public BT.BehaviorTreeNode shootBehavior;
}
