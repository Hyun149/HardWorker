using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    public EnemyData enemyData;
    public event Action<float,float> onChangeProgress;

    // 적 초기화
    public void Init(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }
    // 적 피격
    public void TakeDamage(float value)
    {
        // 진행도 증가
        float progress = enemyData.CurProgress;
        progress = Mathf.Min(progress + value,100);
        enemyData.SetProgress(progress);

        // 진행도 표시
        onChangeProgress?.Invoke(progress, enemyData.MaxProgress);

        // 진행 완료시 재료 지급
        if (progress >= 100) { Ingredient(); }
    }
    // 재료 지급
    void Ingredient()
    {
        // 이번 스테이지에서 죽인 적 카운트를 증가
        enemyManager.UpEnemyCount();

        // 재료를 지급

        // 다음 적 생성
        if (enemyManager.EnemyCount < enemyManager.enemys.Length - 1)
        {
            enemyManager.SpawnEnemy();
        }

        Destroy(this);
     }
}
