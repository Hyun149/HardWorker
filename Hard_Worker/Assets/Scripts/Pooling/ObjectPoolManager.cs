using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 여러 PoolManager들의 부모 스크립트 인비다.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    public GameObject[] prefabs; // 프리팹
    
    protected Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    protected virtual void Awake()
    {
        Instance = this;

        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }
    /// <summary>
    /// Pool에서 오브젝트를 가져옴
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <returns></returns>
    public virtual GameObject GetObject(int prefabIndex)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.LogError($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject obj;
        // 존재한다면
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        // 존재하지 않는다면
        else
        {
            obj = Instantiate(prefabs[prefabIndex]); // 생성
            obj.GetComponent<IPoolable>()?.Init(o => ReturnObject(prefabIndex, o)); // 초기화
        }     
        // 활성화
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }
    /// <summary>
    /// Pool에 반환함
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="obj"></param>
    public virtual void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(obj);
            return;
        }
        // 비활성화
        obj.SetActive(false);
        pools[prefabIndex].Enqueue(obj);
    }
}
