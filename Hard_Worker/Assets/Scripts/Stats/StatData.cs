using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스탯의 기본값과 업그레이드 수치를 저장하는 ScriptableObject 클래스
/// ex) 체력, 공격력, 방어력 등 각각의 스탯에 대해 별도의 StatData 자산을 생성한다.
/// </summary>
[CreateAssetMenu(fileName = "NewStatData", menuName = "Stats/StatData")]
public class StatData : ScriptableObject
{
    /// <summary>
    /// 이 StatData가 어떤 스탯을 나타내는지 구분하는 타입
    /// </summary>
    [SerializeField] private StatType statType;

    /// <summary>
    /// 업그레이드 이전의 기본 스탯 값
    /// 능력치가 0레벨일 경우에 보유할 기본 스텟
    /// </summary>
    [SerializeField] private float baseValue;

    /// <summary>
    /// 업그레이드 단계별로 증가하는 스탯 값의 목록
    /// 인덱스 0 = 1레벨 업그레이드 시 증가량
    /// 인덱스 1 = 2레벨 업그레이드 시 증가량 ...
    /// 예: [10f, 20f, 30f] → 업그레이드마다 누적합으로 계산 가능
    /// </summary>
    [SerializeField] private List<float> upgradeValues = new List<float>();

    /// <summary>
    /// 외부에서 스탯 타입을 읽을 수 있도록 하는 프로퍼티
    /// </summary>
    public StatType StatType => statType;

    /// <summary>
    /// 외부에서 기본값을 읽을 수 있도록 하는 프로퍼티
    /// </summary>
    public float BaseValue => baseValue;

    /// <summary>
    /// 외부에서 업그레이드 수치를 읽을 수 있도록 하는 프로퍼티
    /// </summary>
    public List<float> UpgradeValues => upgradeValues;
}