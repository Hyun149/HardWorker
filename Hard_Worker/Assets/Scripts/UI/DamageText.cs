using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 재료 공격 데미지 표시 스크립트입니다.
/// 활성화시 위로 올라가는 애니메이션을 한 뒤 비활성화됩니다.
/// </summary>
public class DamageText : MonoBehaviour
{
    public TMP_Text damageText;
    public RectTransform rect;
    private Vector2 startPos;
    public float distance = 15f; // 위로 이동할 거리
    public float duration = 1.5f; // 사라지는데 걸리는 시간
    float speed = 100f;

    void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// 초기화 스크립트 입니다.
    /// 위치와 알파값을 초기화합니다.
    /// </summary>
    void Init()
    {
        startPos = rect.anchoredPosition;
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1f);
    }

    /// <summary>
    /// 위로 올라가는 애니메이션을 한 뒤 비활성화됩니다.
    /// </summary>
    public void FadeOutAndMove(int value, Action onComplete)
    {    
        damageText.DOKill();
        rect.DOKill();

        damageText.text = value.ToString();

        // 스피드 설정
        float duration = distance / speed;

        // x축 랜덤 설정
        float randomX = UnityEngine.Random.Range(startPos.x - 100f, startPos.x + 100);
        Vector2 randomPos = new Vector2(startPos.x + randomX, startPos.y);
        rect.anchoredPosition = randomPos;

        // 위로 올라가기
        Vector2 target = randomPos + Vector2.up * distance;
        rect.DOAnchorPos(target, duration);

        // 텍스트 비활성화
        damageText.DOFade(0f, duration)
        .OnComplete(() =>
        {
            // 애니메이션 끝난 뒤에 원래 위치로 되돌림
            rect.anchoredPosition = startPos;
            gameObject.SetActive(false);

            onComplete?.Invoke();
        });
    }
}
