// ?????? ????

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : CharactorBase
{
    private WizardSkills wizardSkills = null;

    private void Start()
    {
        wizardSkills = FindObjectOfType<WizardSkills>();
    }

    public override void OnSkillHit(SkillEnum skillEnum)
    {
        base.OnSkillHit(skillEnum);
        if (!wizardSkills.GetInv())
        {
            TakeDamage(LastHitSkill.damage);
            wizardSkills.SetInv(false);
        }
    }
}
