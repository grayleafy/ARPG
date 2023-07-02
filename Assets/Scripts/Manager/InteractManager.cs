using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : SingletonAutoMono<InteractManager>
{
    [SerializeField] List<InteractController> interactableObjects = new();


    private void Update()
    {
        if (DataManager.GetInstance().controlPlayer != null)
        {
            UpdateInteractPanel(GetInteract(DataManager.GetInstance().controlPlayer, new Vector3(0, 1, 0)));
        }

    }


    /// <summary>
    /// 由交互对象调用，表示当前可以交互
    /// </summary>
    /// <param name="interact"></param>
    public void Login(InteractController interact)
    {
        interactableObjects.Add(interact);
    }

    public void Logout(InteractController interact)
    {
        interactableObjects.Remove(interact);
    }

    //根据控角色面朝的方向，获取可交互对象
    InteractController GetInteract(GameObject player, Vector3 offset)
    {
        Vector3 forward = player.transform.forward;
        int index = -1;
        float minDis = Mathf.Infinity;
        for (int i = 0; i < interactableObjects.Count; i++)
        {
            Vector3 dir = interactableObjects[i].transform.position - (player.transform.position + offset);
            //如果在后面则跳过
            if (Vector3.Dot(forward, dir) < 0) { continue; }
            //找到离直线最近的
            Vector3 projectPoint = Vector3.Project(dir, forward);
            float dis = (dir - projectPoint).magnitude;
            if (index == -1 || dis < minDis)
            {
                index = i;
                minDis = dis;
            }
        }

        if (index == -1) return null;
        return interactableObjects[index];
    }

    //更新交互菜单
    void UpdateInteractPanel(InteractController interact)
    {
        if (interact == null)
        {
            UIManager.GetInstance().HidePanel("InteractPanel");
        }
        else
        {
            UIManager.GetInstance().ShowPanel<InteractPanel>("InteractPanel", E_UI_Layer.Bot, (panel) => panel.SetInteract(interact));
        }
    }
}
