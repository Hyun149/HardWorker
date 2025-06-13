using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatData : 스탯의 기본값과 업그레이드 수치를 저장하는 ScriptableObject 클래스
/// - 예: 체력, 공격력, 방어력 등 각각 별도의 StatData 자산으로 생성하여 사용
/// </summary>
[CreateAssetMenu(fileName = "NewStatData", menuName = "Stats/StatData")]
public class StatData : ScriptableObject
{
    [SerializeField] private StatType statType;                             // 스탯 구분용 타입
    [SerializeField] private float baseValue;                               // 0레벨 기준 기본 스탯 수치
    [SerializeField] private List<float> upgradeValues = new List<float>(); // 레벨업 시 증가 수치 리스트
    [SerializeField] private List<int> upgradeCosts = new List<int>();      // 레벨업 시 요구 비용 리스트

    /// <summary>
    /// StatType : 이 StatData가 어떤 스탯을 나타내는지 반환
    /// </summary>
    public StatType StatType => statType;

    /// <summary>
    /// BaseValue : 기본값 반환 (0레벨 기준)
    /// </summary>
    public float BaseValue => baseValue;

    /// <summary>
    /// UpgradeValues : 레벨업마다 증가하는 수치 리스트 반환
    /// - 인덱스 0 = 1레벨 증가량
    /// - 인덱스 1 = 2레벨 증가량 ...
    /// </summary>
    public List<float> UpgradeValues => upgradeValues;

    /// <summary>
    /// UpgradeCosts : 레벨업마다 필요한 비용 리스트 반환
    /// </summary>
    public List<int> UpgradeCosts => upgradeCosts;
}