
using UnityEngine;

public class ClickEventTest : MonoBehaviour
{
    DamageTextPool damageTextPool;
    EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        damageTextPool = GetComponent<DamageTextPool>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (enemyManager.enemy == null) { return; }
            
            enemyManager.enemy.TakeDamage(10); // 마우스 왼쪽 버튼을 눌렀을 때 수행할 작업
            DamageText(10);// 여기서 공격력 바꿔주시면 됩니다.
        }
    }
    /// <summary>
    /// 공격 데미지 텍스트를 출력합니다. 
    /// </summary>
    /// <param name="_attackDamage"></param>
    void DamageText(float _attackDamage)
    {
        GameObject obj = damageTextPool.GetObject(0); // Pool에서 가져오기
        if (obj == null) { return; }

        DamageText damageText = obj.GetComponent<DamageText>();
        damageText.attackDamage = _attackDamage; 
        damageText.ShowText(); // 텍스트 출력
        damageText.Init(obj => damageTextPool.ReturnObject(0, obj)); // 초기화
    }
}
