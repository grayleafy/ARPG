using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public class DialogueManager : Singleton<DialogueManager>
//{
//    [SerializeField] TextAsset dialogueCsv;
//    [SerializeField] List<Dialogue> dialogues = new();
//    Dictionary<int, int> map = new();
//    [SerializeField] Dialogue currentDialog;

//    [SerializeField] UnityEvent<Dialogue> refreshUI = new();
//    UnityEvent hideUI = new();

//    private new void Awake()
//    {
//        base.Awake();
//        string[] parts = dialogueCsv.text.Split("\n");
//        for (int i = 1; i < parts.Length; i++)
//        {
//            if (parts[i] != "")
//            {
//                var dialogue = Dialogue.Create(parts[i]);
//                dialogues.Add(dialogue);
//                map[dialogue.id] = dialogues.Count - 1;
//            }
//        }
//    }

//    public void SetUIRefresh(UnityAction<Dialogue> action)
//    {
//        refreshUI.AddListener(action);
//    }

//    public void SetHideUI(UnityAction action)
//    {
//        hideUI.AddListener(action);
//    }

//    public void PlayDialogue(int enterID)
//    {
//        currentDialog = dialogues[map[enterID]];
//        refreshUI.Invoke(currentDialog);
//    }

//    public void Jump(int optionIndex)
//    {
//        int jumpID = currentDialog.dialogueOptions[optionIndex].jumpID;
//        PlayDialogue(jumpID);
//    }
//}
