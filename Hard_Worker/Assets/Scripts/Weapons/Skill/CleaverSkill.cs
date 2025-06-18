using UnityEngine;

/// <summary>
/// CleaverSkill : 20% 확률로 3배 피해를 주는 무기 스킬 구현체
/// - IWeaponSkill 인터페이스를 구현하며, 치명타성 확률 스킬에 해당
/// </summary>
public class CleaverSkill : IWeaponSkill
{
    /// <summary>
    /// 스킬을 발동시켜 대상에게 데미지를 입힙니다.
    /// - 20% 확률로 3배 데미지를 가합니다.
    /// </summary>
    /// <param name="target">피해를 입을 대상 적</param>
    /// <param name="clickCount">현재 누적 클릭 수 (사용되지 않음)</param>
    /// <param name="damage">기본 데미지 수치</param>
    /// <param name="showDamageText">데미지 텍스트를 표시하는 콜백</param>
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
