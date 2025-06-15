using UnityEngine;
using TMPro;
using DG.Tweening;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TextMeshProUGUI>();
        }

        StartBlinking();
    }

    private void StartBlinking()
    {
        targetText.DOFade(0f, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
