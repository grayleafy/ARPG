using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PlayTimeLine : GameAction
{
    public string timeLineName;

    PlayableDirector playableDirector = null;
    float duration = 0;

    protected override bool CheckActionComplete()
    {
        if (playableDirector == null)
        {
            return true;
        }

        return duration <= 0;
    }

    protected override void ExecutePerFrame()
    {
        duration -= Time.deltaTime;
    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        GameObject timeLineGameObject = GameObject.Find(timeLineName);
        if (timeLineGameObject != null)
        {
            playableDirector = timeLineGameObject.GetComponent<PlayableDirector>();
            playableDirector.Play();

            duration = (float)playableDirector.duration;
        }


    }
}
