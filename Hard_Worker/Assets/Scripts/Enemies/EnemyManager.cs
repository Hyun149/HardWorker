using UnityEngine;

/// <summary>
/// 적 생성을 관리하는 스크립트입니다.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public Enemy enemy; // 현재 잡고있는 적
  
    [SerializeField] private Vector3 pos = new Vector3 (0.3f, -3f, 0); // 적이 생성될 위치

    private CustomerManager customerManager;
    private EnemyProgress enemyProgress;

    private int enemyCount = 0; // 스테이지 당 죽인 적의 개수
    public int EnemyCount => enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        enemyProgress = GetComponent<EnemyProgress>();
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
        enemy.Init(customerManager,this, enemyProgress);
        FindObjectOfType<ClickEventHandler>()?.StartAutoAttack();
    }

    /// <summary>
    /// 죽인 적 개수를 증가시킵니다.
    /// </summary>
    public void UpEnemyCount(int _enemyCount)
    {
        enemyCount += _enemyCount;
    }
}
