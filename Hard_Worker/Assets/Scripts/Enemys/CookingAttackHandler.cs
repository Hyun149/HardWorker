
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CookingAttackHandler : MonoBehaviour
{
    [Header("의존성")]
    [SerializeField] private PlayerStat playerstat;
    [SerializeField] private DamageTextPool damageTextPool;
    [SerializeField] private EnemyManager enemyManager;
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
            TryPlayerAttack();
        }
    }

    /// <summary>
    /// 기본 손질 (플레이어 클릭 공격) 처리
    /// </summary>
    public void TryPlayerAttack()
    {
        if (enemyManager.enemy == null)
        {
            return;
        }

        float baseDamage = playerstat.GetFinalStatValue(StatType.Cut);
        float damage = CalculateDamage(baseDamage);

        enemyManager.enemy.TakeDamage(baseDamage);
        ShowDamageText(baseDamage);
    }

    /// <summary>
    /// 자동 공격 (보조 셰프 손질) 호출용 메서드
    /// </summary>
    public void TryAutoAttack()
    {
        if (enemyManager.enemy == null || enemyManager.enemy == null)
        {
            Debug.LogWarning("자동 공격 싪패: enemyManager 혹은 enemy가 비어있음");
            return;
        }

        float baseDamage = playerstat.GetFinalStatValue(StatType.AssistSkill);
        float damage = CalculateDamage(baseDamage);

        enemyManager.enemy.TakeDamage(damage);
        ShowDamageText(damage);
    }

    /// <summary>
    /// 크리티컬 여부에 따라 데미지를 계산합니다.
    /// </summary>
    public float CalculateDamage(float baseDamage)
    {
        float critChance = playerstat.GetFinalStatValue(StatType.CritChance);
        float critBonus = playerstat.GetFinalStatValue(StatType.CritBonus);

        bool isCritical = Random.value < critChance;
        return isCritical ? baseDamage * (1f + critBonus) : baseDamage;
    }

    /// <summary>
    /// 데미지 텍스트 출력
    /// </summary>
    private void ShowDamageText(float damage)
    {
        GameObject obj = damageTextPool.GetObject(0);
        if (obj == null)
        {
            return;
        }

        DamageText damageText = obj.GetComponent<DamageText>();
        damageText.attackDamage = damage;
        damageText.ShowText();
        damageText.Init(obj => damageTextPool.ReturnObject(0, obj));
    }
}
