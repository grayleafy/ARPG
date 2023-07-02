using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class InteractPanel : BasePanel
{
    InteractController interactableObject;

    [SerializeField] GameObject showPos;
    [SerializeField] Vector3 worldOffset = new Vector3(0, 1, 0);
    [SerializeField] Vector3 uiOffset = new Vector3(0, 100, 0);

    List<InteractButtonPanel> buttons = new();


    public override void ShowMe()
    {
        base.ShowMe();
        gameObject.GetComponent<RectTransform>().SetParent(UIManager.GetInstance().GetLayerFather(E_UI_Layer.Bot));
    }

    public void SetInteract(InteractController interact)
    {
        if (interactableObject != interact)
        {
            interactableObject = interact;
            //清空
            for (int i = buttons.Count - 1; i >= 0; i--)
            {
                Destroy(buttons[i].gameObject);
                buttons.RemoveAt(i);
            }
            //新建
            for (int i = 0; i < interactableObject.buttons.Count; i++)
            {
                int temp = i;
                ResMgr.GetInstance().LoadAsync<GameObject>("UI/InteractButtonPanel", (panel) =>
                {
                    buttons.Add(panel.GetComponent<InteractButtonPanel>());
                    panel.GetComponent<RectTransform>().SetParent(showPos.GetComponent<RectTransform>(), false);
                    panel.GetComponent<InteractButtonPanel>().SetInteractButton(interactableObject.buttons[temp]);
                });
            }
        }
    }

    private void Update()
    {
        Vector3 pos = interactableObject.transform.position + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPos, null, out Vector2 uiPos);

        showPos.GetComponent<RectTransform>().localPosition = new Vector3(uiPos.x, uiPos.y, 0) + uiOffset;
    }
}
