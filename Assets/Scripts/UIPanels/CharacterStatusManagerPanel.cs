using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatusManagerPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();

        //已有的创建
        List<GameObject> characters = new();
        characters.AddRange(GameObject.FindGameObjectsWithTag("Player").ToList());
        characters.AddRange(GameObject.FindGameObjectsWithTag("Enemy").ToList());
        characters.AddRange(GameObject.FindGameObjectsWithTag("Ally").ToList());

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            ResMgr.GetInstance().LoadAsync<GameObject>("UI/CharacterStatusPanel", (o) =>
            {
                o.GetComponent<CharacterStatusPanel>().SetCharacter(character);
            }, transform);
        }

        //没有的添加事件
        EventCenter.GetInstance().AddEventListener<GameObject>("角色生成", (character) =>
        {
            ResMgr.GetInstance().LoadAsync<GameObject>("UI/CharacterStatusPanel", (o) =>
            {
                o.GetComponent<CharacterStatusPanel>().SetCharacter(character);
            }, transform);
        });
    }
}
