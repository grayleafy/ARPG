using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel_old : BasePanel
{
    //[Header("打开的背包")]
    //[SerializeField] Bag openBag;

    //[Header("翻页相关")]
    //[SerializeField] int page = 1;
    //[SerializeField] int totalPage;
    //[SerializeField] int currentSelectedIndex = 0;


    ////物品格子
    //GameObject[] boxes;

    ////左边的放大信息
    //GameObject closeUpImage;
    //GameObject descriptionTMP;

    protected override void Awake()
    {
        base.Awake();

        //FindChild("EscButton").GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.CloseUI(gameObject));

        //获取格子
        //GameObject grid = FindChild("Grid");
        //boxes = new GameObject[grid.transform.childCount];
        //for (int i = 0; i < grid.transform.childCount; i++)
        //{
        //    boxes[i] = grid.transform.GetChild(i).gameObject;
        //    int currentI = i;
        //    boxes[i].GetComponent<Button>().onClick.AddListener(() => UpdateCloseUp(currentI));
        //}

        //获取左边的控件
        //closeUpImage = FindChild("CloseUp");
        //descriptionTMP = FindChild("DescriptionTMP");

        //FindChild("Consumable").GetComponent<Toggle>().onValueChanged.AddListener(ChangeShowItems);
        //FindChild("Equip").GetComponent<Toggle>().onValueChanged.AddListener(ChangeShowItems);
        //FindChild("Prop").GetComponent<Toggle>().onValueChanged.AddListener(ChangeShowItems);

        //FindChild("UseButton").GetComponent<Button>().onClick.AddListener(UseItem);
    }

    //private void OnEnable()
    //{
    //    openBag = DataManager.Instance.player.GetComponent<Bag>();

    //    ChangeShowItems(true);
    //}

    //点击不同的分类标签，切换显示的物品类型
    void ChangeShowItems(bool value)
    {
        //List<ItemInfo> itemList = new();
        //if (FindChild("Consumable").GetComponent<Toggle>().isOn)
        //{
        //    itemList = DataManager.Instance.player.GetComponent<Bag>().consumables;
        //}
        //if (FindChild("Equip").GetComponent<Toggle>().isOn)
        //{
        //    itemList = DataManager.Instance.player.GetComponent<Bag>().equips;
        //}
        //if (FindChild("Prop").GetComponent<Toggle>().isOn)
        //{
        //    itemList = DataManager.Instance.player.GetComponent<Bag>().props;
        //}

        //ShowItems(itemList);
    }

    //展示背包中的物品
    void ShowItems(List<ItemInfo> iteminfos)
    {
        //for (int i = 0; i < boxes.Length; i++)
        //{
        //    int itemIndex = i + (page - 1) * boxes.Length;
        //    if (itemIndex < iteminfos.Count)
        //    {
        //        FindChild(boxes[i], "Image").GetComponent<Image>().sprite = iteminfos[itemIndex].GetItem().icon;
        //        FindChild(boxes[i], "Image").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        //    }
        //    else
        //    {
        //        FindChild(boxes[i], "Image").GetComponent<Image>().color = new Color(255, 255, 255, 0);
        //    }

        //}
    }

    //更新左边的选中信息
    void UpdateCloseUp(int index)
    {
        //if (FindChild("Consumable").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.consumables.Count)
        //{
        //    currentSelectedIndex = index;
        //    Item item = openBag.consumables[index + (page - 1) * boxes.Length].GetItem();
        //    closeUpImage.GetComponent<Image>().sprite = item.icon;
        //    descriptionTMP.GetComponent<TextMeshProUGUI>().text = item.description;
        //}
        //if (FindChild("Equip").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.equips.Count)
        //{
        //    currentSelectedIndex = index;
        //    Item item = openBag.equips[index + (page - 1) * boxes.Length].GetItem();
        //    closeUpImage.GetComponent<Image>().sprite = item.icon;
        //    descriptionTMP.GetComponent<TextMeshProUGUI>().text = item.description;
        //}
        //if (FindChild("Prop").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.props.Count)
        //{
        //    currentSelectedIndex = index;
        //    Item item = openBag.props[index + (page - 1) * boxes.Length].GetItem();
        //    closeUpImage.GetComponent<Image>().sprite = item.icon;
        //    descriptionTMP.GetComponent<TextMeshProUGUI>().text = item.description;
        //}
    }

    //使用道具
    void UseItem()
    {
        //int index = currentSelectedIndex + (page - 1) * boxes.Length;
        ////获取iteminfo
        //Item item = null;
        //if (FindChild("Consumable").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.consumables.Count)
        //{
        //    item = openBag.consumables[index].GetItem();
        //}
        //if (FindChild("Equip").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.equips.Count)
        //{
        //    item = openBag.equips[index].GetItem();
        //}
        //if (FindChild("Prop").GetComponent<Toggle>().isOn && index + (page - 1) * boxes.Length < openBag.props.Count)
        //{
        //    item = openBag.props[index].GetItem();
        //}

        //if (item is Consumable)
        //{
        //    var consumable = (Consumable)item;
        //    SkillDroper.Instance.ReleaserSkill(consumable.skillDefine, DataManager.Instance.player);
        //}

        //UIManager.Instance.CloseUI(gameObject);
    }
}
