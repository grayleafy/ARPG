using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneObjectInfo
{
    public string name;
    public int id;
    public string animationName;
    public bool interactable = true;
    public bool isActive = true;
}


/// <summary>
/// 控制场景物品的持久化存贮
/// </summary>
public class SceneObjectController : MonoBehaviour
{
    [SerializeField] public SceneObjectInfo info;
    [SerializeField] public bool isPersistent = true;

    Animator animator = null;

    //根据存档设置初始状态
    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (isPersistent)
        {
            var data = DataManager.GetInstance().GetData();
            var tempInfo = data.SceneObjects.Find((x) => x.id == info.id);
            if (tempInfo != null)
            {
                info = tempInfo;
            }
            else
            {
                data.SceneObjects.Add(info);
            }
        }

        UpdateGameObject();
    }

    void UpdateGameObject()
    {
        if (animator != null)
        {
            animator?.Play(info.animationName);
        }


        if (info.interactable == false)
        {
            var interactController = GetComponent<InteractController>();
            if (interactController != null)
            {
                interactController.enabled = false;
                interactController.active = false;
            }
        }

        if (info.isActive == false)
        {
            Destroy(gameObject);
        }
    }


    public void CrossFade(string targetAnimation)
    {
        animator.CrossFade(targetAnimation, 0.25f);
        info.animationName = targetAnimation;
    }

    public void SetInteractivity(bool interactable)
    {
        info.interactable = interactable;
        var interactController = GetComponent<InteractController>();
        if (interactController != null)
        {
            interactController.enabled = interactable;
            //Destroy(interactController);
        }
    }

    public void SetActive(bool active)
    {
        info.isActive = active;

        if (active == false)
        {
            Destroy(gameObject);
        }
    }
}
