using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public enum GameEventTag
{
    Kill,
    Dialog,
}

public class GameEvent
{
    public GameEventTag tag;
    public object[] args;
}

public class GameEventManager : Singleton<GameEventManager>
{
    [SerializeField] int messagesProcessedPerFrame = 10;

    Dictionary<GameEventTag, UnityEvent<object[]>> observer = new();
    Queue<GameEvent> gameEventQueue = new Queue<GameEvent>();

    public void SendGameEvent(GameEventTag tag, object[] args)
    {
        GameEvent gameEvent = new GameEvent();
        gameEvent.tag = tag;
        gameEvent.args = args;
        gameEventQueue.Enqueue(gameEvent);
    }

    public void AddObserver(GameEventTag tag, UnityAction<object[]> action)
    {
        if (!observer.ContainsKey(tag))
        {
            observer[tag] = new UnityEvent<object[]>();
        }
        observer[tag].AddListener(action);
    }

    public void RemoveObserver(GameEventTag tag, UnityAction<object[]> action)
    {
        if (!observer.ContainsKey(tag))
        {
            return;
        }
        observer[tag].RemoveListener(action);
    }

    private void Update()
    {
        for (int t = 0; t < messagesProcessedPerFrame; t++)
        {
            if (gameEventQueue.Count > 0)
            {
                GameEvent gameEvent = gameEventQueue.Dequeue();
                if (observer.ContainsKey(gameEvent.tag))
                {
                    observer[gameEvent.tag].Invoke(gameEvent.args);
                }
            }
            else
            {
                break;
            }
        }

    }


}
