using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UsableItem : Item
{
    [SerializeReference, SubclassSelector] public BehaviorTreeNode useBehavior;
}
