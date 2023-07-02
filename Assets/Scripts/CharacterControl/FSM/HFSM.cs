using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFSM : MonoBehaviour
{
    [Header("设置")]
    [SerializeField] string defaultFSMName;
    [SerializeField] List<FSM> stateMachines = new();

    //[Header("运行时数据")]
    FSM currentFSM;


    [Header("要修改的状态机的索引")]
    [SerializeField] public int indexToModify = 0;

    private void Start()
    {
        for (int i = 0; i < stateMachines.Count; i++)
        {
            stateMachines[i].Init(this);
        }

        currentFSM = stateMachines.Find((x) => defaultFSMName == x.name);
        //if (currentFSM == null)
        //{
        //    currentFSM = stateMachines[0];
        //}
    }

    private void Update()
    {
        currentFSM.Update();
    }

    private void FixedUpdate()
    {
        currentFSM.FixedUpdate();
    }








    public void TransitFSM(string FSMName)
    {
        currentFSM = stateMachines.Find((x) => FSMName == x.name);
        currentFSM.ReSet();
    }
}
