using System;
using UnityEngine;

/// <summary>
/// 손님 오브젝트의 풀링 처리를 담당하는 클래스입니다.
/// - IPoolable 인터페이스를 구현하여 풀 입출 처리 및 초기화 기능을 제공합니다.
/// </summary>
public class CustomerPool : MonoBehaviour, IPoolable
{
    private Action<GameObject> returnToPool;
    private Customer customer;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 오브젝트가 활성화될 때 필요한 컴포넌트를 캐싱합니다.
    /// </summary>
    void OnEnable()
    {
        customer= GetComponent<Customer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 풀에서 꺼내질 때 실행되는 초기화 메서드입니다.
    /// </summary>
    /// <param name="returnAction">반환용 콜백 (풀에 다시 넣는 함수)</param>
    public void Init(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
        spriteRenderer.flipX = true;
        customer.isOrderComplete = false;

    }

    /// <summary>
    /// 풀에서 꺼내졌을 때 호출됩니다. (현재 비어 있음)
    /// </summary>
    public void OnSpawn()
    {
        // 필요 시 손님 등장 시점 처리 가능
    }

    /// <summary>
    /// 풀로 반환될 때 호출됩니다.
    /// </summary>
    public void OnDespawn()
    {
        // 손님을 큐에서 해제

        returnToPool?.Invoke(gameObject); // 풀로 반환
    }
}
