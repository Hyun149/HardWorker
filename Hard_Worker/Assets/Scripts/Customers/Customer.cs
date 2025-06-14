using System.Collections;
using UnityEngine;

/// <summary>
/// 손님 주문 스크립트입니다.
/// </summary>
public class Customer : MonoBehaviour
{
    EnemyManager enemyManager;
    public float walkingTime; // 주문대까지 걸어가는 시간

   /// <summary>
   /// 초기화시 주문합니다.
   /// </summary>
    public void Init()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
     
        // 주문    
        StartCoroutine(MakeOrder());
      
    }
    /// <summary>
    /// 주문시 적(음식 재료)을 생성하고 주문합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MakeOrder()
    {     
        yield return new WaitForSeconds(walkingTime);
      
        // 적 생성
        enemyManager.SpawnEnemy();
    }
}
