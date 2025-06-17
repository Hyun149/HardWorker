using UnityEngine;

/// <summary>
/// 자동 공격 업그레이드 단계를 정의하는 데이터 클래스입니다.
/// </summary>
[System.Serializable]
public class AutoAttackUpgrade
{
    public int requiredPlayerLevel;
    public int unlockCost;
}

/// <summary>
/// 보조셰프(자동 공격) 해금 및 강화 레벨을 관리합니다.
/// 성능 수치는 StatManager에서 관리하며, UI 텍스트는 사용하지 않습니다.
/// </summary>
public class AutoAttackManager : MonoBehaviour
{
    [SerializeField] private AutoAttackUpgrade[] upgrades;
    [SerializeField] private ClickEventHandler clickEventHandler;

    private int currentLevel = 0;

    /// <summary>
    /// 시작 시 참조를 초기화하고 현재 자동 공격 레벨을 적용합니다.
    /// </summary>
    void Start()
    {
        if (clickEventHandler == null)
        {
            clickEventHandler = FindObjectOfType<ClickEventHandler>();
        }

        ApplyLevel();
    }

    /// <summary>
    /// 자동 공격 레벨을 한 단계 업그레이드합니다.
    /// 최대 레벨 도달 시 동작하지 않습니다.
    /// </summary>
    public void Upgrade()
    {
        if (currentLevel >= upgrades.Length - 1)
        {
            return;
        }

        currentLevel++;
        ApplyLevel();

        clickEventHandler.SetPlayerLevel(currentLevel);
    }

    /// <summary>
    /// 현재 자동 공격 레벨을 ClickEventHandler에 적용합니다.
    /// </summary>
    private void ApplyLevel()
    {
        clickEventHandler.SetAutoAttackLevel(currentLevel);
    }

    /// <summary>
    /// 자동 공격이 해금되었을 때 호출되는 메서드입니다.
    /// 레벨을 1로 설정하고 적용합니다.
    /// </summary>
    public void OnAutoAttackUnlocked()
    {
        currentLevel = 1;
        ApplyLevel();
    }

    /// <summary>
    /// 자동 공격이 해금되었는지 여부를 반환합니다.
    /// </summary>
    /// <returns>해금되었으면 true, 아니면 false</returns>
    public bool IsUnlocked() => currentLevel > 0;

    /// <summary>
    /// 현재 자동 공격 레벨을 반환합니다.
    /// </summary>
    public int GetCurrentLevel() => currentLevel;

    /// <summary>
    /// 특정 레벨의 업그레이드 정보를 반환합니다.
    /// 범위를 벗어나면 null을 반환합니다.
    /// </summary>
    /// <param name="level">조회할 업그레이드 레벨</param>
    /// <returns>업그레이드 정보 또는 null</returns>
    public AutoAttackUpgrade GetUpgradeInfo(int level)
    {
        if (level < 0 || level >= upgrades.Length)
        {
            return null;
        }

        return upgrades[level];
    }
}