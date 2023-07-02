using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractController))]
[RequireComponent(typeof(SceneObjectController))]
public class Door : MonoBehaviour
{
    public int keyID;

    public void OpenDoor()
    {
        GameObject player = GameObject.FindWithTag("Player");
        CharacterBag bag = player.GetComponent<NPCController>().info.bag;

        ItemInfo itemInfo = null;
        itemInfo = bag.usableItems.Find((x) => x.id == keyID);
        if (itemInfo == null)
        {
            itemInfo = bag.weapons.Find((x) => x.id == keyID);
        }

        if (itemInfo == null)
        {
            UIManager.GetInstance().ShowPanel<ToolTipPanel>("ToolTipPanel", E_UI_Layer.Mid, (panel) =>
            {
                panel.SetToolTip("没有相应的钥匙");
            });
        }
        else
        {
            var objectController = GetComponent<SceneObjectController>();
            objectController.CrossFade("OpenDoor");
            objectController.SetInteractivity(false);
        }

    }
}
