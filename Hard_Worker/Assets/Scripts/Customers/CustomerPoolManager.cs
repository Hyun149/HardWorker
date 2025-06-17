using System.Linq;
using UnityEngine;

/// <summary>
/// 손님 전용 오브젝트 풀을 관리하는 클래스입니다.
/// - 손님 오브젝트를 효율적으로 재사용하기 위해 ObjectPoolManager를 상속받아 확장합니다.
/// - 손님 생성 시 필요한 의존성(Manager들)도 함께 주입합니다.
/// </summary>
public class CustomerPoolManager : ObjectPoolManager
{
    CustomerManager customerManager;
    FoodSelector foodSelector;
    LineController lineController;
    EnemyManager enemyManager;

    public Vector3 pos = new Vector3(15, -0.81f, 0); // 손님 생성 위치

    /// <summary>
    /// 초기화 시 필요한 매니저 컴포넌트를 캐싱합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // 부모의 Awake 실행되도록

        customerManager = GetComponent<CustomerManager>();
        foodSelector = GetComponent<FoodSelector>();
        lineController = GetComponentInParent<LineController>();
        enemyManager = transform.parent?.GetComponentsInChildren<EnemyManager>(true).FirstOrDefault(m => m.gameObject != gameObject);
    }

    /// <summary>
    /// 오브젝트 풀에서 손님 오브젝트를 가져옵니다.
    /// - 풀에 남은 객체가 있으면 재사용하고, 없으면 새로 생성합니다.
    /// - 새로 생성될 경우 각종 매니저 의존성을 주입하고 초기화도 수행합니다.
    /// </summary>
    /// <param name="prefabIndex">사용할 프리팹 인덱스</param>
    /// <returns>활성화된 손님 GameObject</returns>
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
    /// 오브젝트를 풀에 반환합니다.
    /// </summary>
    /// <param name="prefabIndex">반환 대상의 프리팹 인덱스</param>
    /// <param name="obj">반환할 GameObject</param>
    public override void ReturnObject(int prefabIndex, GameObject obj)
    {
        base.ReturnObject(prefabIndex, obj);
    }
}
