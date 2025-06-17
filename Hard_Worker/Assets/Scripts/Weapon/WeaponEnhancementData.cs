/// <summary>
/// 강화 시 이용되는 테이블 강화할수록 level이 오르고 그 level에 따라 증가하는 공격력, 크리티컬, 요구 비용이 다름
/// </summary>
[System.Serializable]
public class WeaponEnhancementData
{
    public int level;

    public int attack;

    public float criticalRate;

    public int cost;

}
