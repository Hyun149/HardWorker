/// <summary>
/// 무기 데이터(WeaponDataSO)를 기반으로 무기의 현재 강화 레벨과 스탯을 관리하는 클래스입니다.
/// 강화 레벨에 따라 공격력, 크리티컬 확률, 강화 비용 등을 조회할 수 있습니다.
/// </summary>
public class Weapon
{
    public WeaponDataSO data;
    private int enhanceLevel;


    /// <summary>
    /// 무기 인스턴스를 생성합니다.
    /// </summary>
    /// <param name="data">참조할 무기 데이터 (ScriptableObject)</param>
    public Weapon(WeaponDataSO data)
    {
        this.data = data;
        enhanceLevel = 0;
    }

    /// <summary>
    /// 현재 강화 레벨 기준의 공격력 수치를 반환합니다.
    /// </summary>
    /// <returns>공격력 수치</returns>
    public int GetAttack() => data.enhancementTable[enhanceLevel].attack;

    /// <summary>
    /// 현재 강화 레벨 기준의 크리티컬 확률을 반환합니다.
    /// </summary>
    /// <returns>크리티컬 확률 (0.0 ~ 1.0)</returns>
    public float GetCriticalRate() => data.enhancementTable[enhanceLevel].criticalRate;

    /// <summary>
    /// 현재 강화 레벨에서 다음 레벨로 강화하는 데 필요한 비용을 반환합니다.
    /// </summary>
    /// <returns>강화 비용</returns>
    public int GetEnhanceCost() => data.enhancementTable[enhanceLevel].cost;

    /// <summary>
    /// 현재 강화 레벨을 반환합니다.
    /// </summary>
    /// <returns>강화 레벨 (0부터 시작)</returns>
    public int GetLevel() => enhanceLevel;

    /// <summary>
    /// 참조 중인 무기 데이터를 반환합니다.
    /// </summary>
    /// <returns>WeaponDataSO 인스턴스</returns>
    public WeaponDataSO GetData() => data;
    
    /// <summary>
    /// 무기를 1단계 강화합니다.
    /// - 최대 레벨을 초과하지 않도록 제한합니다.
    /// </summary>
    public void Enhance()
    {
        if (enhanceLevel < data.enhancementTable.Count - 1)
            enhanceLevel++;
    }
}
