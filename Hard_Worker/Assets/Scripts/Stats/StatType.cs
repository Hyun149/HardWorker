/// <summary>
/// 게임 내 능력치를 enum 으로 정리
/// UI, StatData, PlayerStat 등에서 공통적으로 사용
/// </summary>
public enum StatType
{
    /// <summary>식재료 손질</summary>
    Cut,

    /// <summary>완벽한 손질 확률</summary>
    CritChance,

    /// <summary>완벽한 손질 보너스</summary>
    CritBonus,

    /// <summary>수익 증가</summary>
    Income,

    /// <summary>보조 셰프 속도</summary>
    AssistSpeed,

    /// <summary>보조 셰프 숙련도</summary>
    AssistSkill
}
