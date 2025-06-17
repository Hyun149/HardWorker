using UnityEngine;
using DG.Tweening;

/// <summary>
/// UI 팝업의 등장/퇴장 애니메이션을 담당하는 범용 유틸리티 클래스입니다.
/// - DOTween 기반으로 크기 및 투명도 애니메이션을 제공합니다.
/// - CanvasGroup과 Transform을 활용하여 UI 팝업을 부드럽게 제어합니다.
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

    /// <summary>
    /// 컴포넌트 초기화 시 CanvasGroup을 설정하고 초기 상태를 비활성화로 설정합니다.
    /// </summary>
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// UI를 팝업 애니메이션과 함께 활성화합니다.
    /// - 투명도 증가
    /// - 크기 확대 후 원래 크기로 축소
    /// - 인터랙션 및 클릭 감지 활성화
    /// </summary>
    public void Show()
    {
        currentTween?.Kill();

        transform.localScale = originalScale * startScale;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, duration * 0.5f));
        seq.Join(transform.DOScale(originalScale * punchScale, duration * 0.6f).SetEase(Ease.OutBack));
        seq.Append(transform.DOScale(originalScale, duration * 0.2f).SetEase(Ease.InOutSine));
        seq.OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });

        currentTween = seq;
    }

    /// <summary>
    /// UI를 페이드아웃 애니메이션과 함께 비활성화합니다.
    /// - 투명도 감소
    /// - 크기 축소
    /// - 인터랙션 및 클릭 감지 비활성화
    /// </summary>
    public void Hide()
    {
        currentTween?.Kill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(0f, duration * 0.3f));
        seq.Join(transform.DOScale(originalScale * startScale, duration * 0.3f).SetEase(Ease.InBack));

        currentTween = seq;
    }
}
