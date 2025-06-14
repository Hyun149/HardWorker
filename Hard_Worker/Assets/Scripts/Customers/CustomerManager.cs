using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 손님 관리하는 스크립트입니다.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    CustomerUI customerUI;

    public List<GameObject> customers = new List<GameObject>();
   [HideInInspector] public Customer customer;

    public List<FoodData> foods = new List<FoodData>();
    public FoodData food;

    public Vector3 pos = new Vector3(3, 3, 0); // 손님 생성 위치

    void Awake()
    {
        customerUI = GetComponent<CustomerUI>();    
    }
    /// <summary>
    /// 손님을 생성합니다.
    /// </summary>
    public void SpawnCustomer()
    {
        // 랜덤하게 손님을 생성합니다.
        int index = Random.Range(0, customers.Count);
        customer = Instantiate(customers[index], pos, Quaternion.identity).GetComponent<Customer>(); ;       
    }
    /// <summary>
    /// 랜덤하게 요리를 주문합니다.
    /// </summary>
    public void RamdomOrder()
    {
         int value = Random.Range(0, foods.Count);

         // 랜덤 음식 생성
         food = foods[value];

        // 음식 이미지 표시
        customerUI.ShowOrderImage(food);
    } 
}
