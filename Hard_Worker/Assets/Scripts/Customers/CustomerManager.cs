using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using DG.Tweening;

/// <summary>
/// 손님 관리하는 스크립트입니다.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    CustomerPoolManager poolManager;
    CustomerUI customerUI;
    LineController lineController;
    //public List<GameObject> customerPrefabs = new List<GameObject>(); // 손님 프리팹
   [HideInInspector] public GameObject customer; // 지금 주문 중인 손님
    public Customer curCustomer;
    public List<FoodData> foods = new List<FoodData>();
    public FoodData food;
    public GameObject foodIconPrefab;
    void Awake()
    {
        poolManager = GetComponent<CustomerPoolManager>();
        lineController = GetComponentInParent<LineController>();
        customerUI = GetComponent<CustomerUI>();    
    }
    /// <summary>
    /// 세명의 손님을 생성합니다.
    /// </summary>
    public void SpawnCustomer()
    { 
        // 네명의 랜덤한 손님 프리팹 선택
        for (int i = 0; i < poolManager.prefabs.Length; i++)
        {
            GetPoolCustomer();// Pool에서 손님 한명을 꺼내옴        
            lineController.AddCustomer(customer.GetComponent<Customer>());
        }
    }
    /// <summary>
    /// 새로운 손님을 줄에 추가합니다.
    /// </summary>
    /// <param name="index"></param>
    public void AddNewCustomer(int index)
    {
        Vector2 spawnPos = lineController.lineStartPos.position + new Vector3(lineController.spacing * index, 0, 0);

        GetPoolCustomer(); // Pool에서 손님 한명을 꺼내옴
        
        Customer _customer = customer.GetComponent<Customer>();
        _customer.targetPos = spawnPos; // 위치 지정

        lineController.AddCustomer(_customer);
    }
    /// <summary>
    /// Pool에서 손님 한명을 꺼내옵니다.
    /// </summary>
    void GetPoolCustomer()
    {
        customer = poolManager.GetObject(0); // Pool에서 까내온 뒤
        CustomerPool customerPool = customer.GetComponent<CustomerPool>();
        customerPool.Init(customer => poolManager.ReturnObject(0, customer)); // 초기화 합니다.
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
        curCustomer.foodData = food;
        // 음식 이미지 표시
        customerUI.ShowOrderImage(food);

        // 기본 골드 보상 설정
        StageManager.Instance.SetBaseReward(food.Difficulty * 100);

    } 
    /// <summary>
    /// 요리 완료시 호출되는 애니메이션 : 손님만족 이모션포함 
    /// </summary>
    /// <param name="customer">현재 손님 객체</param>
    public void ServeFoodAnimation(Customer customer)
    {
        GameObject icon = Instantiate(foodIconPrefab, transform.position + Vector3.up, Quaternion.identity);
        icon.transform.localScale = Vector3.one * 1.5f;

        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        if (sr != null && customer.foodData != null)
            sr.sprite = customer.foodData.FoodImage;

        icon.transform
            .DOMove(customer.transform.position + Vector3.up, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Destroy(icon);
                customerUI.ShowHappyReaction();// 만족 이모션
                SFXManager.Instance.Play(SFXType.HappySound);// 효과음
            });

        icon.transform.DOScale(Vector3.one * 0.5f, 1f);
        icon.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);
    }
}
