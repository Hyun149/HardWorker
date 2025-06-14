using TMPro;
using UnityEngine;

public class FinalStatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cutText;
    [SerializeField] private TMP_Text critText;

    private PlayerStat playerStat;

    private void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        RefreshAllUI();
    }

    public void RefreshAllUI()
    {
        float cut = playerStat.GetFinalStatValue(StatType.Cut);
        float crit = playerStat.GetFinalStatValue(StatType.CritChance);

        cutText.text = $"손질력: {cut}";
        critText.text = $"크리티컬 확률: {crit}%";
    }
}
