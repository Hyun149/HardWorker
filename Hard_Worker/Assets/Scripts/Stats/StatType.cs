/// <summary>
/// StatType : 게임 내 능력치를 enum으로 정리
/// - UI, StatData, PlayerStat 등에서 공통적으로 사용됨
/// </summary>
public enum StatType
{
    Cut,         // 식재료 손질 – 기본 손질 파워 증가
    CritChance,  // 완벽한 손질 확률 – 완벽한 손질 확률 증가
    CritBonus,   // 완벽한 손질 보너스 – 완벽한 손질이 발생할 시 보너스 효과 상승
    Income,      // 수익 증가 – 요리 판매 시 수익 상승
    AssistSpeed, // 보조 셰프 속도 – 보조 셰프의 작업 속도 증가
    AssistSkill  // 보조 셰프 숙련도 – 보조 셰프의 능력 향상
}