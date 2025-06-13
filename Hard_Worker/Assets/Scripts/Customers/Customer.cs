using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Customer : MonoBehaviour
{
    EnemyManager enemyManager;
    public CustomerData customerData; // 손님 데이터
    private int isServed; // 받은 음식의 개수

    Image foodImage; // 음식 이미지
    public float walkingTime; // 주문대까지 걸어가는 시간

    void OnEnable()
    {
        // 주문    
        StartCoroutine(MakeOrder());
        enemyManager.enemys[isServed].GetComponent<Enemy>().completedCooking += AllOrdersCompleted;
    }
    //  초기화
    public void Init(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }
    public IEnumerator MakeOrder()
    {
        // 요청 음식 이미지 생성
        foodImage.sprite = Instantiate(customerData.Foods[isServed].FoodImage, GameObject.Find("Canvas").transform);
        foodImage.transform.position = new Vector3(-200f,200f);

        yield return new WaitForSeconds(walkingTime);
      
        // 적 생성
        enemyManager.SpawnEnemy();
    }
    public void AllOrdersCompleted()
    {
        // 음식을 전부 다 받았다면
        if(isServed >= customerData.Foods.Count)
        {
           // 보상 지급

            // 삭제
            Destroy(this);
        }
    }
    public int Reward( )
    {
        return customerData.Reward;
    }
}
