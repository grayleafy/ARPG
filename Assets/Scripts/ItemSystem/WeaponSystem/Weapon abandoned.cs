//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
//using UnityEngine;

//[Serializable]
//public class Weapon : Item
//{
//    [SerializeField] public string prefabName;
//    [SerializeField] public string holdAnimationName;
//    [SerializeField] public string rigAnimationName;

//    [SerializeReference, SubclassSelector] public List<GameAction> attackActionList = new();
//    [SerializeReference, SubclassSelector] public GameAction AimAction;
//    [SerializeReference, SubclassSelector] public GameAction shootAction;

//    [Header("攻击序列相关")]
//    [SerializeField] public float resetAttackSegmentTime = 1;
//    int currentAttackSegment = -1;
//    float timeToReset = 0;

//    public void Init(GameObject character)
//    {
//        if (attackActionList == null) { return; }
//        for (int i = 0; i < attackActionList.Count; i++)
//        {
//            ((PlayAnimationGameAction)attackActionList[i]).SetCharacter(character);
//        }

//        MonoMgr.GetInstance().AddUpdateListener(UpdateAttackSegment);
//    }

//    public void Attack()
//    {
//        if (attackActionList == null || attackActionList.Count == 0) return;
//        if (currentAttackSegment < 0 || attackActionList[currentAttackSegment].IsCompleted())
//        {
//            currentAttackSegment++;
//            currentAttackSegment %= attackActionList.Count;
//            attackActionList[currentAttackSegment].StartExecute();
//            timeToReset = resetAttackSegmentTime;
//        }
//    }

//    public void Aim()
//    {

//    }

//    //更新攻击段数
//    void UpdateAttackSegment()
//    {
//        if (attackActionList == null || attackActionList.Count == 0) return;
//        if (currentAttackSegment >= 0 && attackActionList[currentAttackSegment].IsCompleted())
//        {
//            timeToReset -= Time.deltaTime;
//            if (timeToReset < 0)
//            {
//                currentAttackSegment = -1;
//            }
//        }

//    }

//    //设置输入
//    public void EquipAsMeleeWeapon()
//    {

//    }

//    public void EquipAsRangedWeapon()
//    {

//    }
//}
