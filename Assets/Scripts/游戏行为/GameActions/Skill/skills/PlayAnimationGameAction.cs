using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放动画，需要运行时设置实例
/// </summary>
[Serializable]
public class PlayAnimationGameAction : GameActionWithReleaser
{
    [SerializeField] public string animationName;
    [SerializeField] public float fadeTime = 0.25f;
    [SerializeField] public float advanceEndTime = 0f;
    [SerializeField] public bool applyRootMotion = true;
    [SerializeField] public bool forbidLocomotion = true;

    GameObject character;  //需要运行时设置

    float clipTime;
    Animator animator;
    bool originRootApply;
    bool originLocomotion;

    protected override bool CheckActionComplete()
    {
        if (clipTime <= 0)
        {
            Finish();
            return true;
        }
        return false;
    }

    protected override void ExecutePerFrame()
    {
        clipTime -= Time.deltaTime;
    }

    protected override void Init()
    {
        this.character = releaser;

        if (character == null)
        {
            Debug.LogError("未在运行时设置实例");
        }
        animator = character.GetComponent<Animator>();
        originRootApply = animator.applyRootMotion;
        animator.applyRootMotion = applyRootMotion;
        animator.CrossFade("NoAction", 0.25f);

        MonoMgr.GetInstance().StartCoroutine(Transit());

        //限制移动输入
        var locomotion = character.GetComponent<LocomotionController>();
        if (forbidLocomotion && locomotion != null)
        {

            originLocomotion = locomotion.applyLocomotion;
            locomotion.StopListemInput();
        }
    }

    IEnumerator Transit()
    {
        yield return null;
        animator.CrossFade(animationName, fadeTime, animator.GetLayerIndex("Action"), 0);    //目标动画归一化时间设置为0
        clipTime = AnimationManager.GetInstance().GetClipLength(animator.runtimeAnimatorController, animationName) - advanceEndTime;
    }



    protected override void Finish()
    {
        //恢复
        animator.applyRootMotion = originRootApply;
        var locomotion = character.GetComponent<LocomotionController>();
        if (forbidLocomotion && locomotion != null && originLocomotion == true)
        {
            locomotion.StartListenInput();
        }
    }
}
