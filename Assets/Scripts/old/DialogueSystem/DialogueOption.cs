using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOption
{
    [SerializeField] public string content;
    [SerializeField] public int jumpID;

    public static DialogueOption Create(string dialogueOptionDef)
    {
        string[] parts = dialogueOptionDef.Split("=>");
        DialogueOption dialogueOption = new DialogueOption();
        dialogueOption.content = parts[0];
        dialogueOption.jumpID = int.Parse(parts[1]);
        return dialogueOption;
    }
}
