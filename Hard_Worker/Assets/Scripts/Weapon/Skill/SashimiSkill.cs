using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SashimiSkill : IWeaponSkill
{
    public void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText)
    {
        if (clickCount % 4 == 0) //클릭4번당
        {
            Debug.Log("사시미칼 스킬발동");
            float bounusDamage = damage / 2;
            target.TakeDamage(bounusDamage); //4번 클릭마다 50% 추뎀
            showDamageText?.Invoke(bounusDamage);
        }
    }
}
