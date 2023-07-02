using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanel : BasePanel
{
    [SerializeField] RectTransform responseChoiceGroup;
    [SerializeField] float wordsPerSecond = 30; //每秒字数
    List<ResponseChoiceButtonPanel> responseButtons = new();

    string showContent;
    bool isTalking = false;
    bool isStartActing = false;

    public void ShowDialogueSegment(DialogueSegment dialogueSegment)
    {
        //清除
        for (int i = responseButtons.Count - 1; i >= 0; i--)
        {
            Destroy(responseButtons[i].gameObject);
            responseButtons.RemoveAt(i);
        }

        GetControl<TextMeshProUGUI>("Speaker").text = dialogueSegment.speaker;
        StartCoroutine(ReallyUpdateContent(dialogueSegment));



    }


    IEnumerator ReallyUpdateContent(DialogueSegment dialogueSegment)
    {
        showContent = "";
        isTalking = true;
        isStartActing = false;
        if (dialogueSegment.enterAction != null) isStartActing = true;
        GetControl<Button>("DialogueBox").onClick.RemoveAllListeners();
        GetControl<Button>("DialogueBox").onClick.AddListener(() => isTalking = false);


        for (int i = 0; i < dialogueSegment.content.Length; i++)
        {
            //更新显示文本
            showContent += dialogueSegment.content[i];
            GetControl<TextMeshProUGUI>("Content").text = showContent;

            //更新动作是否结束
            if (dialogueSegment.enterAction != null)
            {
                isStartActing = !dialogueSegment.enterAction.IsCompleted();
            }


            if (isTalking == false && isStartActing == false)
            {
                break;
            }
            yield return new WaitForSeconds(1 / wordsPerSecond);
        }
        showContent = dialogueSegment.content;
        GetControl<TextMeshProUGUI>("Content").text = showContent;

        //动作还没结束，等待
        while (isStartActing == true)
        {
            if (dialogueSegment.enterAction != null)
            {
                isStartActing = !dialogueSegment.enterAction.IsCompleted();
            }
            yield return null;
        }

        AddResponseButton(dialogueSegment);
    }

    //添加回复选项按钮
    void AddResponseButton(DialogueSegment dialogueSegment)
    {
        //如果没有回复选项，单击进入下一段对话
        if (dialogueSegment.responseChoices.Count == 0)
        {
            GetControl<Button>("DialogueBox").onClick.RemoveAllListeners();
            GetControl<Button>("DialogueBox").onClick.AddListener(() => DialogueManager.GetInstance().NextTalk(dialogueSegment.defaultJumpIDs));
        }

        //不同的回复选项
        for (int i = 0; i < dialogueSegment.responseChoices.Count; i++)
        {
            var temp = dialogueSegment.responseChoices[i];
            var panel = ResMgr.GetInstance().Load<GameObject>("UI/ResponseChoiceButtonPanel", responseChoiceGroup).GetComponent<ResponseChoiceButtonPanel>();
            responseButtons.Add(panel);
            panel.GetComponent<RectTransform>().SetSiblingIndex(responseChoiceGroup.GetComponent<RectTransform>().childCount - 1);
            panel.ShowResponseChoice(temp);
        }

        //刷新自动布局
        responseChoiceGroup.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        responseChoiceGroup.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        LayoutRebuilder.MarkLayoutForRebuild(responseChoiceGroup.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(responseChoiceGroup.GetComponent<RectTransform>());
    }
}
