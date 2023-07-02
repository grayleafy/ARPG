using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    protected GameObject holder;
    [HideInInspector] public bool isInteracting = false;   //正在攻击 

    public virtual void Init(GameObject holder)
    {
        this.holder = holder;
    }



    public virtual void Attack()
    {

    }

    public virtual void Aim()
    {

    }

    public virtual void Shoot()
    {

    }
}
