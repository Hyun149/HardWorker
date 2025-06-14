using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEventTest : MonoBehaviour
{
    EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
       // enemyManager.enemy.enemyData.SetProgress(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (enemyManager.enemy == null) { return; }
            // 마우스 왼쪽 버튼을 눌렀을 때 수행할 작업
            enemyManager.enemy.TakeDamage(10);
            enemyManager.DamageText(10);
        }
    }
}
