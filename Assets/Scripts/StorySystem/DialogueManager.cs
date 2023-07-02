using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : SingletonAutoMono<DialogueManager>
{
    List<DialogueSegment> DialogueSegments { get { return DataManager.GetInstance().GetData().dialogueSegments; } }
    Dictionary<int, DialogueSegment> dialogueSegmentsMap = new();
    public DialogueSegment currentDialogueSegment;


    public void StartTalk(List<int> entryIDs)
    {
        currentDialogueSegment = GetDialogueSegment(entryIDs);

        if (currentDialogueSegment != null)
        {
            currentDialogueSegment.Start();
            UIManager.GetInstance().ShowPanel<DialoguePanel>("DialoguePanel", E_UI_Layer.Mid, (panel) => panel.ShowDialogueSegment(currentDialogueSegment));
        }
        else
        {
            UIManager.GetInstance().HidePanel("DialoguePanel");
        }
    }

    public void NextTalk(List<int> entryIDs)
    {
        currentDialogueSegment.End();

        //结束对话
        if (entryIDs == null || entryIDs.Count == 0)
        {
            UIManager.GetInstance().HidePanel("DialoguePanel");
        }
        //下一段对话
        else
        {
            StartTalk(entryIDs);
        }
    }


    //从对话入口列表中获取满足条件的第一个对话片段
    DialogueSegment GetDialogueSegment(List<int> entryIDs)
    {
        for (int i = 0; i < entryIDs.Count; i++)
        {
            DialogueSegment segment = GetDialogueSegment(entryIDs[i]);
            if (segment.entryCondition == null || segment.entryCondition.EvaluateCondition(DataManager.GetInstance().controlPlayer))
            {
                return segment;
            }
        }
        return null;
    }

    //根据对话id获取对话片段
    DialogueSegment GetDialogueSegment(int id)
    {
        if (!dialogueSegmentsMap.ContainsKey(id)) { UpdateMap(); }
        return dialogueSegmentsMap[id];
    }

    void UpdateMap()
    {
        for (int i = 0; i < DialogueSegments.Count; i++)
        {
            dialogueSegmentsMap[DialogueSegments[i].id] = DialogueSegments[i];
        }
    }
}
