using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// 지정된 TextMeshPro 텍스트를 깜빡이도록 처리하는 컴포넌트입니다.
/// DOTween을 사용하여 텍스트의 알파 값을 반복적으로 변화시킵니다.
/// </summary>
public class BlinkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float duration = 1f;

    /// <summary>
    /// 시작 시 텍스트 컴포넌트를 초기화하고 깜빡임을 시작합니다.
    /// </summary>
    private void Start()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TextMeshProUGUI>();
        }

        StartBlinking();
    }

    /// <summary>
    /// DOTween을 사용해 텍스트의 알파 값을 변경하며 깜빡이도록 설정합니다.
    /// </summary>
    private void StartBlinking()
    {
        targetText.DOFade(0f, duration)
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복 (투명 ↔ 불투명)
            .SetEase(Ease.InOutSine);    // 부드러운 전환 효과
    }
}
