using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerStat : 플레이어의 능력치를 관리하는 클래스
/// - StatData를 기반으로 현재 능력치 값 계산
/// - 강화 레벨 저장 및 스탯 업그레이드 처리
/// </summary>
public class PlayerStat : MonoBehaviour
{
    [Header("스탯 데이터 연결")]
    [SerializeField] private List<StatData> statDataList; // 능력치 종류별 ScriptableObject 리스트

    private Dictionary<StatType, int> statLevels = new(); // 각 능력치별 현재 레벨 저장

    /// <summary>
    /// GetStatValue : 최종 능력치 값 계산
    /// - baseValue + 업그레이드 수치 누적
    /// - 10레벨 단위로 UpgradeValues 값을 반복 사용
    /// </summary>
    public float GetStatValue(StatType type)
    {
        StatData data = statDataList.Find(d => d.StatType == type);
        if (data == null) return 0;

        int level = GetStatLevel(type);
        float value = data.BaseValue;

        for (int i = 0; i < level; i++)
        {
            if (data.UpgradeValues.Count == 0)
                break;

            int valueIndex = Mathf.Min(i / 10, data.UpgradeValues.Count - 1); // 구간 인덱스
            value += data.UpgradeValues[valueIndex];
        }

        return value;
    }

    /// <summary>
    /// GetUpgradeCost : 현재 레벨 기준 누적 강화 비용 반환
    /// - UpgradeCosts 구간별 단가를 10레벨 단위로 반복 누적
    /// - 다음 레벨까지의 총 비용을 반환
    /// </summary>
    public int GetUpgradeCost(StatType type)
    {
        StatData data = statDataList.Find(d => d.StatType == type);
        if (data == null || data.UpgradeCosts.Count == 0) return 0;

        int level = GetStatLevel(type);
        int cost = 0;

        for (int i = 0; i <= level; i++)
        {
            int tierIndex = Mathf.Min(i / 10, data.UpgradeCosts.Count - 1); // 10레벨 단위 구간
            cost += data.UpgradeCosts[tierIndex];
        }

        return cost;
    }

    /// <summary>
    /// GetStatLevel : 현재 능력치 강화 레벨 반환
    /// - 강화 이력이 없으면 0
    /// </summary>
    public int GetStatLevel(StatType type)
    {
        return statLevels.ContainsKey(type) ? statLevels[type] : 0;
    }

    /// <summary>
    /// UpgradeStat : 능력치 1단계 업그레이드 처리
    /// - 유효성 검사 후 레벨 증가
    /// - 추후 골드 시스템 추가 시 비용 비교 및 차감 위치 있음
    /// </summary>
    public void UpgradeStat(StatType type)
    {
        if (!CanUpgrade(type)) return;

        // TODO: 골드 시스템 연동 예시
        // int cost = GetUpgradeCost(type);
        // if (!playerInventory.HasEnoughGold(cost)) return;
        // playerInventory.UseGold(cost);

        if (!statLevels.ContainsKey(type))
            statLevels[type] = 0;

        statLevels[type]++;
    }

    /// <summary>
    /// CanUpgrade : 업그레이드 가능한지 확인
    /// - 현재 레벨이 maxLevel 미만인지 검사
    /// - 추후 골드 조건 등 추가 예정
    /// </summary>
    public bool CanUpgrade(StatType type)
    {
        StatData data = statDataList.Find(d => d.StatType == type);
        int level = GetStatLevel(type);

        // TODO: 골드 조건을 추가하려면 여기서 비교
        // int cost = GetUpgradeCost(type);
        // return data != null && level < data.MaxLevel && playerInventory.HasEnoughGold(cost);

        return data != null && level < data.MaxLevel;
    }
}
