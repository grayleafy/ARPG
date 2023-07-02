//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class HandleController : MonoBehaviour
//{
//    [SerializeField] Transform handlePos;
//    [SerializeField] Weapon meleeWeapon;
//    [SerializeField] Weapon rangedWeapon;

//    GameObject meleeWeaponEntity;
//    GameObject rangedWeaponEntity;

//    private void Awake()
//    {
//        //LoadWeapon();

//    }

//    //加载模型并默认显示近战武器
//    void LoadWeapon()
//    {
//        if (meleeWeapon != null)
//        {
//            ResMgr.GetInstance().LoadAsync<GameObject>("Weapons/" + meleeWeapon.prefabName, (entity) => { meleeWeaponEntity = entity; SwitchWeapon(true); }, handlePos);
//        }
//        if (rangedWeapon != null)
//        {
//            ResMgr.GetInstance().LoadAsync<GameObject>("Weapons/" + rangedWeapon.prefabName, (entity) => { rangedWeaponEntity = entity; SwitchWeapon(true); }, handlePos);
//        }
//    }

//    //切换武器模型
//    void SwitchWeapon(bool melee)
//    {
//        if (melee)
//        {
//            meleeWeaponEntity?.SetActive(true);
//            rangedWeaponEntity?.SetActive(false);
//        }
//        else
//        {
//            meleeWeaponEntity?.SetActive(false);
//            rangedWeaponEntity?.SetActive(true);
//        }
//    }

//    public void StartInputListen()
//    {
//        InputMgr.GetInstance().AddListener(InputEvent.MouseLeftTap, () =>
//        {
//            if (meleeWeapon.attackAction.IsCompleted())
//            {
//                meleeWeapon.attackAction.StartExecute();
//            }
//        });
//        InputMgr.GetInstance().AddListener(InputEvent.MouseLeftHold, () => meleeWeapon.attackChargeAction.StartExecute());
//        InputMgr.GetInstance().AddListener(InputEvent.MouseRightDown, () => rangedWeapon.AimAction.StartExecute());
//    }

//    public void StopInputListen()
//    {
//        InputMgr.GetInstance().RemoveListener(InputEvent.MouseLeftTap, () => meleeWeapon.attackAction.StartExecute());
//        InputMgr.GetInstance().RemoveListener(InputEvent.MouseLeftHold, () => meleeWeapon.attackChargeAction.StartExecute());
//        InputMgr.GetInstance().RemoveListener(InputEvent.MouseRightDown, () => rangedWeapon.AimAction.StartExecute());
//    }
//}
