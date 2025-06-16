using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 재료 공격 데미지 텍스트 Pool 스크립트입니다.
/// 텍스트를 생성하고 Pool을 관리합니다.
/// </summary>
public class DamageTextPool : ObjectPoolManager
{
    EnemyManager enemyManager;

    protected override void Awake()
    {
        base.Awake(); // 부모의 Awake 실행되도록

        enemyManager = GetComponentInChildren<EnemyManager>();
    }
    /// <summary>
    /// Pool에서 가져오는 기능입니다.
    /// </summary>
    /// <returns></returns>
    public override GameObject GetObject(int prefabIndex)
    {
        // 적이 생성되고 Spawn 될 수 있게끔 설정
        if (enemyManager.enemy.gameObject.activeSelf == false
            || enemyManager.enemy.isAttackEnd == true)
        {
            return null;
        }
        return base.GetObject(prefabIndex);
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
