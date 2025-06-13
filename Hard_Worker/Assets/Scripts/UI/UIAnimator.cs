using UnityEngine;
using DG.Tweening;

/// <summary>
/// UI 팝업의 등장/퇴장 애니메이션을 담당하는 범용 유틸리티 클래스입니다.
/// - DOTween 기반으로 크기 + 투명도 애니메이션을 제공합니다.
/// - 모든 팝업 UI에 재사용 가능합니다.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIAnimator : MonoBehaviour
{
    [Header("Animation Setting")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float startScale = 0.5f;
    [SerializeField] private float punchScale = 1.1f;

    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    private Tween currentTween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// UI를 활성화하고 애니메이션을 재생합니다.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        currentTween?.Kill();

        transform.localScale = originalScale * startScale;
        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, duration * 0.5f));
        seq.Join(transform.DOScale(originalScale * punchScale, duration * 0.6f).SetEase(Ease.OutBack));
        seq.Append(transform.DOScale(originalScale, duration * 0.2f).SetEase(Ease.InOutSine));

        currentTween = seq;
    }

    /// <summary>
    /// UI를 비활성화하는 애니메이션을 재생합니다.
    /// </summary>
    public void Hide()
    {
        currentTween?.Kill();

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(0f, duration * 0.3f));
        seq.Join(transform.DOScale(originalScale * startScale, duration * 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() => gameObject.SetActive(false));

        currentTween = seq;
    }
}
