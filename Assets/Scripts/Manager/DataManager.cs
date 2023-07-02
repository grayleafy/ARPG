using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class ArchiveData
{
    public string sceneName;
    //public List<SceneInfo> sceneInfos = new();
    [Header("npc角色信息")]
    public List<NPCInfo> NPCInfos = new();
    public int controlPlayerID;
    public List<int> activeTeamMemberIDs = new();
    public List<int> backupTeamMemberIDs = new();

    [Space(100)]
    [Header("场景物品信息")]
    public List<SceneObjectInfo> SceneObjects = new();

    [Space(100)]
    [Header("对话列表")]
    public List<DialogueSegment> dialogueSegments = new();

    [Space(100)]
    [Header("任务列表")]
    public List<Task> tasks = new();

    [Space(100)]
    [Header("武器列表")]
    public List<Weapon> weapons = new();

    [Space(100)]
    [Header("道具列表")]
    public List<UsableItem> usableItems = new();
}

[Serializable]
public class SceneInfo
{
    public string sceneName;
}



public class DataManager : SingletonAutoMono<DataManager>
{
    [SerializeField] ArchiveData data = new();
    [SerializeField] public Dictionary<int, GameObject> NPCs = new();

    [Space(100)]
    [Header("运行时数据")]
    [SerializeField] public GameObject controlPlayer;
    [SerializeField] public List<GameObject> teamMembers;

    //获取运行时数据
    public ArchiveData GetData()
    {
        //data.sceneName = SceneManager.GetActiveScene().name;
        return data;
    }
    //从本地读取数据后设置为运行时数据
    public void SetData(ArchiveData data) { this.data = data; }

    //测试用，当不存在首次存档时，代码生成存档数据
    public ArchiveData GenerateData()
    {


        return data;
    }

    //根据id获取NPC信息
    NPCInfo GetNPCInfo(int id)
    {
        NPCInfo res = data.NPCInfos.Find(x => x.id == id);
        return res;
    }

    public GameObject GetNPC(int id)
    {
        if (NPCs.ContainsKey(id)) return NPCs[id];
        return null;
    }

    //场景中的NPC信息加入到运行时存档数据，建立关联
    void AddNPCInfo(NPCInfo info, string sceneName)
    {
        data.NPCInfos.Add(info);
    }

    /// <summary>
    /// 由npc控制器调用，在数据管理器中登录
    /// </summary>
    /// <returns></returns>
    public bool NPCLogin(ref NPCInfo info, GameObject npc)
    {
        NPCInfo newInfo = DataManager.GetInstance().GetNPCInfo(info.id);
        if (newInfo != null)
        {
            if (newInfo.sceneName != info.sceneName) //如果在另一个场景
            {
                return false;
            }
            else //如果存在则替换
            {
                info = newInfo;
                DataManager.GetInstance().NPCs[info.id] = npc;
                return true;
            }

        }
        else //不存在则新建
        {
            DataManager.GetInstance().AddNPCInfo(info, SceneManager.GetActiveScene().name);
            DataManager.GetInstance().NPCs[info.id] = npc;
            return true;
        }
    }

    /// <summary>
    /// 加载场景前调用
    /// </summary>
    public void InitBeforeloadScene()
    {
        NPCs = new();
        controlPlayer = null;
    }

    /// <summary>
    /// 加载场景时初始化，创建存档中记录的额外的npc
    /// </summary>
    public void InitWhenLoadScene()
    {
        //更新场景名
        data.sceneName = SceneManager.GetActiveScene().name;

        //创建额外npc
        List<int> waitingIDs = new();
        for (int i = 0; i < data.NPCInfos.Count; i++)
        {
            if (data.NPCInfos[i].sceneName == data.sceneName && !NPCs.ContainsKey(data.NPCInfos[i].id))
            {
                waitingIDs.Add(data.NPCInfos[i].id);
            }
        }
        AsyncGroup asyncGroup = new AsyncGroup(InitPlayerAndTeamMember, waitingIDs.Count);
        for (int i = 0; i < waitingIDs.Count; i++)
        {
            InstantiateNPCAsync(waitingIDs[i], (x) => asyncGroup.OnNodeComplete());
        }



    }

    //维护主角列表,并设置相机
    void InitPlayerAndTeamMember()
    {
        controlPlayer = GetNPC(data.controlPlayerID);
        CameraManager.GetInstance().SetTarget(controlPlayer);

        teamMembers = new();
        for (int i = 0; i < data.activeTeamMemberIDs.Count; i++)
        {
            teamMembers.Add(GetNPC(data.activeTeamMemberIDs[i]));
        }
    }

    void InstantiateNPCAsync(int id, UnityAction<GameObject> fun)
    {
        NPCInfo npcInfo = data.NPCInfos.Find(x => x.id == id);
        ResMgr.GetInstance().LoadAsync<GameObject>(npcInfo.prefabName, fun);   //数组全部异步结束
    }
}
