using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


[Serializable]
public enum CharacterTag
{
    NPC,
    Player,
    Enemy,
    Neutral,
    Ally,
}

[Serializable]
public class NPCInfo
{
    [Header("手动设置属性")]
    public int id;
    public string name;
    public CharacterTag tag;
    public string prefabName;
    public List<int> dialogueEntrys;

    [Header("生命值属性")]
    public float maxHealth = 100;
    public float health = 100;
    public float maxToughness = 10; //最大韧性
    public float toughness = 10; //韧性值
    public float toughnessRegenSpeed = 5; //韧性恢复速度
    public float toughnessRecoveryTime = 5; //击破后的脆弱时间

    [Header("武器相关")]
    public int mainWeaponID;
    public int secondaryWeaponID;

    [Header("自动更新属性")]
    public string sceneName;
    public Vector3 position;
    public Quaternion rotation;

    [Header("背包")]
    public int money;
    public CharacterBag bag;



    public void SetTag(CharacterTag tag, GameObject character)
    {
        this.tag = tag;
        character.tag = tag.ToString();
    }

    public void InitTag(GameObject character)
    {
        character.tag = tag.ToString();
    }
}

public class NPCController : MonoBehaviour
{
    [SerializeField] public NPCInfo info;
    [SerializeField] public bool isPersistent = true;

    [Header("运行时属性")]
    public bool isInStagger = false; //硬直
    public float staggerRecoverTime = 0; //硬直恢复时间


    public bool updateLocomotion = true;  //用于传送门，不自动保存位置



    //注册自己，如果当前npc在另一个场景，则删除
    private void Awake()
    {
        UpdateInfo();
        if (isPersistent)
        {
            if (DataManager.GetInstance().NPCLogin(ref info, gameObject))
            {
                UpdateGameObject();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        EventCenter.GetInstance().EventTrigger<GameObject>("角色生成", gameObject);
    }

    private void OnDestroy()
    {
        UpdateInfo();
        if (isPersistent)
        {
            DataManager.GetInstance()?.NPCs.Remove(info.id);
        }
    }

    private void Update()
    {
        UpdateInfo();
        //硬直恢复
        if (Time.time >= staggerRecoverTime)
        {
            isInStagger = false;
        }
        else
        {
            // Debug.Log("time:" + Time.time);
        }
    }













    //根据gameobject更新info
    void UpdateInfo()
    {
        if (updateLocomotion)
        {
            //场景和位置
            info.sceneName = SceneManager.GetActiveScene().name;
            info.position = transform.position;
            info.rotation = transform.rotation;
        }


        //韧性自动恢复
        if (info.toughness > 0)
        {
            info.toughness += info.toughnessRegenSpeed * Time.deltaTime;
            info.toughness = Mathf.Min(info.toughness, info.maxToughness);
        }

    }

    //根据info更新gameobject
    void UpdateGameObject()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(info.position);
        }
        else
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.MovePosition(info.position);
                rigidbody.MoveRotation(info.rotation.normalized);
            }
            else
            {
                transform.position = info.position;
                transform.rotation = info.rotation;
            }
        }


        info.InitTag(gameObject);
    }




    #region npc行为

    public void SetStagger(float lastingTime, bool force = false)
    {
        float recoverTime = Time.time + lastingTime;
        if (force)
        {
            staggerRecoverTime = recoverTime;
        }
        else
        {
            staggerRecoverTime = MathF.Max(staggerRecoverTime, recoverTime);
        }

        isInStagger = true;


    }
    public void DoTalk()
    {
        //Debug.Log("对话");
        DialogueManager.GetInstance().StartTalk(info.dialogueEntrys);
    }

    //打断行为树中的可打断行为
    public void InterruptSkiil()
    {
        BT.CharacterEntry tree = (BT.CharacterEntry)GetComponent<BehaviorTreeController>().entry;
        tree.InterruptSkill();

    }

    public void Follow(Vector3 destination)
    {
        NavMeshAgentController navMeshAgentController = GetComponent<NavMeshAgentController>();
        navMeshAgentController.StartNavagation();
        navMeshAgentController.SetDestination(destination);
    }

    public void MoveTo(Transform destination)
    {
        Vector3 d = destination.position;
        Follow(d);
    }

    public void Disappear()
    {
        info.sceneName = "Death";
        updateLocomotion = false;
        GetComponent<BehaviorTreeController>().entry.StopExecute();
        gameObject.SetActive(false);
    }

    public void StopFollow()
    {
        NavMeshAgentController navMeshAgentController = GetComponent<NavMeshAgentController>();
        navMeshAgentController.StopNavagation();
    }

    //拿起武器，返回是否已经动画结束
    public bool HoldMainWeapon()
    {
        return GetComponent<WeaponHoldController>().Attack(WeaponHoldType.MainWeapon);
    }

    public void SetAimTarget(Transform target)
    {
        var controller = GetComponent<WeaponHoldController>();
        controller.SetAimTarget(target);
    }

    //武器瞄准，返回拿出副武器的动画是否已经结束
    public bool Aim()
    {
        var controller = GetComponent<WeaponHoldController>();
        return controller.Aim(WeaponHoldType.SecondaryWeapon);
    }

    public void CancelAim()
    {
        var controller = GetComponent<WeaponHoldController>();
        controller.CancelAim();
    }

    ////进入硬直
    //public void EnterStagger()
    //{
    //    isInStagger = true;
    //}

    //public void LeaveStagger()
    //{
    //    isInStagger = false;
    //}


    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="weapon"></param>
    public void EquipWeapon(Weapon weapon)
    {
        WeaponHoldController controller = GetComponent<WeaponHoldController>();
        if (weapon.type == Weapon.WeaponType.MainWeapon)
        {
            info.mainWeaponID = weapon.id;
        }
        else if (weapon.type == Weapon.WeaponType.SecondaryWeapon)
        {
            info.secondaryWeaponID = weapon.id;
        }

        controller.LoadWeapon();
        var bt = GetComponent<BehaviorTreeController>();
        bt.entry.StopExecute();
        bt.entry.Init(null);   //重新构造行为树
        bt.entry.StartExecute();
    }

    /// <summary>
    /// 获得道具
    /// </summary>
    /// <param name="id"></param>
    public void ObtainItem(int id)
    {
        ItemInfo itemInfo = new();
        itemInfo.id = id;
        Item item = itemInfo.GetItem();

        if (item is Weapon)
        {
            info.bag.weapons.Add(itemInfo);
        }
        else
        {
            info.bag.usableItems.Add(itemInfo);
        }
    }

    /// <summary>
    /// 购买道具
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool BuyItem(int id)
    {
        ItemInfo itemInfo = new();
        itemInfo.id = id;
        Item item = itemInfo.GetItem();
        if (item.purchasePrice > info.money)
        {
            return false;
        }

        info.money -= item.purchasePrice;
        ObtainItem(id);
        return true;
    }



    #endregion



    #region abandoned
    ////当前正在执行的技能
    //public SkillGameAction currentSkill;

    //public bool isInteracting()
    //{
    //    if (currentSkill == null) return false;
    //    return !currentSkill.IsCompleted();
    //}

    ////释放技能， 并注册当前技能动作
    //public void CastSkill(SkillGameAction skill)
    //{
    //    if (currentSkill != null && !currentSkill.IsCompleted())
    //    {
    //        Debug.LogError("上一个技能未完成");
    //    }

    //    currentSkill = skill;
    //    skill.SetReleaser(this.gameObject);
    //    skill.StartExecute();
    //}

    ////打断当前正在执行的技能
    //public void Interrupt()
    //{
    //    if (currentSkill != null)
    //    {
    //        currentSkill.ForceInterrupt();
    //    }
    //}
    #endregion
}
