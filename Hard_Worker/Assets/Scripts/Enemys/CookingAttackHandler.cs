using UnityEngine;

/// <summary>
/// 플레이어의 클릭 공격 및 자동 공격(보조 셰프 공격)을 처리하는 클래스입니다.
/// - 좌클릭으로 손질 공격을 수행합니다.
/// - 자동 공격(보조 셰프)은 외부에서 호출됩니다.
/// - 데미지 계산 및 크리티컬 확률 처리
/// - 데미지 텍스트 표시 처리
/// </summary>
public class CookingAttackHandler : MonoBehaviour
{
    [Header("의존성")]
    [SerializeField] private PlayerStat playerstat;
    [SerializeField] private DamageTextPool damageTextPool;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemyProgress enemyProgress;

    [Header("무기 스킬 관련")]
    [SerializeField] private WeaponManager weaponManager;
    private Weapon weapon; // 현재 무기 데이터
    private IWeaponSkill currentSkill;
    public int clickCount;
    /// <summary>
    /// Awake : 무기 착용 이벤트 구독
    /// </summary>
    void Awake()
    {
        weaponManager.OnWeaponEquipped += OnWeaponEquipped;
    }
    /// <summary>
    /// 의존성 컴포넌트 초기화 (만약 에디터에서 할당되지 않은 경우)
    /// </summary>
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyProgress = GetComponent<EnemyProgress>();
        damageTextPool = GetComponent<DamageTextPool>();
    }
    /// <summary>
    /// 무기 착용 이벤트 발생시 실행
    /// </summary>
    private void OnWeaponEquipped()
    {
        weapon = weaponManager.GetEquippedWeapon();

        if (weapon == null || weapon.data == null)
        {
            Debug.LogError("무기 또는 무기 데이터가 없습니다.");
            return;
        }
        currentSkill = WeaponSkillFactory.GetSkill(weapon.data.skillType);

        if (currentSkill == null)
        {
            Debug.LogWarning("currentSkill이 null입니다.");
        }
    }
    /// <summary>
    /// 마우스 클릭 시 플레이어의 기본 손질 공격을 수행합니다.
    /// - 적이 존재하지 않으면 아무 동작도 하지 않음
    /// - 손질 능력치 기반 데미지 계산 및 적용
    /// - 데미지 텍스트 출력
    /// - 스킬 발동
    /// </summary>
    public void TryPlayerAttack()
    {
        if (enemyManager.enemy == null 
            || enemyProgress.TargetProgress >= enemyProgress.MaxProgress)
        {
            return;
        }
        
        clickCount++;       //클릭 카운트
        float baseDamage = playerstat.GetFinalStatValue(StatType.Cut);
        float damage = CalculateDamage(baseDamage);

        enemyManager.enemy.TakeDamage(baseDamage);
        ShowDamageText(baseDamage);
        //스킬 발동
        currentSkill?.Activate(enemyManager.enemy, clickCount,baseDamage,ShowDamageText);
    }

    /// <summary>
    /// 보조 셰프 자동 공격 시 호출되는 메서드입니다.
    /// - 어시스트 능력치 기반 공격
    /// - 적이 없는 경우 경고 로그 출력
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
       
        ShowDamageText(damage);
        enemyManager.enemy.TakeDamage(damage);
      
    }

    /// <summary>
    /// 크리티컬 확률과 보너스를 반영하여 실제 데미지를 계산합니다.
    /// </summary>
    /// <param name="baseDamage">기본 능력치로부터 계산된 베이스 데미지</param>
    /// <returns>최종 계산된 데미지 (크리티컬 반영)</returns>
    public float CalculateDamage(float baseDamage)
    {
        float critChance = playerstat.GetFinalStatValue(StatType.CritChance) / 100f;
        float critBonus = playerstat.GetFinalStatValue(StatType.CritBonus);

        bool isCritical = Random.value < Mathf.Clamp01(critChance);
        Debug.Log($"[AutoAttack] 크확: {critChance}, 크리떴냐: {isCritical}, 크뎀: {critBonus}");

        return isCritical ? baseDamage * (1f + critBonus) : baseDamage;
    }

    /// <summary>
    /// 데미지 텍스트 오브젝트를 풀에서 꺼내 화면에 표시합니다.
    /// - 공격 데미지를 숫자로 띄워줍니다.
    /// </summary>
    /// <param name="damage">표시할 데미지 값</param>
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
