using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IdleState : FSMState
{
    LocomotionController locomotionController;
    WeaponHoldController weaponHoldController;


    public override bool CheckIsFinished(FSM fsm)
    {
        return true;
    }

    public override void Init(FSM fsm)
    {
        locomotionController = fsm.character.GetComponent<LocomotionController>();
        if (locomotionController == null)
        {
            Debug.LogError("该角色没有locomotionController");
        }

        weaponHoldController = fsm.character.GetComponent<WeaponHoldController>();
    }

    public override void InitType()
    {
        type = FSMStateType.Idle;
    }


    public override void EnterState(FSM fsm)
    {
        base.EnterState(fsm);
        //locomotionController.StartListenInput();
        //weaponHoldController.StartListenInput();
    }

    public override void ExitState(FSM fsm)
    {
        base.ExitState(fsm);
        //locomotionController.StopListemInput();
        //weaponHoldController.StopListenInput();
    }
}
