using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDroper : Singleton<SkillDroper>
{
    private void Update()
    {

    }

    public void ReleaserSkill(string skillDef, GameObject releaser)
    {
        Skill skill = new Skill();
        skill.InitByDefineString(skillDef);
        skill.releaser = releaser;
        ReleaseSkill(skill);
    }

    public void ReleaseSkill(Skill skill, GameObject releaser)
    {
        skill.releaser = releaser;
        ReleaseSkill(skill);
    }



    void ReleaseSkill(Skill skill)
    {
        for (int i = 0; i < skill.conditions.Length; i++)
        {
            if (skill.conditions[i].ForceCheck() == false)
            {
                Debug.Log("不满足释放条件");
                return;
            }
        }

        Debug.Log("select start");
        StartCoroutine(skill.selectMethod.Run(this, skill, skill.releaser));
    }


}
