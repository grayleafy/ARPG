using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

[Serializable]
public class Dialogue
{
    [SerializeField] public int id;
    [SerializeField] public string speakerName;
    [SerializeField] public string content;
    [SerializeField] public int defautJump;
    [SerializeField] public DialogueOption[] dialogueOptions;
    [SerializeField] public string[] gameActionDefs;

    public static Dialogue Create(string dialogueDef)
    {
        Dialogue dialogue = new Dialogue();
        string[] parts = dialogueDef.Split(",");

        dialogue.id = int.Parse(parts[0]);
        dialogue.speakerName = parts[1];
        dialogue.content = parts[2];
        dialogue.defautJump = int.Parse(parts[3]);
        {
            string pattern = @"\{([^}]*)\}";
            MatchCollection matches = Regex.Matches(parts[4], pattern);
            string[] dialogueOptionDefs = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                dialogueOptionDefs[i] = matches[i].Groups[1].Value;
            }
            dialogue.dialogueOptions = new DialogueOption[dialogueOptionDefs.Length];
            for (int i = 0; i < dialogueOptionDefs.Length; i++)
            {
                dialogue.dialogueOptions[i] = DialogueOption.Create(dialogueOptionDefs[i]);
            }
        }
        {
            string pattern = @"\{([^}]*)\}";
            MatchCollection matches = Regex.Matches(parts[5], pattern);
            dialogue.gameActionDefs = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                dialogue.gameActionDefs[i] = matches[i].Groups[1].Value;
            }
        }

        return dialogue;
    }
}
