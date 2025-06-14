using UnityEngine;
using TMPro;

/// <summary>
/// 보유 골드를 실시간으로 표시하는 UI 제어 스크립트입니다.
/// </summary>
public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        GoldManager.Instance.onGoldChanged.AddListener(UpdateUI);
        UpdateUI(GoldManager.Instance.CurrentGold);
    }

    private void UpdateUI(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }
}
