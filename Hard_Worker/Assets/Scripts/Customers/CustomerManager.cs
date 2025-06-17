using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 손님을 생성하고, 주문 및 반응 애니메이션을 관리하는 클래스입니다.
/// - 손님 생성/추가, 음식 서빙 반응, 오브젝트 풀 관리 기능을 포함합니다.
/// </summary>
public class CustomerManager : MonoBehaviour
{
    public List<FoodData> foods = new List<FoodData>(); // 요리 데이터들
    public FoodData food;
    public GameObject foodIconPrefab;
    public Customer curCustomer; // 지금 주문 중인 손님
    public CustomerUI customerUI;
    
    private CustomerPoolManager poolManager;
    private LineController lineController;

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
            curCustomer = GetPoolCustomer();// Pool에서 손님 한명을 꺼내옴                                
            lineController.AddCustomer(curCustomer);
        }
    }
    /// <summary>
    /// 새로운 손님을 줄에 추가합니다.
    /// </summary>
    /// <param name="index"></param>
    public void AddNewCustomer(int index)
    {
        Vector2 spawnPos = lineController.lineStartPos.position + new Vector3(lineController.spacing * index, 0, 0);

        curCustomer = GetPoolCustomer(); // Pool에서 손님 한명을 꺼내옴

        curCustomer.targetPos = spawnPos;
        lineController.AddCustomer(curCustomer);
    }
    /// <summary>
    /// Pool에서 손님 한명을 꺼내옵니다.
    /// </summary>
    Customer GetPoolCustomer()
    {
        var customer = poolManager.GetObject(0).GetComponent<Customer>(); // Pool에서 까내온 뒤
        customer.GetComponent<CustomerPool>().Init(customer => poolManager.ReturnObject(0, customer)); // 초기화 합니다.
        return customer;
    }
    /// <summary>
    /// 요리 완료시 호출되는 애니메이션 : 손님만족 이모션포함 
    /// </summary>
    /// <param name="customer">현재 손님 객체</param>
    public void ServeFoodAnimation(Customer customer)
    {
        GameObject icon = Instantiate(foodIconPrefab, transform.position, Quaternion.identity);
        icon.transform.localScale = Vector3.one * 1.5f;

        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        if (sr != null && customer.foodData != null)
            sr.sprite = customer.foodData.FoodImage;

        icon.transform
            .DOMove(customer.transform.position, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Destroy(icon);
                customerUI.ShowServeReaction();// 서빙시 손님 반응
                SFXManager.Instance.Play(SFXType.HappySound);// 효과음
            });

        icon.transform.DOScale(Vector3.one * 0.5f, 1f);
        icon.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);
       
        // 손님 퇴장
        curCustomer.GetComponent<CustomerController>().CompleteOrder(true);
    }
}
