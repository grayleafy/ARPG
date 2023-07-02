using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private void Start()
    {
        GetControl<Button>("EnterGame").onClick.AddListener(() => GameManager.GetInstance().NewGame());
        GetControl<Button>("LoadGame").onClick.AddListener(() => GameManager.GetInstance().LoadGame());
    }
}
