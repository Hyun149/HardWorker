using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 손님의 Pool을 관리하는 스크립트 입니다.
/// </summary>
public class CustomerPoolManager : ObjectPoolManager
{
    CustomerManager customerManager;
    FoodSelector foodSelector;
    LineController lineController;
    EnemyManager enemyManager;

    public Vector3 pos = new Vector3(15, -0.81f, 0); // 손님 생성 위치

    protected override void Awake()
    {
        base.Awake(); // 부모의 Awake 실행되도록

        customerManager = GetComponent<CustomerManager>();
        foodSelector = GetComponent<FoodSelector>();
        lineController = GetComponentInParent<LineController>();
        enemyManager = transform.parent?.GetComponentsInChildren<EnemyManager>(true).FirstOrDefault(m => m.gameObject != gameObject);
    }
    /// <summary>
    /// Pool에서 가져오는 기능입니다.
    /// </summary>
    /// <returns></returns>
    public override GameObject GetObject(int prefabIndex)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.LogError($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject obj;
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else
        {
            prefabIndex = UnityEngine.Random.Range(0, prefabs.Length);
            obj = Instantiate(prefabs[prefabIndex], pos, Quaternion.identity);
            obj.GetComponent<CustomerController>().Init(customerManager, foodSelector, lineController, enemyManager);
            obj.GetComponent<IPoolable>()?.Init(o => ReturnObject(prefabIndex, o));

        }
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }
    /// <summary>
    ///  Pool에서 반납하는 기능입니다.
    /// </summary>
    /// <returns></returns>
    public override void ReturnObject(int prefabIndex, GameObject obj)
    {
        base.ReturnObject(prefabIndex, obj);
    }
}
