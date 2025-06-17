using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendSkill : IWeaponSkill
{
    public void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText)
    {
        if (clickCount % 7 == 0)//클릭 7번당
        {
            Debug.Log("전설의칼 스킬발동");
            GoldManager.Instance.AddGold((int)damage*10); // 데미지*10 만큼 추가 보너스 코인
        }
    }
}
