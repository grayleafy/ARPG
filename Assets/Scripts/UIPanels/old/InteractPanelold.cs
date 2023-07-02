using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//public class InteractPanel : BasePanel
//{
//private void Awake()
//{

//}
//private void Start()
//{
//    InteractManager.Instance.SetUICallback(Refresh);
//    Refresh();
//}

//private void OnDisable()
//{
//    InteractManager.Instance?.RemoveUICallback();
//}

//void Refresh()
//{
//    GameObject list = FindChild("ButtonList");

//    for (int i = 0; i < list.transform.childCount; i++)
//    {
//        list.transform.GetChild(i).gameObject.SetActive(false);
//    }

//    for (int i = 0; i < InteractManager.Instance.interactButtons.Count; i++)
//    {
//        GameObject buttonObject = list.transform.GetChild(i).gameObject;
//        if (buttonObject == null) { break; }
//        buttonObject.SetActive(true);
//        buttonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InteractManager.Instance.interactButtons[i].description;
//        buttonObject.GetComponent<Button>().onClick.RemoveAllListeners();
//        int temp = i;
//        buttonObject.GetComponent<Button>().onClick.AddListener(() => InteractManager.Instance.interactButtons[temp].onClick.Invoke());
//    }

//    float h = InteractManager.Instance.interactButtons.Count * list.GetComponent<GridLayoutGroup>().cellSize.y / 2f;
//    Vector3 pos = list.GetComponent<RectTransform>().localPosition;
//    list.GetComponent<RectTransform>().localPosition = new Vector3(pos.x, h, pos.z);
//}
//}
