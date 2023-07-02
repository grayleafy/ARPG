using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleHit : MonoBehaviour
{
    public CharacterTag characterTag;
    public float radius;
    public float damageValue;
    public float toughnessPenalty;
    public float lastingTime;
    public float IntervalTime;

    float lastDamageTime;
    float startTime;



    private void OnEnable()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        startTime = Time.time;
        lastDamageTime = Time.time - IntervalTime;
    }

    private void Update()
    {
        float t = Time.time;
        if (t - startTime >= lastingTime)
        {
            PoolMgr.GetInstance().PushObj("Magics/Meteors", gameObject);
            return;
        }

        if (t - lastDamageTime >= IntervalTime)
        {
            lastDamageTime = t;

            GameObject[] targets = null;
            if (characterTag == CharacterTag.Enemy)
            {
                List<GameObject> temp = new();
                temp = GameObject.FindGameObjectsWithTag("Player").ToList();
                temp.AddRange(GameObject.FindGameObjectsWithTag("Ally").ToList());
                targets = temp.ToArray();
            }
            else if (characterTag == CharacterTag.Player || characterTag == CharacterTag.Ally)
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
            }

            if (targets != null)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if ((targets[i].transform.position - transform.position).magnitude <= radius)
                    {
                        Debug.Log("魔法判断");
                        EventCenter.GetInstance().EventTrigger<(GameObject, GameObject, float, float)>("伤害判定", (gameObject, targets[i], damageValue, toughnessPenalty));
                    }
                }
            }
        }
    }
}
