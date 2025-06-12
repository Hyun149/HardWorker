using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemyCount = 0; // 스테이지 당 죽인 적의 개
    public GameObject[] enemys = new GameObject[3];
    public Vector3 pos = new Vector3 (0, 1f, 0);
    
    public int EnemyCount => enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        // 적 생성
        SpawnEnemy();
    }
    // 적 생성
    public void SpawnEnemy()
    {
        // 적 생성
        Enemy enemy = Instantiate(enemys[enemyCount], pos, Quaternion.identity).GetComponent<Enemy>();
        enemy.Init(this.gameObject.GetComponent<EnemyManager>());
    }
    // 현재 스테이지에서 죽인 적 개수 증가
    public void UpEnemyCount()
    {
        enemyCount++;
    }
}
