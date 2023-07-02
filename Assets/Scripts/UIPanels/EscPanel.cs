using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : BasePanel
{
    private void Start()
    {
        GetControl<Button>("SaveButton").onClick.AddListener(() => GameManager.GetInstance().SaveGame());
        GetControl<Button>("LoadButton").onClick.AddListener(() => GameManager.GetInstance().LoadGame());
        GetControl<Button>("ExitButton").onClick.AddListener(() => UIManager.GetInstance().HidePanel("EscPanel"));
    }

    public override void ShowMe()
    {
        base.ShowMe();
        InputMgr.GetInstance().AddListener(InputEvent.EscDown, () => UIManager.GetInstance().HidePanel("EscPanel"), true);
    }

    public override void HideMe()
    {
        base.HideMe();
        InputMgr.GetInstance().AddListener(InputEvent.EscDown, () => UIManager.GetInstance().ShowPanel<EscPanel>("EscPanel"), true);
    }
}
