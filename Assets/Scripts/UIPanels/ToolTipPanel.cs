using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipPanel : BasePanel
{
    public string toolTip;


    public override void ShowMe()
    {
        base.ShowMe();
        GetControl<Button>("Esc").onClick.AddListener(() => UIManager.GetInstance().HidePanel("ToolTipPanel"));
    }

    public void SetToolTip(string text)
    {
        toolTip = text;
    }

    private void Update()
    {
        GetControl<TextMeshProUGUI>("ToolTip").text = toolTip;
    }
}
