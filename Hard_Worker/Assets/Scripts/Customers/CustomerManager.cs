using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 손님 관리하는 스크립트입니다.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    CustomerUI customerUI;
    LineController lineController;

    public List<GameObject> customerPrefabs = new List<GameObject>(); // 손님 프리팹
   [HideInInspector] public Customer customer; // 지금 주문 중인 손님
    public Customer curCustomer;
    public List<FoodData> foods = new List<FoodData>();
    public FoodData food;

    public Vector3 pos = new Vector3(15, -0.81f, 0); // 손님 생성 위치

    void Awake()
    {
        //  lineController = FindObjectOfType<LineController>();
        lineController = GetComponentInParent<LineController>();
        customerUI = GetComponent<CustomerUI>();    
    }
    /// <summary>
    /// 세명의 손님을 생성합니다.
    /// </summary>
    public void SpawnCustomer()
    { 
        // 네명의 랜덤한 손님 프리팹 선택
        for (int i = 0; i < customerPrefabs.Count; i++)
        {     
            int index = Random.Range(0, customerPrefabs.Count);
            customer = Instantiate(customerPrefabs[i], pos, Quaternion.identity).GetComponent<Customer>();
            lineController.AddCustomer(customer);
        }
     
    }
    /// <summary>
    /// 새로운 손님을 줄에 추가합니다.
    /// </summary>
    /// <param name="index"></param>
    public void AddNewCustomer(int index)
    {
        int rand = Random.Range(0, customerPrefabs.Count);
        Vector2 spawnPos = lineController.lineStartPos.position + new Vector3(lineController.spacing * index, 0, 0);
       
        GameObject customer = Instantiate(customerPrefabs[rand], pos, Quaternion.identity);
        Customer _customer = customer.GetComponent<Customer>();

        _customer.targetPos = spawnPos; // 위치 지정
        lineController.customers.Enqueue(customer.GetComponent<Customer>());
    }
    /// <summary>
    /// 랜덤하게 요리를 주문합니다.
    /// </summary>
    public void RamdomOrder()
    {
        // 스테이지가 올라갈 수록 난이도가 높은 음식이 나오게
        List<FoodData> probabilityFoods = new List<FoodData>();

        foreach (var food in foods)
        {
            int stage = StageManager.Instance.Stage;
           
            if (food.Difficulty <= stage + 1)
            {
                // 난이도가 높은 음식일 수록 더 적게 나오게
                int weight = Mathf.Clamp((stage + 1) - food.Difficulty + 1, 1, 10);// 최대치 제한
                                                                                          
                for (int i = 0; i < weight; i++)
                {
                    probabilityFoods.Add(food);
                }
            }
        }
        // 음식이 없다면 전체 중에서 랜덤
        if (probabilityFoods.Count == 0)
        {
            probabilityFoods.AddRange(foods);
        }

        int value = Random.Range(0, probabilityFoods.Count);
        food = probabilityFoods[value];
        // 음식 이미지 표시
        customerUI.ShowOrderImage(food);
    } 
}
