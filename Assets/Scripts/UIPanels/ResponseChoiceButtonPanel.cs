using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResponseChoiceButtonPanel : BasePanel
{
    [SerializeField] TextMeshProUGUI contentTMP;
    protected override void Awake()
    {
        base.Awake();
        GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
    }
    public void ShowResponseChoice(ResponseChoice choice)
    {
        contentTMP.text = choice.content;
        GetControl<Button>("ResponseChoiceButton").onClick.AddListener(() => DialogueManager.GetInstance().NextTalk(choice.jumpIDs));
    }
}
