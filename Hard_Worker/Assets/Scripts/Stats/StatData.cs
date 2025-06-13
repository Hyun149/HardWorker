using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatData : 스탯의 기본값과 업그레이드 수치를 저장하는 ScriptableObject 클래스
/// - 예: 식재료 손질, 완벽한 손질 확률, 완벽한 손질 보너스 등 각각 별도의 StatData 자산으로 생성하여 사용
/// - UI나 강화 시스템에서 참조하여 값 계산
/// </summary>
[CreateAssetMenu(fileName = "NewStatData", menuName = "Stats/StatData")]
public class StatData : ScriptableObject
{
    [Header("스탯 설정")]
    [SerializeField] private StatType statType;           // 이 StatData가 담당하는 능력치 타입
    [SerializeField] private float baseValue;             // 0레벨 기준 스탯 기본 수치

    [Tooltip("10레벨 단위로 증가 수치를 입력")]
    [SerializeField] private List<float> upgradeValues = new List<float>();
    // ※ 레벨 구간별 증가 수치 (10레벨 단위)
    // - Index 0 → 1~10레벨 증가량
    // - Index 1 → 11~20레벨 증가량
    // - Index 2 → 21~30레벨 증가량 ...
    // 값이 부족하면 마지막 값이 반복 적용됨

    [Header("업그레이드 설정")]
    [Tooltip("10레벨 단위로 업그레이드 비용 단가 입력")]
    [SerializeField] private List<int> upgradeCosts = new List<int>();
    // ※ 레벨 구간별 강화 비용 단가 (10레벨 단위)
    // - Index 0 → 1~10레벨 단가
    // - Index 1 → 11~20레벨 단가
    // - Index 2 → 21~30레벨 단가 ...
    // 실제 비용은 이전 구간 누적합 + 현재 구간 단가 * (해당 구간 내 강화 횟수)

    [SerializeField] private int maxLevel = 100;          // 강화 가능한 최대 레벨 제한

    // --- 프로퍼티 영역 ---

    public StatType StatType => statType;                 // 능력치 종류 반환
    public float BaseValue => baseValue;                  // 기본 수치 반환
    public List<float> UpgradeValues => upgradeValues;    // 업그레이드 증가 수치 반환
    public List<int> UpgradeCosts => upgradeCosts;        // 업그레이드 비용 반환
    public int MaxLevel => maxLevel;                      // 최대 레벨 반환
}
