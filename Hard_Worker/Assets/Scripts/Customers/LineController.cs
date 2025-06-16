using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 손님들 대기줄을 관리하는 스크립트입니다.
/// </summary>
public class LineController : MonoBehaviour
{
    CustomerManager customerManager;
    public Transform lineStartPos; // 줄의 시작 위치
    public float spacing = 3f; // 손님 간 간격

    public Queue<Customer> customers = new Queue<Customer>(); // 줄을 선 손님들 담을 Queue

    void Awake()
    {
        customerManager = GetComponentInChildren<CustomerManager>();
    }
    /// <summary>
    /// 줄을 생성합니다.
    /// </summary>
    public void CreateLine()
    {
        int i = 0;
        foreach (var customer in customers) 
        {
            Debug.Log($"목표 위치:{customer}");
            Vector3 targetPos = lineStartPos.position + new Vector3(spacing * i, 0, 0);
            Debug.Log($"{targetPos}");
            StartCoroutine(customer.MoveCoroutine(targetPos));
            i++;
        }    
    }
    public void AddCustomer(Customer customer)
    {
        customers.Enqueue(customer);
    }

    public void RemoveCustomer(Customer customer)
    {
        customers.Dequeue();
    }
    /// <summary>
    /// 줄세우기 관리 하는 부분입니다.
    /// </summary>
    /// <returns></returns>
   public IEnumerator HandleOrder()
    {
        while (customers.Count > 0)
        {
            customerManager.curCustomer = customers.Dequeue();
            Customer customer = customerManager.curCustomer;

            // 주문하기
            yield return StartCoroutine(customer.MakeOrder(lineStartPos.position));

            // 나머지 손님 이동
            int i = 0;
            foreach (var _customer in customers)
            {
                Vector2 newPos = lineStartPos.position + new Vector3(spacing * i, 0, 0);
                yield return StartCoroutine(_customer.MoveCoroutine(newPos));
                i++;
            }
        }
       
    }
  
}
