using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 손님들의 대기 줄을 관리하는 클래스입니다.
/// - 손님을 줄에 추가하고, 줄 정렬 및 주문 흐름을 제어합니다.
/// </summary>
public class LineController : MonoBehaviour
{
    private CustomerManager customerManager;

    public Transform lineStartPos; // 줄의 시작 위치
    public float spacing = 3f; // 손님 간 간격
    public Queue<Customer> customers = new Queue<Customer>(); // 줄을 선 손님들 담을 Queue

    /// <summary>
    /// 초기화 시 CustomerManager를 찾아 저장합니다.
    /// </summary>
    void Awake()
    {
        customerManager = GetComponentInChildren<CustomerManager>();
    }

    /// <summary>
    /// 줄에 선 손님들을 위치에 맞게 정렬합니다.
    /// </summary>
    public void CreateLine()
    {
        int i = 0;
        foreach (var customer in customers) 
        {
            Vector3 targetPos = lineStartPos.position + new Vector3(spacing * i, 0, 0);
            customer.GetComponent<CustomerController>().StartMove(targetPos);
            i++;
        }    
    }

    /// <summary>
    /// 줄에 손님을 추가합니다.
    /// </summary>
    /// <param name="customer">추가할 손님</param>
    public void AddCustomer(Customer customer)
    {
        customers.Enqueue(customer);
    }

    /// <summary>
    /// 줄 맨 앞의 손님을 제거합니다.  
    /// (첫 번째 손님만 제거 가능)
    /// </summary>
    /// <param name="customer">제거할 손님</param>
    public void RemoveCustomer(Customer customer)
    {
        if (customers.Count > 0 && customers.Peek() == customer)
        {
            customers.Dequeue();
        }
    }

    /// <summary>
    /// 줄에 선 손님들이 순서대로 주문하고,  
    /// 다음 손님들이 앞으로 이동하도록 처리합니다.
    /// </summary>
    /// <returns>주문 흐름 코루틴</returns>
    public IEnumerator HandleOrder()
    {
        while (customers.Count > 0)
        {
            customerManager.curCustomer = customers.Dequeue();
            Customer customer = customerManager.curCustomer;
            CustomerController controller = customer.GetComponent<CustomerController>(); 
            // 주문하기
            yield return StartCoroutine(controller.MakeOrder(lineStartPos.position));

            // 나머지 손님 이동
            List<Customer> tempList = new List<Customer>(customers);
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 newPos = lineStartPos.position + new Vector3(spacing * i, 0, 0);

                controller = tempList[i].GetComponent<CustomerController>();
               // yield return StartCoroutine(controller.MoveCoroutine(newPos));===============
                controller.StartMove(newPos);
                yield return null;
            }
        }
    }
}
