using UnityEngine;

/// <summary>
/// SashimiSkill : 클릭 4회마다 50% 추가 데미지를 가하는 스킬
/// - IWeaponSkill 인터페이스 구현체
/// - 일정 주기로 추가 공격 효과를 주는 전형적인 퍼센트 기반 추뎀 스킬
/// </summary>
public class SashimiSkill : IWeaponSkill
{
    /// <summary>
    /// 클릭 횟수가 4의 배수일 때 50% 추가 데미지를 가합니다.
    /// </summary>
    /// <param name="target">피해를 입을 적 오브젝트</param>
    /// <param name="clickCount">현재 클릭 횟수</param>
    /// <param name="damage">기본 데미지 수치</param>
    /// <param name="showDamageText">추가 데미지를 표시할 UI 콜백</param>
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
