using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 재료 공격 데미지를 표시하는 텍스트 UI입니다.
/// - 텍스트가 위로 떠오르며 점점 사라지는 연출을 보여줍니다.
/// - 연출이 끝나면 오브젝트 풀로 반환됩니다.
/// </summary>
public class DamageText : MonoBehaviour, IPoolable
{
    public float attackDamage;

    [SerializeField] private float duration = 1.5f; // 사라지는데 걸리는 시간
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector2 startPos = new Vector3(0f, -3f, 0); // 적이 생성될 위치
    [SerializeField] private float distance = 15f; // 위로 이동할 거리

    private Action<GameObject> returnToPool;
    private GameObject damageTexts;
    private float speed = 100f;

    /// <summary>
    /// 오브젝트를 풀에서 꺼낼 때 호출되는 초기화 메서드입니다.
    /// 위치와 투명도를 초기화합니다.
    /// </summary>
    /// <param name="returnAction">풀로 반환할 때 호출될 액션입니다.</param>
    public void Init(Action<GameObject> returnAction)
    {
        damageTexts = GameObject.Find("DamageTexts");
        transform.SetParent(damageTexts.transform);
        transform.localScale = Vector3.one;

        // 위치 초기화
        rect.anchoredPosition = startPos;
     
        // 투명도 초기화
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1f);

        returnToPool = returnAction;
    }

    /// <summary>
    /// 오브젝트가 활성화된 직후 호출됩니다.
    /// </summary>
    public void OnSpawn()
    {
        // 가장 위로 가게 설정
        damageTexts.transform.SetAsLastSibling();
        // 위로 올라가는 애니메이션 후, Pool에 return
        FadeOutAndMove(attackDamage, () =>
        {
            // 풀로 반환
            OnDespawn();
        });
    }

    /// <summary>
    /// 오브젝트를 풀로 반환합니다.
    /// </summary>
    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject); // 풀로 반환
    }

    /// <summary>
    /// 텍스트를 위로 띄우고 점점 사라지게 한 후, 완료되면 콜백을 실행합니다.
    /// </summary>
    /// <param name="value">표시할 데미지 수치</param>
    /// <param name="onComplete">애니메이션 종료 후 실행할 콜백</param>
    public void FadeOutAndMove(float value, Action onComplete)
    {
        if (!gameObject.activeInHierarchy) { return; }

        damageText.text = value.ToString();

        // 애니메이션 지속 시간
        float duration = distance / speed;

        // x축 랜덤 설정
        float randomX = UnityEngine.Random.Range(startPos.x - 100f, startPos.x + 100);
        Vector2 randomPos = new Vector2(startPos.x + randomX, startPos.y);
        rect.anchoredPosition = randomPos;

        // 기존 트윈 제거
        damageText.DOKill();
        rect.DOKill();

        // 위로 올라가기
        Vector2 target = randomPos + Vector2.up * distance;
        rect.DOAnchorPos(target, duration).SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        // 텍스트 비활성화
        Tween tween= damageText.DOFade(0f, duration).SetLink(gameObject, LinkBehaviour.KillOnDestroy)
        .OnComplete(() =>
        {
            // 애니메이션 끝난 뒤에 원래 위치로 되돌림
            rect.anchoredPosition = startPos;

            // Pool에 return
            onComplete?.Invoke();
        });
       
    }

    /// <summary>
    /// 텍스트에 현재 데미지 수치를 표시합니다.
    /// </summary>
    public void ShowText()
    {
        damageText.text = attackDamage.ToString();
    }
}
