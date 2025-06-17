using System;
using UnityEngine;

/// <summary>
/// 손님 Pool을 관리하는 스크립트 입니다.
/// </summary>
public class CustomerPool : MonoBehaviour, IPoolable
{
    private Action<GameObject> returnToPool;
    Customer customer;
    SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        customer= GetComponent<Customer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Init(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
        spriteRenderer.flipX = true;
        customer.isOrderComplete = false;

    }

    public void OnSpawn()
    {


    }
    public void OnDespawn()
    {
        // 손님을 큐에서 해제

        returnToPool?.Invoke(gameObject); // 풀로 반환
    }
}
