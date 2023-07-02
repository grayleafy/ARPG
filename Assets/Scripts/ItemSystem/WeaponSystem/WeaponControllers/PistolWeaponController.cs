using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolWeaponController : WeaponController
{
    [SerializeField] Transform muzzle;
    [SerializeField] List<ParticleSystem> fireEffects;
    [SerializeField] List<ParticleSystem> hitEffects;
    [SerializeField] Transform hitTransform;
    [SerializeField] TrailRenderer bulletTrailRenderer;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float damage = 10f;

    RaycastHit raycastHit;




    public override void Shoot()
    {
        base.Shoot();

        for (int i = 0; i < fireEffects.Count; i++)
        {
            fireEffects[i].Emit(1);
        }

        if (Physics.Raycast(muzzle.position, muzzle.forward, out raycastHit))
        {
            StartCoroutine(ShootBullet(muzzle.position, raycastHit.point, raycastHit.normal));
        }
        else
        {
            Vector3 destination = muzzle.position + 50f * muzzle.forward;
            Vector3 normal = -muzzle.forward;
            StartCoroutine(ShootBullet(muzzle.position, destination, normal));
        }

    }

    IEnumerator ShootBullet(Vector3 origin, Vector3 destination, Vector3 normal)
    {
        Vector3 pos = origin;
        var bullet = Instantiate(bulletTrailRenderer, muzzle.position, Quaternion.identity);
        bullet.AddPosition(muzzle.position);
        bullet.transform.position = raycastHit.point;

        Vector3 v = (destination - origin).normalized * bulletSpeed;
        float totalTime = (destination - origin).magnitude / bulletSpeed;
        float t = totalTime;

        while (true)
        {
            t -= Time.deltaTime;
            pos += v * Time.deltaTime;
            bullet.transform.position = pos;
            if (t <= 0) break;
            yield return null;
        }
        GameObject.Destroy(bullet.gameObject);

        hitTransform.position = destination;
        Quaternion rot = Quaternion.LookRotation(normal);
        hitTransform.rotation = rot;
        for (int i = 0; i < hitEffects.Count; i++)
        {
            hitEffects[i].Emit(1);
        }


        if (Physics.Raycast(origin, destination - origin, out raycastHit))
        {
            EventCenter.GetInstance().EventTrigger<(GameObject, GameObject, float, float)>("伤害判定", (holder, GameManager.GetInstance().GetNPCController(raycastHit.collider), damage, 1));
        }
    }
}
