using UnityEngine;

/// <summary>
/// LegendSkill : 클릭 7회마다 발동되는 골드 보너스 스킬
/// - IWeaponSkill 인터페이스를 구현한 특수 무기 스킬
/// - 7번째 클릭마다 현재 데미지 × 10 만큼 골드를 획득함
/// </summary>
public class LegendSkill : IWeaponSkill
{
    /// <summary>
    /// 스킬을 발동시켜 보너스 골드를 지급합니다.
    /// - 클릭 수가 7의 배수일 때 발동됩니다.
    /// </summary>
    /// <param name="target">공격 대상 적 (사용되지 않음)</param>
    /// <param name="clickCount">현재 클릭 횟수</param>
    /// <param name="damage">기본 데미지 수치</param>
    /// <param name="showDamageText">데미지 텍스트 표시 콜백 (사용되지 않음)</param>
    public void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText)
    {
        if (clickCount % 7 == 0)//클릭 7번당
        {
            Debug.Log("전설의칼 스킬발동");
            GoldManager.Instance.AddGold((int)damage*10); // 데미지*10 만큼 추가 보너스 코인
        }
    }
}
