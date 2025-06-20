using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 클릭 기반 공격 및 자동 공격을 처리하는 클래스입니다.
/// - 클릭 또는 자동 공격 시 이펙트 및 사운드 출력
/// - 플레이어 스탯 기반 데미지 및 치명타 처리
/// - 플레이어 레벨 조건에 따라 자동 공격 해금
/// </summary>
public class ClickEventHandler : MonoBehaviour
{
    [SerializeField] private PlayerStat playerstat;
    [SerializeField] private CookingAttackHandler cookingAttackHandler;
    
    [Header("클릭 설정")]
    [SerializeField] private bool isPaused = false;

    [Header("플레이어 레벨")]
    [SerializeField] private int playerLevel;
    [SerializeField] private int autoAttackUnlockLevel; // 자동 공격 해금 레벨

    [Header("자동 공격 설정")]
    [SerializeField] private int autoAttackLevel;
    [SerializeField] private float baseAutoAttackInterval;
    [SerializeField] private bool isAutoAttackUnlocked = false;

    [Header("파티클 시스템")]
    [SerializeField] private ParticleSystem normalAttackParticle;
    [SerializeField] private ParticleSystem criticalAttackParticle;

    [Header("파티클 설정")] // 추가된 부분
    [SerializeField] private AttackParticleSettings particleSettings = new AttackParticleSettings();

    [Header("오디오")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip normalAttackSound;
    [SerializeField] private AudioClip criticalAttackSound;
    
    [Header("클릭 애니메이션 관련")]
    [SerializeField] private CursorManager cursorManager;
    
    private Camera mainCamera;
    private Coroutine autoAttackCoroutine;
    private AutoAttackManager autoAttackManager;

    /// <summary>
    /// 초기화: 메인 카메라 참조, 파티클 설정 적용, 자동 공격 상태 체크
    /// </summary>
    void Start()
    {
        mainCamera = Camera.main;
        autoAttackManager = FindObjectOfType<AutoAttackManager>();

        // 파티클 설정 적용 (추가된 부분)
        if (normalAttackParticle != null)
        {
            AttackParticleSettings.SetupNormalAttackParticle(normalAttackParticle, particleSettings);
        }
        if (criticalAttackParticle != null)
        {
            AttackParticleSettings.SetupCriticalAttackParticle(criticalAttackParticle, particleSettings);
        }

        // 플레이어 레벨 체크하여 자동 공격 해금
        CheckAutoAttackUnlock();
    }

    /// <summary>
    /// 클릭 입력 감지 및 처리
    /// </summary>
    void Update()
    {
        // 일시정지 상태면 클릭 처리하지 않음
        if (isPaused)
            return;

        // 마우스 클릭 또는 터치 처리
        if (Input.GetMouseButtonDown(0))
        {
            // UI 위에 있는지 확인
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // 모바일 터치 시 UI 위에 있는지 확인
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
            }

            PerformClick();
        }
    }

    /// <summary>
    /// 클릭 시 치명타 여부에 따라 공격 처리
    /// </summary>
    void PerformClick()
    {
        Vector3 clickPosition = GetClickWorldPosition();
        cursorManager?.PlayClickAnimation();

        float critChance = Mathf.Clamp(playerstat.GetStatValue(StatType.CritChance) * 0.01f, 0f, 0.95f);
        bool isCritical = Random.value < critChance;

        cookingAttackHandler.TryPlayerAttack(isCritical);

        float baseDamage = playerstat.GetStatValue(StatType.Cut);
        float critBonus = playerstat.GetStatValue(StatType.CritBonus);
        float finalDamage = isCritical ? baseDamage * (1f + critBonus) : baseDamage;

        if (isCritical)
        {
            OnCriticalAttack(clickPosition, finalDamage);
        }
        else
        {
            OnNormalAttack(clickPosition, finalDamage);
        }
    }

    /// <summary>
    /// 클릭한 위치를 월드 좌표로 변환합니다.
    /// </summary>
    Vector3 GetClickWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // 카메라로부터의 거리
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    /// <summary>
    /// 일반 공격 처리: 이펙트, 사운드 실행
    /// </summary>
    void OnNormalAttack(Vector3 position, float damage)
    {
        // 일반 공격 이펙트
        if (normalAttackParticle != null)
        {
            ParticleSystem particle = Instantiate(normalAttackParticle, position, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }

        // 일반 공격 사운드
        if (audioSource != null && normalAttackSound != null)
        {
            audioSource.PlayOneShot(normalAttackSound);
        }
    }

    /// <summary>
    /// 크리티컬 공격 처리: 이펙트, 사운드 실행
    /// </summary>
    void OnCriticalAttack(Vector3 position, float damage)
    {
        // 치명타 이펙트
        if (criticalAttackParticle != null)
        {
            ParticleSystem particle = Instantiate(criticalAttackParticle, position, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }

        // 치명타 사운드
        if (audioSource != null && criticalAttackSound != null)
        {
            audioSource.PlayOneShot(criticalAttackSound);
        }
    }

    /// <summary>
    /// 자동 공격 해금 조건 확인 및 처리
    /// </summary>
    public void CheckAutoAttackUnlock()
    {
        int currentAssistLevel = playerstat.GetStatLevel(StatType.AssistSkill);
        
        if (currentAssistLevel >= autoAttackUnlockLevel && !isAutoAttackUnlocked)
        {
            isAutoAttackUnlocked = true;
            SetAutoAttackLevel(1);

            if (autoAttackManager != null)
            {
                autoAttackManager.OnAutoAttackUnlocked();
            }
        }
    }

    /// <summary>
    /// 자동 공격 시작
    /// </summary>
    public void StartAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        if (autoAttackLevel > 0 && isAutoAttackUnlocked)
        {
            autoAttackCoroutine = StartCoroutine(AutoAttackCoroutine());
        }
    }

    /// <summary>
    /// 자동 공격 정지
    /// </summary>
    public void StopAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null;
        }
    }

    /// <summary>
    /// 자동 공격 반복 실행 코루틴
    /// </summary>
    IEnumerator AutoAttackCoroutine()
    {
        while (true)
        {
            float attackInterval = GetAutoAttackInterval();
            yield return new WaitForSeconds(attackInterval);

            if (!isPaused)
            {
                Vector3 targetPosition = GetAutoAttackTargetPosition();

                float critChance = Mathf.Clamp(playerstat.GetStatValue(StatType.CritChance) * 0.01f, 0f, 0.95f);
                bool isCritical = Random.Range(0f, 1f) < critChance;

                float assistPower = playerstat.GetStatValue(StatType.AssistSkill);
                float critBonus = playerstat.GetStatValue(StatType.CritBonus);
                float finalDamage = isCritical ? assistPower * (1f + critBonus) : assistPower;

                cookingAttackHandler.TryAutoAttack(isCritical);

                if (isCritical)
                {
                    OnCriticalAttack(targetPosition, finalDamage);
                }
                else
                {
                    OnNormalAttack(targetPosition, finalDamage);
                }
            }
        }
    }

    /// <summary>
    /// 일시정지 상태 설정
    /// </summary>
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    /// <summary>
    /// 자동 공격 레벨 설정 및 실행 제어
    /// </summary>
    public void SetAutoAttackLevel(int level)
    {
        if (!isAutoAttackUnlocked && level > 0)
        {
            return;
        }

        autoAttackLevel = level;

        if (level > 0)
        {
            StartAutoAttack();
        }
        else
        {
            StopAutoAttack();
        }
    }

    /// <summary>
    /// 플레이어 레벨 설정 및 해금 여부 확인
    /// </summary>
    public void SetPlayerLevel(int level)
    {
        playerLevel = level;
        CheckAutoAttackUnlock();
    }

    /// <summary>
    /// 현재 자동 공격 간격 반환
    /// </summary>
    public float GetAutoAttackInterval()
    {
        float interval = playerstat.GetStatValue(StatType.AssistSpeed);
        return Mathf.Clamp(interval, 0.3f, 15f);
    }

    /// <summary>
    /// 자동 공격 해금 여부 반환
    /// </summary>
    public bool IsAutoAttackUnlocked()
    {
        return isAutoAttackUnlocked;
    }

    /// <summary>
    /// 현재 가장 가까운 적의 위치 반환 (없으면 클릭 위치)
    /// </summary>
    private Vector3 GetAutoAttackTargetPosition()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0)
        {
            return GetClickWorldPosition();
        }
        
        Enemy target = enemies.OrderBy(e => e.transform.position.x).FirstOrDefault();
        return target != null ? target.transform.position : GetClickWorldPosition();
    }
}