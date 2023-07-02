using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SectorDamageGameAction : GameActionWithReleaser
{
    [Header("半径")]
    [SerializeField] public float radius;
    [Header("角度范围")]
    [SerializeField] public float angleRange_L;
    [SerializeField] public float angleRange_R;
    [Header("高度相差范围")]
    [SerializeField] public float heightRange_L;
    [SerializeField] public float heightRange_R;
    [Header("伤害数值")]
    public float damageValue;
    public float toughnessPenalty;

    protected override bool CheckActionComplete()
    {
        return true;
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        GameObject[] enermys = null;
        if (releaser.tag == "Player")
        {
            enermys = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else if (releaser.tag == "Enemy")
        {
            enermys = GameObject.FindGameObjectsWithTag("Player");
        }

        for (int i = 0; i < enermys.Length; i++)
        {
            if (IsInArea(enermys[i]))
            {
                EventCenter.GetInstance().EventTrigger<(GameObject, GameObject, float, float)>("伤害判定", (releaser, enermys[i], damageValue, toughnessPenalty));
            }
        }
    }

    bool IsInArea(GameObject enemy)
    {
        Vector3 enemyPos = enemy.transform.position;
        enemyPos = releaser.transform.InverseTransformPoint(enemyPos);

        //半径限制
        if (enemyPos.x * enemyPos.x + enemyPos.z * enemyPos.z > radius * radius)
        {
            return false;
        }

        //高度范围
        if (enemyPos.y < angleRange_L || enemyPos.y > angleRange_R)
        {
            return false;
        }

        //角度范围
        float angle = MathF.Atan2(enemyPos.x, enemyPos.z) * 180 / Mathf.PI;
        if (angle < angleRange_L || angle > angleRange_R)
        {
            return false;
        }

        return true;
    }
}
