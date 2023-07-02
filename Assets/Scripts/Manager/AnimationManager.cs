using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : SingletonAutoMono<AnimationManager>
{
    public Dictionary<RuntimeAnimatorController, Dictionary<string, float>> clipMap = new();

    //获取动画剪辑的时长
    public float GetClipLength(RuntimeAnimatorController controller, string clipName)
    {
        if (!clipMap.ContainsKey(controller))
        {
            clipMap[controller] = new Dictionary<string, float>();
            foreach (var state in controller.animationClips)
            {
                clipMap[controller][state.name] = state.length;
            }
        }

        if (clipMap[controller].ContainsKey(clipName))
        {
            return clipMap[controller][clipName];
        }
        else
        {
            Debug.LogError("动画器中没有找到该剪辑");
            return -1;
        }
    }

    public void CrossFade(Animator animator, string clipName, float dt)
    {
        animator.CrossFade(clipName, dt);
    }
}
