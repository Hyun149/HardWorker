using System.Collections;
using UnityEngine;

/// <summary>
/// 손님의 이동 및 주문 과정을 제어하는 클래스입니다.
/// - 이동, 애니메이션, 주문 처리, 퇴장까지 전반적인 흐름을 관리합니다.
/// </summary>
public class CustomerController : MonoBehaviour
{
    [SerializeField] private float walkingTime; // 주문대까지 걸어가는 시간
    [SerializeField] private bool isArrived = false; // 계산대에 도착했는지 여부
    
    private Customer customer;
    private CustomerManager customerManager;
    private FoodSelector foodSelector;
    private CustomerAni customerAni;
    private LineController lineController;
    private EnemyManager enemyManager;
    private SpriteRenderer spriteRenderer;
    private CustomerPool pool;
    private Coroutine moveCoroutine;

    /// <summary>
    /// 활성화될 때 필요한 컴포넌트를 캐싱합니다.
    /// </summary>
    private void OnEnable()
    {
        customer = GetComponent<Customer>();
        customerAni = GetComponent<CustomerAni>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pool = GetComponent<CustomerPool>();
    }

    /// <summary>
    ///  초기화 합니다.
    /// </summary>
    /// <param name="customerManager"></param>
    /// <param name="foodSelector"></param>
    /// <param name="lineController"></param>
    /// <param name="enemyManager"></param>
    public void Init(CustomerManager customerManager, FoodSelector foodSelector,
                      LineController lineController, EnemyManager enemyManager)
    {
        this.customerManager = customerManager;
        this.foodSelector = foodSelector;
        this.lineController = lineController;
        this.enemyManager = enemyManager;
    }

    /// <summary>
    /// 주문 상태를 변경합니다.
    /// </summary>
    /// <param name="value"></param>
    public void CompleteOrder(bool value)
    {
        customer.isOrderComplete = value;
    }
    /// <summary>
    ///  재사용시 코루틴의 중복 사용을 막기 위한 부분입니다.
    /// </summary>
    /// <returns></returns>
    public void StartMove(Vector2 targetPos)
    {
        // 이전 코루틴이 실행 중이면 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // 새 이동 시작
        moveCoroutine = StartCoroutine(MoveRoutine(targetPos));
    }
    /// <summary>
    ///  손님을 목표 위치까지 걷게끔 하는 부분입니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveRoutine(Vector2 targetPos)
    {
        isArrived = false;
        customerAni.Walk();

        while (Vector2.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, customer.moveSpeed * Time.deltaTime);
            isArrived = true;
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
        StartMove(targetPos);
        yield return new WaitUntil(() => isArrived == true);
        foodSelector.RamdomOrder(); // 랜덤 주문
        enemyManager.SpawnEnemy();  // 적 생성 

        // 주문 완료까지 기다림
        yield return new WaitUntil(() => customer.isOrderComplete == true);

        yield return new WaitForSeconds(2F);
        // 주문 완료시 걷기 이동
        StartCoroutine(Exit());
    }
    /// <summary>
    /// 주문을 완료하고 밖으로 나가는 스크립트 입니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Exit()
    {
        customer.targetPos = customer.exitPos;
        spriteRenderer.flipX = false;

        // 밖으로 나가기
        StartMove(customer.targetPos);

        // 새로운 손님 생성
        customerManager.AddNewCustomer(lineController.customers.Count);
        yield return new WaitForSeconds(walkingTime);

        // Pool에 반환
        pool.OnDespawn();
    }
}
