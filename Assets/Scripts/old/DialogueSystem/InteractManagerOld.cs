using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractManagerOld : Singleton<InteractManagerOld>
{
    [SerializeField] public List<InteractButton> interactButtons = new();

    UnityEvent updateUI = new();

    private new void Awake()
    {
        base.Awake();
    }

    public void AddButton(InteractButton button)
    {
        interactButtons.Add(button);
        updateUI.Invoke();
    }

    public void RemoveButton(InteractButton button)
    {
        interactButtons.Remove(button);
        updateUI.Invoke();
    }

    public void SetUICallback(UnityAction callback)
    {
        updateUI.RemoveAllListeners();
        updateUI.AddListener(callback);
    }

    public void RemoveUICallback()
    {
        updateUI.RemoveAllListeners();
    }
}
