using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public class DialoguePanel : BasePanel
//{
//    GameObject nameObject;
//    GameObject contentObject;
//    GameObject optionListObject;

//void Awake()
//{
//    nameObject = FindChild("Name");
//    contentObject = FindChild("Content");
//    optionListObject = FindChild("Options");
//}

//private void Start()
//{
//    DialogueManager.Instance.SetUIRefresh(ShowDialogue);
//    DialogueManager.Instance.SetHideUI(Close);

//    for (int i = 0; i < optionListObject.transform.childCount; i++)
//    {
//        int temp = i;
//        optionListObject.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => DialogueManager.Instance.Jump(temp));
//    }

//    Close();
//}

//void Close()
//{
//    FindChild("MainPanel").SetActive(false);
//}

//void ShowDialogue(Dialogue dialogue)
//{
//    FindChild("MainPanel").SetActive(true);
//    nameObject.GetComponent<TextMeshProUGUI>().text = dialogue.speakerName;
//    contentObject.GetComponent<TextMeshProUGUI>().text = dialogue.content;

//    for (int i = 0; i < optionListObject.transform.childCount; i++)
//    {
//        optionListObject.transform.GetChild(i).gameObject.SetActive(false);
//    }

//    for (int i = 0; i < dialogue.dialogueOptions.Length; i++)
//    {
//        optionListObject.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.dialogueOptions[i].content;
//        optionListObject.transform.GetChild(i).gameObject.SetActive(true);
//    }

//    float h = dialogue.dialogueOptions.Length * optionListObject.GetComponent<GridLayoutGroup>().cellSize.y / 2;
//    optionListObject.GetComponent<RectTransform>().localPosition = new Vector3(0, h, 0);
//}
//}
