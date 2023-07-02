using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InteractButton
{
    [SerializeField] public string description;
    [SerializeField] public UnityEvent onClick;
}

public class InteractController : MonoBehaviour
{
    [Header("可交互类型")]
    [SerializeField] public List<InteractButton> buttons;

    [Header("交互区域")]
    [SerializeField] float ridius = 2f;
    [SerializeField] Vector3 offset = new Vector3(0, 1, 0);
    SphereCollider interactCollider = null;

    public bool active = true;


    private void Awake()
    {
        interactCollider = gameObject.AddComponent<SphereCollider>();
        interactCollider.radius = ridius;
        interactCollider.center = offset;
        interactCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.gameObject == DataManager.GetInstance().controlPlayer)
            {
                InteractManager.GetInstance().Login(this);
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (active)
        {
            if (other.gameObject == DataManager.GetInstance().controlPlayer)
            {
                InteractManager.GetInstance().Logout(this);
            }
        }

    }



    private void OnDisable()
    {
        InteractManager.GetInstance().Logout(this);
        active = false;
    }






}
