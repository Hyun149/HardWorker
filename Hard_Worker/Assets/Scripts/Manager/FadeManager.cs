using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// DOTween을 활용한 페이드 인/아웃 전환을 담당하는 매니저입니다.
/// - SceneLoader와 연동하여 부드러운 화면 전환을 제공합니다.
/// </summary>
public class FadeManager : MonoSingleton<FadeManager>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        fadeImage.raycastTarget = false;
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// 화면을 어둡게 덮는 페이드 아웃을 수행합니다.
    /// </summary>
    /// <param name="onComplete">완료 후 콜백</param>
    public void FadeOut(System.Action onComplete = null)
    {
        fadeImage.raycastTarget = true;
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 어두운 화면을 서서히 밝히는 페이드 인을 수행합니다.
    /// </summary>
    /// <param name="onComplete">완료 후 콜백</param>
    public void FadeIn(System.Action onComplete = null)
    {
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            fadeImage.raycastTarget = false;
            onComplete?.Invoke();
        });
    }
}
