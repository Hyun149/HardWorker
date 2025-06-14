using UnityEngine;

/// <summary>
/// 적 생성을 관리하는 스크립트입니다.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    EnemyManager enemyManager;

    public Enemy enemy; // 현재 잡고있는 적
    private int enemyCount = 0; // 스테이지 당 죽인 적의 개수
  
    public Vector3 pos = new Vector3 (0.3f, -3f, 0); // 적이 생성될 위치

    private DamageTextPool damageTextPool;

    public int EnemyCount => enemyCount;
    CustomerManager customerManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        damageTextPool = GetComponent<DamageTextPool>();
        customerManager = FindObjectOfType<CustomerManager>();  
    }
    /// <summary>
    /// 적을 생성합니다.
    /// </summary>
    public void SpawnEnemy()
    {
        // 적 생성
        enemy = Instantiate(customerManager.food.Enemys[enemyCount], pos, Quaternion.identity).GetComponent<Enemy>();
    
        UpEnemyCount(1);

        // 적 초기화
        enemy.Init();

    }
    /// <summary>
    /// 죽인 적 개수를 증가시킵니다.
    /// </summary>
    /// <param name="_enemyCount"></param>
    public void UpEnemyCount(int _enemyCount)
    {
        enemyCount += _enemyCount;
    }
    /// <summary>
    /// 적 공격시 공격 데미지 텍스트를 활성화합니다.
    /// </summary>
    /// <param name="value"></param>
    public void DamageText(int value)
    {
        if (enemyManager.enemy.gameObject.activeSelf == false) { return; }

        DamageText dt = damageTextPool.Get();
        dt.gameObject.SetActive(true);
        damageTextPool.damageTexts.transform.SetAsLastSibling();
        dt.FadeOutAndMove(value, () =>
        {
            damageTextPool.Return(dt);
        });
    }
}
