using TMPro;
using UnityEngine;

public class FinalStatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cutText;
    [SerializeField] private TMP_Text critText;
    [SerializeField] private TMP_Text critBonusText;
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text assistSpeedText;
    [SerializeField] private TMP_Text assistSkillText;

    private PlayerStat playerStat;

    private void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        playerStat.onStatChanged += RefreshAllUI;
        RefreshAllUI();
    }

    private void OnDestroy()
    {
        if (playerStat != null)
        {
            playerStat.onStatChanged -= RefreshAllUI;
        }
    }

    public void RefreshAllUI()
    {
        float cut = playerStat.GetFinalStatValue(StatType.Cut);
        float crit = playerStat.GetFinalStatValue(StatType.CritChance);

        cutText.text = $"식재료 손질력: {cut}";
        critText.text = $"완벽한 손질 확률: {crit}%";
        critBonusText.text = $"완벽한 손질 보너스: {playerStat.GetFinalStatValue(StatType.CritBonus)}%";
        incomeText.text = $"수익 증가: {playerStat.GetFinalStatValue(StatType.Income)}%";
        assistSpeedText.text = $"보조 셰프 속도: {playerStat.GetFinalStatValue(StatType.AssistSpeed)}s";
        assistSkillText.text = $"보조 셰프 숙련도: {playerStat.GetFinalStatValue(StatType.AssistSkill)}";
    }
}
