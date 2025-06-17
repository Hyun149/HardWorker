using UnityEngine;
using TMPro;

/// <summary>
/// 보유 골드를 실시간으로 표시하는 UI 제어 스크립트입니다.
/// - 골드 변화 시 자동으로 UI를 갱신합니다.
/// - 한국식 숫자 단위(만, 억)로 보기 좋게 포맷팅됩니다.
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
        goldText.text = $"Gold: {FormatKoreanNumber(amount)}";
    }

    /// <summary>
    /// 한국식 숫자 단위(만, 억)로 포맷팅하여 문자열로 반환합니다.
    /// 예: 15_000 → "1.5만", 125_000_000 → "1.2억"
    /// </summary>
    private string FormatKoreanNumber(long num)
    {
        string result;

        // 1억(100,000,000) 이상일 경우 → "1.2억" 형식으로 표시
        if (num >= 100_000_000)
            result = (num / 100_000_000f).ToString("0.#") + "억";

        // 1만(10,000) 이상일 경우 → "1.5만", "250만" 형식으로 표시
        else if (num >= 10_000)
            result = (num / 10_000f).ToString("0.#") + "만";

        // 1만 미만이면 숫자 그대로 표시 (예: 9500)
        else
            result = num.ToString();

        // 정수형 소수점 제거: "1.0만" → "1만" 으로 깔끔하게 표현
        return result.Replace(".0", "");
    }
}
