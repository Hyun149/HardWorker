using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponSkill
{
    void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText);
}
public static class WeaponSkillFactory
{
    public static IWeaponSkill GetSkill(WeaponSkillType type)
    {
        switch (type)
        {
            case WeaponSkillType.Sashimi: return new SashimiSkill();
            case WeaponSkillType.Cleaver: return new CleaverSkill();
            case WeaponSkillType.Legend: return new LegendSkill();
            default: return null;
        }
    }
}
