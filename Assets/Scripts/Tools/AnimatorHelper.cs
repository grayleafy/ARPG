using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHelper
{
    /// <summary>
    /// 强制动画过渡
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="name"></param>
    /// <param name="transitionDuration"></param>
    /// <param name="layer"></param>
    /// <param name="normalizedTime"></param>
    public static void ForceCrossFade(this Animator animator, string name, float transitionDuration, int layer, float normalizedTime = float.NegativeInfinity)
    {
        animator.Update(0);
        if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
        {
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
        else
        {
            animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
            animator.Update(0);
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
    }

    /// <summary>
    /// 强制动画过渡
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="name"></param>
    /// <param name="transitionDuration"></param>
    public static void ForceCrossFade(this Animator animator, string name, float transitionDuration)
    {
        animator.Update(0);

        //animator.Play(name);
        animator.Update(0);
        animator.CrossFade(name, transitionDuration);
        animator.Update(0);

    }
}
