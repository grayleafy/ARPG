using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusPanel : BasePanel
{
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 uiOffset = new Vector3(0, 80, 0);

    GameObject character = null;
    NPCInfo info;

    public void SetCharacter(GameObject character)
    {
        this.character = character;
        info = character.GetComponent<NPCController>().info;
    }

    private void Update()
    {
        //Debug.Log("update");

        if (character != null && character.activeInHierarchy && info.health > 0)
        {
            GetControl<Slider>("HealthBar").value = info.health / info.maxHealth;
            GetControl<Slider>("ToughnessBar").value = info.toughness / info.maxToughness;

            //跟随位置
            Vector3 pos = character.transform.position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>().parent as RectTransform, screenPos, null, out Vector2 uiPos);
            GetComponent<RectTransform>().localPosition = new Vector3(uiPos.x, uiPos.y, 0) + uiOffset;


            ////判断是否被遮挡,单面透明墙？？
            //Vector3 dir = pos - Camera.main.transform.position;
            //if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hitInfo, dir.magnitude))
            //{
            //    if (hitInfo.collider.gameObject != character)
            //    {
            //        for (int i = 0; i < transform.childCount; i++)
            //        {
            //            transform.GetChild(i).gameObject.SetActive(false);
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < transform.childCount; i++)
            //    {
            //        transform.GetChild(i).gameObject.SetActive(true);
            //    }
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
