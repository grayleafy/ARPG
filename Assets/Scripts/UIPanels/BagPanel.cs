using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    CharacterBag bag;
    public enum ShowItemType
    {
        Weapon,
        Consumable,
    }
    ShowItemType showItemType = ShowItemType.Weapon;

    //物品格子
    GameObject[] boxes;

    //当前选中的道具
    Item focusedItem = null;


    public override void ShowMe()
    {
        base.ShowMe();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bag = player.GetComponent<NPCController>().info.bag;

        //退出按钮
        GetControl<Button>("EscButton").onClick.AddListener(() => UIManager.GetInstance().HidePanel("BagPanel"));

        //装备和物品的切换
        GetControl<Toggle>("Consumable").onValueChanged.AddListener((x) =>
        {
            showItemType = ShowItemType.Consumable;
        });
        GetControl<Toggle>("Equip").onValueChanged.AddListener((x) =>
        {
            showItemType = ShowItemType.Weapon;
        });
        GetControl<Toggle>("Equip").isOn = true;

        //获取格子
        GameObject grid = GetControl<GridLayoutGroup>("Grid").gameObject;
        boxes = new GameObject[grid.transform.childCount];
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            //Debug.Log("lay  " + grid.transform.childCount);
            boxes[i] = grid.transform.GetChild(i).gameObject;
            int currentI = i;
            boxes[i].GetComponent<Button>().onClick.AddListener(() => UpdateCloseUp(currentI));
        }


        //装备的使用和道具的使用
        GetControl<Button>("UseButton").onClick.AddListener(UseOrEquip);
    }


    //使用道具
    void UseOrEquip()
    {
        if (focusedItem is Weapon)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NPCController>().EquipWeapon(focusedItem as Weapon);
        }
        else if (focusedItem is UsableItem)
        {
            GameObject player = GameObject.FindWithTag("Player");
            UsableItem usableItem = focusedItem as UsableItem;
            BehaviorTreeNode useBehavior = usableItem.useBehavior.DeepClone();

            useBehavior.SetValue<GameObject>("gameObject", player);
            useBehavior.Init(null);

            if (!(useBehavior is BT.Entry))
            {
                Debug.LogError("道具行为根节点不是入口节点");
            }
            else
            {
                ((BT.Entry)useBehavior).StartExecute();
            }

        }
    }

    //更新选中的物品
    private void UpdateCloseUp(int currentI)
    {
        int index = currentI;
        ItemInfo itemInfo = null;
        if (showItemType == ShowItemType.Weapon)
        {
            if (index < bag.weapons.Count)
                itemInfo = bag.weapons[index];
        }
        else if (showItemType == ShowItemType.Consumable)
        {
            if (index < bag.usableItems.Count)
                itemInfo = bag.usableItems[index];
        }
        if (itemInfo != null)
        {
            Item item = itemInfo.GetItem();
            GetControl<TextMeshProUGUI>("DescriptionTMP").text = item.ToolTip;
            GetControl<Image>("CloseUp").sprite = item.icon;
            focusedItem = item;
        }
    }

    private void Update()
    {
        ShowGrid();
    }

    //更新网格
    void ShowGrid()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            int index = i;
            ItemInfo itemInfo = null;
            if (showItemType == ShowItemType.Weapon)
            {
                if (index < bag.weapons.Count)
                    itemInfo = bag.weapons[index];
            }
            else if (showItemType == ShowItemType.Consumable)
            {
                if (index < bag.usableItems.Count)
                    itemInfo = bag.usableItems[index];
            }
            if (itemInfo != null)
            {
                Item item = itemInfo.GetItem();
                boxes[i].transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
                boxes[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
            }
            else
            {
                boxes[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
        }
    }


}
