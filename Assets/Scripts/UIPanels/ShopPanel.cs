using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
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

    /// <summary>
    /// 创建之前要先设置背包
    /// </summary>
    /// <param name="bag"></param>
    public void SetBag(CharacterBag bag)
    {
        this.bag = bag;
    }

    public override void ShowMe()
    {
        base.ShowMe();


        //退出按钮
        GetControl<Button>("EscButton").onClick.AddListener(() => UIManager.GetInstance().HidePanel("ShopPanel"));

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
            Debug.Log("lay  " + grid.transform.childCount);
            boxes[i] = grid.transform.GetChild(i).gameObject;
            int currentI = i;
            boxes[i].GetComponent<Button>().onClick.AddListener(() => UpdateCloseUp(currentI));
        }


        //装备的使用和道具的使用
        GetControl<Button>("UseButton").onClick.AddListener(Buy);
    }


    void Buy()
    {

        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<NPCController>().BuyItem(focusedItem.id))
        {

        }
        else
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        if (itemInfo != null)
        {
            Item item = itemInfo.GetItem();
            GetControl<TextMeshProUGUI>("DescriptionTMP").text = item.ToolTip;
            GetControl<Image>("CloseUp").sprite = item.icon;
            GetControl<TextMeshProUGUI>("Price").text = "价格：" + item.purchasePrice.ToString();
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
                throw new NotImplementedException();
            }
            if (itemInfo != null)
            {
                Item item = itemInfo.GetItem();
                boxes[i].transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
                boxes[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }


}
