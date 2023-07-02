using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Events;


public class GameManager : SingletonAutoMono<GameManager>
{
    [SerializeField] string dataFilePath;
    [SerializeField] string firstEnterDatafilePath = "Data/ArchiveData";

    [Header("战斗相关")]
    [SerializeField] float hitForce = 1.0f;

    private void Awake()
    {
        dataFilePath = Application.persistentDataPath + "/ArchiveData.txt"; ;
    }

    private void Start()
    {
        EnterMainPanel();

        //监听游戏事件
        EventCenter.GetInstance().AddEventListener<(GameObject, GameObject, float, float)>("伤害判定", TakeDamage);
        EventCenter.GetInstance().AddEventListener<GameObject>("角色死亡", CharacterDie);
    }

    //进入游戏主面板
    public void EnterMainPanel()
    {
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel");
    }

    //进入新游戏
    public void NewGame()
    {
        ArchiveData data = ReadFirstEnterData();
        if (data != null)
        {
            DataManager.GetInstance().SetData(data);
            ScenesMgr.GetInstance().LoadSceneAsyn(data.sceneName, () => EnterGameInit()); //可能同时点击加载和新游戏会有问题
        }
    }

    //加载游戏
    public void LoadGame()
    {
        ArchiveData data = ReadSaveData();
        if (data != null)
        {
            DataManager.GetInstance().SetData(data);
            InitBeforeLoadScene();
            ScenesMgr.GetInstance().LoadSceneAsyn(data.sceneName, () => EnterGameInit()); //可能同时点击加载和新游戏会有问题
        }
        else
        {
            Debug.Log("没有存档");
        }
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="fun"></param>
    public void LoadScene(string sceneName, UnityAction fun)
    {
        DataManager.GetInstance().GetData().sceneName = sceneName;
        InitBeforeLoadScene();
        ScenesMgr.GetInstance().LoadSceneAsyn(sceneName, () =>
        {
            EnterSceneInit();
            fun?.Invoke();
        }); //可能同时点击加载和新游戏会有问题
    }

    void EnterSceneInit()
    {
        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<CharacterStatusManagerPanel>("CharacterStatusManagerPanel", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<SystemPanel>("SystemPanel");
        InputMgr.GetInstance().InitWhenLoadScene();
        CameraManager.GetInstance().InitWhenLoadScene();
        DataManager.GetInstance().InitWhenLoadScene();
    }

    //从本地文件夹读取存档数据
    ArchiveData ReadSaveData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            ArchiveData data = JsonConvert.DeserializeObject<ArchiveData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return data;
        }
        return null;
    }

    //从打包文件夹读取初始存档
    ArchiveData ReadFirstEnterData()
    {
        ArchiveData data = new ArchiveData();
        TextAsset json = ResMgr.GetInstance().Load<TextAsset>(firstEnterDatafilePath);
        if (json == null)
        {
            Debug.Log("没有初始存档");
            data = DataManager.GetInstance().GenerateData();
        }
        else
        {
            data = JsonConvert.DeserializeObject<ArchiveData>(json.text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

        }

        return data;
    }



    //保存游戏
    public void SaveGame()
    {
        ArchiveData data = DataManager.GetInstance().GetData();
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });
        File.WriteAllText(dataFilePath, json);
        Debug.Log("存档");
    }

    //进入游戏前根据存档信息初始化
    void InitBeforeLoadScene()
    {
        InputMgr.GetInstance().InitBeforeLoadScene();
        DataManager.GetInstance().InitBeforeloadScene();
        //Time.timeScale = 0;
    }


    //进入游戏根据存档信息初始化
    void EnterGameInit()
    {
        //Time.timeScale = 1;
        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<CharacterStatusManagerPanel>("CharacterStatusManagerPanel", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<SystemPanel>("SystemPanel");

        InputMgr.GetInstance().InitWhenLoadScene();
        CameraManager.GetInstance().InitWhenLoadScene();
        DataManager.GetInstance().InitWhenLoadScene();
        TaskManager.GetInstance().InitWhenLoadGame();
    }



    #region 战斗部分



    //伤害判定
    void TakeDamage((GameObject, GameObject, float, float) hitInfo)
    {
        //Debug.Log("hit");
        if (hitInfo.Item2 == null) return;

        //获取角色信息
        NPCController defender = hitInfo.Item2.GetComponent<NPCController>();


        //如果受击的是角色
        if (defender != null)
        {
            //同一阵营
            if (hitInfo.Item1.GetComponent<NPCController>() != null && hitInfo.Item1.GetComponent<NPCController>().info.tag == defender.info.tag)
            {
                return;
            }

            if (defender.info.health <= 0) return;
            defender.info.health -= hitInfo.Item3;

            if (defender.info.health <= 0) //死亡
            {
                EventCenter.GetInstance().EventTrigger<GameObject>("角色死亡", defender.gameObject);
            }
            else //受击削韧
            {
                //韧性条减少
                if (defender.info.toughness > 0)
                {
                    defender.info.toughness -= hitInfo.Item4;
                    //设置韧性条恢复
                    if (defender.info.toughness <= 0)
                    {
                        var delayAction = new DelayGameAction();
                        delayAction.SetCallback(() =>
                        {
                            defender.info.toughness = defender.info.maxToughness;
                        });
                        delayAction.delayTime = defender.info.toughnessRecoveryTime;
                        delayAction.StartExecute();
                    }
                }

                //如果韧性条小于零，受击打断
                if (defender.info.toughness <= 0)
                {
                    //打断当前动作
                    defender.InterruptSkiil();
                    //调整面向
                    defender.GetComponent<LocomotionController>()?.DoFaceInstantly(hitInfo.Item1.transform.position);
                    //播放受击动画
                    defender.GetComponent<Animator>().Play("Hit");
                    //速度归零并施加推力
                    defender.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Vector3 force = (defender.transform.position - hitInfo.Item1.transform.position).normalized * hitForce;
                    defender.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                    //设置硬直
                    //StartCoroutine(SetStagger(defender, 0.5f));
                    defender.SetStagger(0.5f);
                }
            }
        }
    }



    //根据碰撞体获取角色实例
    public GameObject GetNPCController(Collider collider)
    {
        Transform root = collider.transform;
        NPCController defender = null;
        while (defender == null && root != null)
        {
            defender = root.GetComponent<NPCController>();
            root = root.parent;
        }
        if (defender != null) return defender.gameObject;
        return null;
    }

    //角色死亡
    void CharacterDie(GameObject character)
    {
        character.tag = "Untagged";
        character.GetComponent<NPCController>().info.sceneName = "Death";
        character.GetComponent<NPCController>().updateLocomotion = false;
        character.GetComponent<Animator>().CrossFade("Death", 0.25f);
        character.GetComponent<BehaviorTreeController>().entry.StopExecute();
        character.GetComponent<LocomotionController>().enabled = false;
        character.GetComponent<NavMeshAgentController>().enabled = false;
        character.GetComponent<WeaponHoldController>().enabled = false;
        character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        character.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //var colliders = character.GetComponentsInChildren<Collider>();
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    //if (colliders[i] is BoxCollider)
        //    //{
        //    //    BoxCollider collider = colliders[i] as BoxCollider;
        //    //    Physics.CheckBox(collider.center)
        //    //}
        //    colliders[i].isTrigger = false;
        //}

        character.GetComponent<CapsuleCollider>().enabled = false;

        //StartCoroutine(Delay(0, () =>
        //{
        //    //刚体速度设置为0
        //    Rigidbody[] rigidbodys = character.GetComponentsInChildren<Rigidbody>();
        //    for (int i = 0; i < rigidbodys.Length; i++)
        //    {
        //        rigidbodys[i].velocity = Vector3.zero;
        //        rigidbodys[i].angularVelocity = Vector3.zero;
        //    }


        //    character.GetComponent<Animator>().enabled = false;
        //    character.GetComponent<WeaponHoldController>()?.ClearWeapon();
        //}));


    }
    //后续武器渐变消失
    IEnumerator Delay(float delay, UnityAction fun)
    {
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        fun.Invoke();
    }

    #endregion



    //、、连续受击？
    //IEnumerator SetStagger(NPCController defender, float t)
    //{
    //    defender.isInStagger = true;
    //    yield return new WaitForSeconds(t);
    //    defender.isInStagger = false;
    //}
}
