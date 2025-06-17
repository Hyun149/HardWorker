
/// <summary>
/// 무기 데이터(WeaponDataSO)를 기반으로 무기의 현재 강화 레벨과 스탯을 관리하는 클래스입니다.
/// 강화 레벨에 따라 공격력, 크리티컬 확률, 강화 비용 등을 조회할 수 있습니다.
/// </summary>
public class Weapon
{
    public WeaponDataSO data;
    private int enhanceLevel;
    
    public Weapon(WeaponDataSO data)
    {
        this.data = data;
        enhanceLevel = 0;
    }
    
    public int GetAttack() => data.enhancementTable[enhanceLevel].attack;
    public float GetCriticalRate() => data.enhancementTable[enhanceLevel].criticalRate;
    public int GetEnhanceCost() => data.enhancementTable[enhanceLevel].cost;
    public void Enhance() { if (enhanceLevel < data.enhancementTable.Count - 1) enhanceLevel++; }
    public int GetLevel() => enhanceLevel;
    public WeaponDataSO GetData() => data;
}
