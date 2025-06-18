/// <summary>
/// 무기 스킬 발동 인터페이스
/// </summary>
public interface IWeaponSkill
{
    void Activate(Enemy target, int clickCount, float damage,System.Action<float> showDamageText);
}
/// <summary>
/// 무기 스킬 타입에 따라 해당 스킬 인스턴스를 생성하여 반환하는 팩토리 클래스
/// </summary>
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
