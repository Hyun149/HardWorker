using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaverSkill : IWeaponSkill
{
    public void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText)
    {
        if (Random.value < 0.2f)
        {
            float bonusDamage = damage * 3f;//20프로 확률로 3배 데미지
            target.TakeDamage(bonusDamage);
            showDamageText?.Invoke(bonusDamage);
        }
    }
}
