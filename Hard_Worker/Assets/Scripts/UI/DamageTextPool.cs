using UnityEngine;

/// <summary>
/// 재료 공격 데미지 텍스트 Pool 스크립트입니다.
/// 텍스트를 생성하고 Pool을 관리합니다.
/// </summary>
public class DamageTextPool : ObjectPoolManager
{
    private EnemyManager enemyManager;
    private EnemyProgress enemyProgress;

    /// <summary>
    /// 풀 매니저 초기화 시 호출됩니다.
    /// EnemyManager와 EnemyProgress를 찾아 초기화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // 부모의 Awake 실행되도록
        enemyProgress = GetComponentInChildren<EnemyProgress>();
        enemyManager = GetComponentInChildren<EnemyManager>();
    }

    /// <summary>
    /// 풀에서 텍스트 오브젝트를 가져옵니다.
    /// 단, 적이 활성화된 상태일 때만 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="prefabIndex">풀에서 가져올 프리팹 인덱스</param>
    /// <returns>사용 가능한 오브젝트 또는 null</returns>
    public override GameObject GetObject(int prefabIndex)
    {
        // 적이 생성되고 Spawn 될 수 있게끔 설정
        if (enemyManager.enemy.gameObject.activeSelf == false )
        {
            return null;
        }
        return base.GetObject(prefabIndex);
    }

    /// <summary>
    /// 사용이 끝난 텍스트 오브젝트를 풀로 반환합니다.
    /// </summary>
    /// <param name="prefabIndex">프리팹 인덱스</param>
    /// <param name="obj">반환할 오브젝트</param>
    public override void ReturnObject(int prefabIndex, GameObject obj)
    {
      base.ReturnObject(prefabIndex, obj);
    }
}
