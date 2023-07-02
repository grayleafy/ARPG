using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SwordWeaponController : WeaponController
{


    [Header("攻击序列相关")]
    [SerializeReference, SubclassSelector] public List<SkillGameAction> attackActionList = new();
    [SerializeField] float resetAttackSegmentTime = 1;
    int currentAttackSegment = -1;
    float timeToReset = 0;

    private void Update()
    {
        UpdateAttackSegment();
    }

    public override void Init(GameObject holder)
    {
        base.Init(holder);
        //设置gameobject
        if (attackActionList == null) { return; }
    }

    public override void Attack()
    {
        base.Attack();
        //if (attackActionList == null || attackActionList.Count == 0) return;
        //if (currentAttackSegment < 0 || attackActionList[currentAttackSegment].IsCompleted())
        //{
        //    currentAttackSegment++;
        //    currentAttackSegment %= attackActionList.Count;
        //    holder.GetComponent<NPCController>()?.CastSkill(attackActionList[currentAttackSegment]);
        //    timeToReset = resetAttackSegmentTime;
        //}
    }

    //更新攻击段数
    void UpdateAttackSegment()
    {
        if (attackActionList == null || attackActionList.Count == 0) return;
        if (currentAttackSegment >= 0 && attackActionList[currentAttackSegment].IsCompleted())
        {
            timeToReset -= Time.deltaTime;
            if (timeToReset < 0)
            {
                currentAttackSegment = -1;
            }
        }

    }
}
