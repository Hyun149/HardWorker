using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어의 최종 스탯을 UI로 표시하는 클래스입니다.
/// 스탯이 변경될 때마다 UI를 자동으로 갱신합니다.
/// </summary>
public class FinalStatUI : MonoBehaviour
{
    [Header("스탯 텍스트")]
    [SerializeField] private TMP_Text cutText;             // 식재료 손질력
    [SerializeField] private TMP_Text critText;            // 완벽 손질 확률
    [SerializeField] private TMP_Text critBonusText;       // 완벽 손질 보너스
    [SerializeField] private TMP_Text incomeText;          // 수익 증가율
    [SerializeField] private TMP_Text assistSpeedText;     // 보조 셰프 속도
    [SerializeField] private TMP_Text assistSkillText;     // 보조 셰프 숙련도

    private PlayerStat playerStat;

    /// <summary>
    /// 컴포넌트 초기화 시 호출됩니다.
    /// PlayerStat을 찾아서 스탯 변경 이벤트에 등록하고, UI를 갱신합니다.
    /// </summary>
    private void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        playerStat.onStatChanged += RefreshAllUI;
        RefreshAllUI();
    }

    /// <summary>
    /// 오브젝트 파괴 시 이벤트 등록 해제합니다.
    /// </summary>
    private void OnDestroy()
    {
        if (playerStat != null)
        {
            playerStat.onStatChanged -= RefreshAllUI;
        }
    }

    /// <summary>
    /// 현재 플레이어의 모든 스탯 값을 UI 텍스트로 갱신합니다.
    /// </summary>
    public void RefreshAllUI()
    {
        float cut = playerStat.GetFinalStatValue(StatType.Cut);          // 장비로 인한 증가량 포함 (손질력)
        float crit = playerStat.GetFinalStatValue(StatType.CritChance);  // 장비로 인한 증가량 포함 (완벽한 손질 확률) 

        cutText.text = $"식재료 손질력: {cut}";
        critText.text = $"완벽한 손질 확률: {crit}%";
        critBonusText.text = $"완벽한 손질 보너스: {playerStat.GetFinalStatValue(StatType.CritBonus)}%";
        incomeText.text = $"수익 증가: {playerStat.GetFinalStatValue(StatType.Income)}%";
        assistSpeedText.text = $"보조 셰프 속도: {playerStat.GetFinalStatValue(StatType.AssistSpeed)}s";
        assistSkillText.text = $"보조 셰프 숙련도: {playerStat.GetFinalStatValue(StatType.AssistSkill)}";
    }
}
