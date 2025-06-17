using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerStat : 플레이어의 능력치를 관리하는 클래스입니다.
/// - StatData를 기반으로 강화 수치와 최종 스탯을 계산합니다.
/// - 강화 레벨 저장 및 스탯 업그레이드 처리 기능을 포함합니다.
/// - 외부 시스템(무기, 자동 공격 등)과 연동되어 최종 스탯을 계산하거나 자동 공격 해금 조건 등을 관리합니다.
/// </summary>
public class PlayerStat : MonoBehaviour
{
    /// <summary>
    /// 스탯 변경 시 호출되는 이벤트입니다.
    /// - UI 갱신 등 외부 구독자들에게 스탯 변경 알림을 보냅니다.
    /// </summary>
    public event Action onStatChanged;

    [Header("스탯 데이터 연결")]
    [SerializeField] private List<StatData> statDataList; // 능력치 종류별 ScriptableObject 리스트
    [SerializeField] private WeaponManager weaponManager;

    /// <summary>
    /// 주어진 스탯 타입의 강화 수치를 기준으로 기본 능력치를 계산합니다.
    /// - baseValue + 업그레이드 수치 누적
    /// - 10레벨 단위로 UpgradeValues 값을 반복 사용
    /// 이 값은 무기나 버프와 같은 외부 보정치를 포함하지 않은 수치입니다.
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
        return GameManager.Instance.playerData.GetStatLevel(type);
    }

    /// <summary>
    /// UpgradeStat : 능력치 1단계 업그레이드 처리
    /// - 유효성 검사 후 레벨 증가
    /// </summary>
    public void UpgradeStat(StatType type)
    {
        SFXManager.Instance.Play(SFXType.StatEnhance);

        var data = statDataList.Find(d => d.StatType == type);
        if (data == null) return;

        int level = GetStatLevel(type);
        if (level >= data.MaxLevel) return;

        int cost = GetUpgradeCost(type);
        if (!GoldManager.Instance.SpendGold(cost)) return;

        GameManager.Instance.playerData.SetStatLevel(type, level + 1);
        GameManager.Instance.SaveGame(); // 능력치 강화 후 저장

        onStatChanged?.Invoke();

        if (type == StatType.AssistSkill)
        {
            FindObjectOfType<ClickEventHandler>()?.CheckAutoAttackUnlock();
        }
    }

    /// <summary>
    /// CanUpgrade : 업그레이드 가능한지 확인
    /// - 현재 레벨이 maxLevel 미만인지 검사
    /// - 추후 골드 조건 등 추가 예정
    /// </summary>
    public bool CanUpgrade(StatType type)
    {
        StatData data = statDataList.Find(d => d.StatType == type);
        if (data == null) return false;

        int level = GetStatLevel(type);
        if (level >= data.MaxLevel) return false;

        int cost = GetUpgradeCost(type);
        return GoldManager.Instance.CurrentGold >= cost;
    }

    /// <summary>
    /// GetMaxLevel : 주어진 스탯 타입의 최대 강화 레벨을 반환
    ///
    /// UI나 조건 검사에서 특정 스탯이 최대 레벨에 도달했는지 확인할 때 사용된다.
    /// 예: 강화 비용 표시 여부
    /// </summary>
    public int GetMaxLevel(StatType type)
    {
        StatData data = statDataList.Find(d => d.StatType == type);
        return data != null ? data.MaxLevel : 0;
    }

    /// <summary>
    /// 최종 스탯 값을 반환합니다.
    /// - 강화 수치 + 장비 보정치를 포함한 실제 적용 수치
    /// </summary>
    /// <param name="type">스탯 종류</param>
    /// <returns>최종 적용되는 스탯 수치</returns>
    public float GetFinalStatValue(StatType type)
    {
        float baseStat = GetStatValue(type);
        if (type == StatType.Cut || type == StatType.CritChance && weaponManager != null)
        {
            Weapon equipped = weaponManager.GetEquippedWeapon();
            if (equipped != null)
            {
                if (type == StatType.Cut)
                {
                    baseStat += equipped.GetAttack();
                }
                else if (type == StatType.CritChance)
                {
                    baseStat += equipped.GetCriticalRate();
                }
            }
        }

        return baseStat;
    }

    /// <summary>
    /// 외부에서 수동으로 스탯 변경 이벤트를 발생시킬 때 호출합니다.
    /// - 예: 장비 장착, 저장 데이터 로드 후 UI 갱신 필요 시
    /// </summary>
    public void NotifyStatChanged()
    {
        onStatChanged?.Invoke();
    }
}
