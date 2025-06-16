using System.Collections;
using UnityEngine;

/// <summary>
/// 손님 주문 스크립트입니다.
/// </summary>
public class Customer : MonoBehaviour
{
    CustomerManager customerManager;
    CustomerAni customerAni;
    LineController lineController;
    EnemyManager enemyManager;
    SpriteRenderer spriteRenderer;
    CustomerPool pool;

    public Vector2 targetPos; // 목표 위치
    public Vector2 exitPos; // 퇴장 위치

    public float walkingTime; // 주문대까지 걸어가는 시간
    public float moveSpeed = 10f;
    public bool isOrderComplete = false; // 주문 완료 여부

    private Coroutine moveCoroutine;

    private void OnEnable()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        lineController = FindObjectOfType<LineController>();
        enemyManager = FindObjectOfType<EnemyManager>();
        customerAni = GetComponent<CustomerAni>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pool = GetComponent<CustomerPool>();
    }
    /// <summary>
    /// 주문 상태를 변경합니다.
    /// </summary>
    /// <param name="value"></param>
    public void CompleteOrder(bool value)
    {
        isOrderComplete = value;
    }
    /// <summary>
    ///  재사용시 코루틴의 중복 사용을 막기 위한 부분입니다.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        moveCoroutine = StartCoroutine(MoveRoutine(targetPos));
        yield return moveCoroutine;
        moveCoroutine = null;
    }
    /// <summary>
    ///  손님을 목표 위치까지 걷게끔 하는 부분입니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveRoutine(Vector2 targetPos)
    {
        customerAni.Walk();

        while (Vector2.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }

        customerAni.Idle();

        // 도착 후 정확히 위치 보정
        transform.position = targetPos;

    }
 
    /// <summary>
    /// 주문시 적(음식 재료)을 생성하고 주문합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MakeOrder(Vector2 targetPos)
    {
        yield return StartCoroutine(MoveCoroutine(targetPos));

        customerManager.RamdomOrder(); // 랜덤 주문
        enemyManager.SpawnEnemy();  // 적 생성 

        // 주문 완료까지 기다림
        yield return new WaitUntil(() => isOrderComplete == true);

        // 주문 완료시 걷기 이동
        StartCoroutine(Exit());

        // 새로운 손님 생성
        customerManager.AddNewCustomer(lineController.customers.Count);
    }
    /// <summary>
    /// 주문을 완료하고 밖으로 나가는 스크립트 입니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Exit()
    {
        targetPos = exitPos;
        spriteRenderer.flipX = false;

        // 밖으로 나가기
        StartCoroutine(MoveCoroutine(targetPos));

        yield return new WaitForSeconds(walkingTime);

        pool.OnDespawn();
       // Destroy(gameObject);
    }
}
