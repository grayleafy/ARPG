using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public enum WeaponHoldType
{
    MainWeapon,
    SecondaryWeapon,
    HoldNothing,
}

public class WeaponHoldController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator mainRigAnimator;
    [SerializeField] Transform mainWeaponHolder;
    [SerializeField] Animator secondaryRigAnimator;
    [SerializeField] Transform secondaryWeaponHolder;



    //[SerializeField] int mainWeaponID;
    //[SerializeField] int secondaryWeaponID;

    public WeaponHoldType currentHold = WeaponHoldType.HoldNothing;

    WeaponInteractController mainWeapon;
    WeaponInteractController secondaryWeapon;
    [Header("时间设置")]
    [SerializeField] float keepHoldTime = 5;
    [SerializeField] float advanceTime = 0.3f;
    float leftHoldTime = 0;
    //bool isAttacking = false;   //攻击协程，只能存在一个
    bool isAiming = false;



    [Header("瞄准设置")]
    //[SerializeField] float aimFollowHalfTime = 0.2f;
    [SerializeField] MultiAimConstraint aimConstraint;
    [SerializeField] RigBuilder rigBuilder;
    LocomotionController locomotionController;
    NPCController npcController;
    [SerializeField] Transform rigAimTarget;
    [SerializeField] Transform aimTarget;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 0.7f, 0);

    private void Awake()
    {
        locomotionController = GetComponent<LocomotionController>();
        npcController = GetComponent<NPCController>();
        //StartListenInput();

    }

    private void Start()
    {
        LoadWeapon();
    }

    private void Update()
    {
        leftHoldTime -= Time.deltaTime;
        if (leftHoldTime <= 0)
        {
            if (currentHold == WeaponHoldType.MainWeapon)
            {
                RetractWeapon(WeaponHoldType.MainWeapon);
            }
            else if (currentHold == WeaponHoldType.SecondaryWeapon)
            {
                RetractWeapon(WeaponHoldType.SecondaryWeapon);
            }
        }
    }

    private void LateUpdate()
    {
        if (aimTarget != null)
        {
            rigAimTarget.position = aimTarget.position + aimOffset;
        }
    }

    public void StartListenInput()
    {
        InputMgr.GetInstance().AddListener(InputEvent.MouseLeftDown, () =>
        {
            Attack(WeaponHoldType.MainWeapon);
        }, true);
        InputMgr.GetInstance().AddListener(InputEvent.MouseRightDown, () => Aim(WeaponHoldType.SecondaryWeapon), true);

        SetAimTarget(InputMgr.GetInstance().GetAimPoint());
    }

    public void StopListenInput()
    {
        InputMgr.GetInstance().RemoveListener(InputEvent.MouseLeftDown, () =>
        {
            Attack(WeaponHoldType.MainWeapon);
        });
        InputMgr.GetInstance().RemoveListener(InputEvent.MouseRightDown, () => Aim(WeaponHoldType.SecondaryWeapon));
    }

    public void LoadWeapon()
    {
        ClearWeapon();
        int mainWeaponID = npcController.info.mainWeaponID;
        if (mainWeaponID > 0)
        {
            var mainWeaponInfo = DataManager.GetInstance().GetData().weapons.Find(x => x.id == mainWeaponID);
            ResMgr.GetInstance().LoadAsync<GameObject>(mainWeaponInfo.prefabName, (o) =>
            {
                mainWeapon = o.GetComponent<WeaponInteractController>();
                mainWeapon.Init.Invoke(gameObject);
                EquipWeapon(WeaponHoldType.MainWeapon);
            }, mainWeaponHolder);
        }



        int secondaryWeaponID = npcController.info.secondaryWeaponID;
        if (secondaryWeaponID > 0)
        {
            var secondaryWeaponInfo = DataManager.GetInstance().GetData().weapons.Find(x => x.id == secondaryWeaponID);
            ResMgr.GetInstance().LoadAsync<GameObject>(secondaryWeaponInfo.prefabName, (o) =>
            {
                secondaryWeapon = o.GetComponent<WeaponInteractController>();
                secondaryWeapon.Init.Invoke(gameObject);
                EquipWeapon(WeaponHoldType.SecondaryWeapon);
            }, secondaryWeaponHolder);
        }
    }

    public void ClearWeapon()
    {
        for (int i = 0; i < mainWeaponHolder.childCount; i++)
        {
            var child = mainWeaponHolder.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < secondaryWeaponHolder.childCount; i++)
        {
            var child = secondaryWeaponHolder.GetChild(i);
            Destroy(child.gameObject);
        }
    }


    void EquipWeapon(WeaponHoldType holdType)
    {
        if (holdType == WeaponHoldType.MainWeapon)
        {
            mainRigAnimator.Play(mainWeapon.rigAnimationName + "_Equip");
            animator.CrossFade("HoldNothing", 0.25f);
        }
        else if (holdType == WeaponHoldType.SecondaryWeapon)
        {
            secondaryRigAnimator.Play(secondaryWeapon.rigAnimationName + "_Equip");
            animator.CrossFade("HoldNothing", 0.25f);
        }
    }




    //拿起对应武器，并返回需要的转换时间,若已经拿起，返回时间0并不播放过渡动画
    bool HoldWeapon(WeaponHoldType holdType, out float transitTime)
    {
        transitTime = 0f;
        if (holdType == WeaponHoldType.MainWeapon)
        {
            if (currentHold != WeaponHoldType.MainWeapon)
            {
                if (mainWeapon != null)
                {
                    mainRigAnimator.Play(mainWeapon.rigAnimationName + "_EquipToHold");
                    transitTime = AnimationManager.GetInstance().GetClipLength(mainRigAnimator.runtimeAnimatorController, mainWeapon.rigAnimationName + "_EquipToHold");
                    animator.CrossFade(mainWeapon.holdAnimationName, 0.25f);
                    currentHold = WeaponHoldType.MainWeapon;
                }
                else
                {
                    return false;
                }
                if (secondaryWeapon != null)
                {
                    secondaryRigAnimator.Play(secondaryWeapon.rigAnimationName + "_Equip");
                }


            }
        }
        else if (holdType == WeaponHoldType.SecondaryWeapon)
        {
            if (currentHold != WeaponHoldType.SecondaryWeapon)
            {
                if (secondaryWeapon != null)
                {
                    secondaryRigAnimator.Play(secondaryWeapon.rigAnimationName + "_EquipToHold");
                    transitTime = AnimationManager.GetInstance().GetClipLength(secondaryRigAnimator.runtimeAnimatorController, secondaryWeapon.rigAnimationName + "_EquipToHold");
                    animator.CrossFade(secondaryWeapon.holdAnimationName, 0.25f);
                    currentHold = WeaponHoldType.SecondaryWeapon;
                }
                else
                {
                    return false;
                }
                if (mainWeapon != null)
                {
                    mainRigAnimator.Play(mainWeapon.rigAnimationName + "_Equip");
                }


            }
        }
        leftHoldTime = keepHoldTime;
        return true;
    }

    void RetractWeapon(WeaponHoldType holdType)
    {
        if (holdType == WeaponHoldType.MainWeapon)
        {
            mainRigAnimator.Play(mainWeapon.rigAnimationName + "_HoldToEquip");
            animator.CrossFade("HoldNothing", 0.25f);
            currentHold = WeaponHoldType.HoldNothing;
        }
        else if (holdType == WeaponHoldType.SecondaryWeapon)
        {
            secondaryRigAnimator.Play(secondaryWeapon.rigAnimationName + "_HoldToEquip");
            animator.CrossFade("HoldNothing", 0.25f);
            currentHold = WeaponHoldType.HoldNothing;
        }
    }

    public bool Aim(WeaponHoldType holdType)
    {
        //if (npcController.isInteracting()) return;
        //if (holdType == WeaponHoldType.SecondaryWeapon)
        //{

        if (currentHold == WeaponHoldType.SecondaryWeapon && aimFinished && isAiming)
        {
            return true;
        }
        float t = 0;
        if (currentHold != WeaponHoldType.SecondaryWeapon)
        {
            HoldWeapon(holdType, out t);
        }

        isAiming = true;
        locomotionController.DoAim(true);
        aimFinished = false;
        StartCoroutine(ReallyAim(holdType, t));

        return aimFinished;

        ////松开后还原
        //InputMgr.GetInstance().AddListener(InputEvent.MouseRightUp, () =>
        //{
        //    secondaryRigAnimator.CrossFade(secondaryWeapon.rigAnimationName + "_Hold", 0.25f);
        //    InputMgr.GetInstance().AddListener(InputEvent.MouseLeftDown, () => Attack(WeaponHoldType.MainWeapon), true);
        //    isAiming = false;
        //    locomotionController.DoAim(false);
        //    InputMgr.GetInstance().AddListener(InputEvent.MouseRightUp, () => { }, true);
        //}, true);



        //}
    }



    bool aimFinished = false;
    IEnumerator ReallyAim(WeaponHoldType holdType, float t)
    {
        InputMgr.GetInstance().AddListener(InputEvent.MouseLeftDown, () => { }, true);

        t -= advanceTime;
        while (true)
        {
            t -= Time.deltaTime;
            if (t < 0) break;
            yield return null;
        }
        aimFinished = true;

        if (isAiming)
        {
            //左键改为射击
            //InputMgr.GetInstance().AddListener(InputEvent.MouseLeftDown, () => Shoot(holdType), true);

            secondaryRigAnimator.CrossFade(secondaryWeapon.rigAnimationName + "_Aim", 0.25f);

            //if (holdType == WeaponHoldType.SecondaryWeapon)
            // {
            //      secondaryWeapon.aim?.Invoke();
            // }

            while (isAiming)
            {
                leftHoldTime = keepHoldTime;
                //Ray ray = Camera.main.ScreenPointToRay(InputMgr.GetInstance().MousePos);
                //if (Physics.Raycast(ray, out AimPointHitInfo))
                //{
                //    aimPoint.position = SpringSystem.Spring.Damper(aimPoint.position, AimPointHitInfo.point, aimFollowHalfTime, Time.deltaTime);
                //}
                yield return null;
            }
        }
    }

    public void CancelAim()
    {
        secondaryRigAnimator.CrossFade(secondaryWeapon.rigAnimationName + "_Hold", 0.25f);
        //InputMgr.GetInstance().AddListener(InputEvent.MouseLeftDown, () => Attack(WeaponHoldType.MainWeapon), true);
        isAiming = false;
        locomotionController.DoAim(false);
        //InputMgr.GetInstance().AddListener(InputEvent.MouseRightUp, () => { }, true);
    }



    //主武器拿出，返回是否已经完成，可重复调用
    public bool Attack(WeaponHoldType holdType)
    {
        //if (npcController.isInteracting()) return;

        //if (isAttacking) return false;
        //isAttacking = true;

        leftHoldTime = keepHoldTime;

        if (currentHold == WeaponHoldType.MainWeapon && attackFinished)
        {
            return true;
        }

        if (currentHold != WeaponHoldType.MainWeapon)
        {
            if (HoldWeapon(holdType, out float t))
            {
                attackFinished = false;
                StartCoroutine(ReallyAttack(holdType, t));
            }
            else
            {
                return false;
            }
        }

        return attackFinished;
    }


    bool attackFinished = false;
    IEnumerator ReallyAttack(WeaponHoldType holdType, float t)
    {

        t -= advanceTime;    //拔剑加速
        while (true)
        {
            t -= Time.deltaTime;
            if (t < 0) break;
            yield return null;
        }
        attackFinished = true;

        //if (holdType == WeaponHoldType.MainWeapon)
        //{
        //    mainWeapon.attack?.Invoke();
        //}
        //else if (holdType == WeaponHoldType.SecondaryWeapon)
        //{
        //    secondaryWeapon.attack?.Invoke();
        //}


        //isAttacking = false;
    }


    public void SetAimTarget(Transform target)
    {
        //var source = aimConstraint.data.sourceObjects;
        ////source.SetTransform(0, InputMgr.GetInstance().GetAimPoint());
        //source.Clear();
        ////source.SetTransform(0, target);
        //source.Add(new WeightedTransform(target, 1));
        //aimConstraint.data.sourceObjects = source;
        ////aimConstraint.Reset();
        //aimConstraint.enabled = false;
        //aimConstraint.enabled = true;
        //rigBuilder.Build();
        aimTarget = target;
        locomotionController.SetAimPoint(target);
    }


    public enum WeaponEffectType
    {
        Attack,
        Aim,
        Shoot,
    }
    public void CustomWeaponEffect(WeaponHoldType holdType, WeaponEffectType effectType)
    {
        if (holdType == WeaponHoldType.MainWeapon)
        {
            if (effectType == WeaponEffectType.Attack)
            {
                mainWeapon.attack?.Invoke();
            }
            else if (effectType == WeaponEffectType.Aim)
            {
                mainWeapon.aim?.Invoke();
            }
            else if (effectType == WeaponEffectType.Shoot)
            {
                mainWeapon.shoot?.Invoke();
            }
        }
        else if (holdType == WeaponHoldType.SecondaryWeapon)
        {
            if (effectType == WeaponEffectType.Attack)
            {
                secondaryWeapon.attack?.Invoke();
            }
            else if (effectType == WeaponEffectType.Aim)
            {
                secondaryWeapon.aim?.Invoke();
            }
            else if (effectType == WeaponEffectType.Shoot)
            {
                secondaryWeapon.shoot?.Invoke();
            }
        }
    }




    //void Shoot(WeaponHoldType holdType)
    //{
    //    if (npcController.isInteracting()) return;
    //    if (holdType == WeaponHoldType.SecondaryWeapon)
    //    {
    //        secondaryWeapon.shoot?.Invoke();
    //    }
    //}
}
