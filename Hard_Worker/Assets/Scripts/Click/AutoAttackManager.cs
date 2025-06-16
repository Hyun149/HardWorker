using UnityEngine;
using TMPro;

/// <summary>
/// 자동 공격 업그레이드 단계를 정의하는 데이터 클래스입니다.
/// </summary>
[System.Serializable]
public class AutoAttackUpgrade
{
    public int level;
    public int requiredPlayerLevel;
    public float attackSpeedBonus; // 공격 속도 증가율 (%)
    public int upgradeCost;
    public string description;
}

/// <summary>
/// 자동 공격 시스템의 레벨 업그레이드 및 UI를 관리하는 클래스입니다.
/// </summary>
public class AutoAttackManager : MonoBehaviour
{
    [Header("자동 공격 업그레이드 설정")]
    [SerializeField]
    private AutoAttackUpgrade[] autoAttackUpgrades = new AutoAttackUpgrade[]
    {
        new AutoAttackUpgrade { level = 1, requiredPlayerLevel = 5, attackSpeedBonus = 0f, upgradeCost = 0, description = "자동 공격 해금" },
        new AutoAttackUpgrade { level = 2, requiredPlayerLevel = 10, attackSpeedBonus = 10f, upgradeCost = 100, description = "공격 속도 10% 증가" },
        new AutoAttackUpgrade { level = 3, requiredPlayerLevel = 15, attackSpeedBonus = 20f, upgradeCost = 500, description = "공격 속도 20% 증가" },
        new AutoAttackUpgrade { level = 4, requiredPlayerLevel = 20, attackSpeedBonus = 35f, upgradeCost = 1000, description = "공격 속도 35% 증가" },
        new AutoAttackUpgrade { level = 5, requiredPlayerLevel = 25, attackSpeedBonus = 50f, upgradeCost = 2500, description = "공격 속도 50% 증가" }
    };

    [Header("참조")]
    [SerializeField] private ClickEventHandler clickEventHandler;

    [Header("UI 참조")]
    [SerializeField] private GameObject autoAttackUI;
    [SerializeField] private TextMeshProUGUI autoAttackLevelText;
    [SerializeField] private TextMeshProUGUI autoAttackSpeedText;
    [SerializeField] private TextMeshProUGUI nextUpgradeText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private GameObject upgradeButton;

    private int currentAutoAttackLevel = 0;

    /// <summary>
    /// 시작 시 참조 설정 및 UI 초기화
    /// </summary>
    void Start()
    {
        if (clickEventHandler == null)
        {
            clickEventHandler = FindObjectOfType<ClickEventHandler>();
        }

        UpdateUI();
    }

    /// <summary>
    /// 자동 공격을 업그레이드합니다.
    /// - 조건: 최대 레벨이 아님, (주석 처리됨: 플레이어 레벨 및 골드 조건)
    /// </summary>
    public void UpgradeAutoAttack()
    {
        if (currentAutoAttackLevel >= autoAttackUpgrades.Length - 1)
        {
            Debug.Log("자동 공격이 최대 레벨입니다!");
            return;
        }

        AutoAttackUpgrade nextUpgrade = autoAttackUpgrades[currentAutoAttackLevel + 1];

        // 플레이어 레벨 확인
        // if (GameManager.Instance.GetPlayerLevel() < nextUpgrade.requiredPlayerLevel)
        // {
        //     Debug.Log($"플레이어 레벨 {nextUpgrade.requiredPlayerLevel} 필요!");
        //     return;
        // }

        // 골드 확인
        // if (GameManager.Instance.GetPlayerGold() < nextUpgrade.upgradeCost)
        // {
        //     Debug.Log("골드가 부족합니다!");
        //     return;
        // }

        // 업그레이드 적용
        currentAutoAttackLevel++;
        clickEventHandler.SetAutoAttackLevel(currentAutoAttackLevel);

        // 골드 차감 (GameManager에서 처리)
        // GameManager.Instance.SpendGold(nextUpgrade.upgradeCost);

        Debug.Log($"자동 공격 레벨 {currentAutoAttackLevel}로 업그레이드!");
        UpdateUI();
    }

    /// <summary>
    /// 자동 공격 UI를 최신 정보로 갱신합니다.
    /// </summary>
    void UpdateUI()
    {
        if (autoAttackUI == null) return;

        // 자동 공격이 해금되었는지 확인
        if (clickEventHandler.IsAutoAttackUnlocked())
        {
            autoAttackUI.SetActive(true);

            // 현재 레벨 표시
            if (autoAttackLevelText != null)
                autoAttackLevelText.text = $"자동 공격 Lv.{currentAutoAttackLevel}";

            // 현재 공격 속도 표시
            if (autoAttackSpeedText != null)
            {
                float interval = clickEventHandler.GetCurrentAutoAttackInterval();
                autoAttackSpeedText.text = $"공격 속도: {interval:F2}초";
            }

            // 다음 업그레이드 정보
            if (currentAutoAttackLevel < autoAttackUpgrades.Length - 1)
            {
                AutoAttackUpgrade nextUpgrade = autoAttackUpgrades[currentAutoAttackLevel + 1];

                if (nextUpgradeText != null)
                    nextUpgradeText.text = nextUpgrade.description;

                if (upgradeCostText != null)
                    upgradeCostText.text = $"비용: {nextUpgrade.upgradeCost} 골드";

                if (upgradeButton != null)
                    upgradeButton.SetActive(true);
            }
            else
            {
                // 최대 레벨
                if (nextUpgradeText != null)
                    nextUpgradeText.text = "최대 레벨 달성!";

                if (upgradeCostText != null)
                    upgradeCostText.text = "";

                if (upgradeButton != null)
                    upgradeButton.SetActive(false);
            }
        }
        else
        {
            autoAttackUI.SetActive(false);
        }
    }

    /// <summary>
    /// 자동 공격이 해금되었을 때 호출됩니다.
    /// 초기 레벨을 1로 설정하고 UI를 갱신합니다.
    /// </summary>
    public void OnAutoAttackUnlocked()
    {
        currentAutoAttackLevel = 1;
        UpdateUI();
    }

    /// <summary>
    /// 현재 자동 공격 레벨을 반환합니다.
    /// </summary>
    public int GetCurrentAutoAttackLevel()
    {
        return currentAutoAttackLevel;
    }

    /// <summary>
    /// 특정 레벨의 자동 공격 업그레이드 정보를 반환합니다.
    /// </summary>
    /// <param name="level">요청할 업그레이드 레벨</param>
    /// <returns>해당 레벨의 업그레이드 정보 또는 null</returns>
    public AutoAttackUpgrade GetUpgradeInfo(int level)
    {
        if (level < 0 || level >= autoAttackUpgrades.Length)
            return null;

        return autoAttackUpgrades[level];
    }
}