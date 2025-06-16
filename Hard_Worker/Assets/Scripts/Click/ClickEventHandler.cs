using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 클릭 기반 공격 및 자동 공격을 처리하는 클래스입니다.
/// - 플레이어 레벨에 따라 자동 공격 해금
/// - 치명타 확률 처리
/// - 클릭/자동 공격 시 파티클 및 사운드 출력
/// </summary>
public class ClickEventHandler : MonoBehaviour
{
    [Header("클릭 설정")]
    [SerializeField] private bool isPaused = false;

    [Header("플레이어 레벨")]
    [SerializeField] private int playerLevel = 1;
    [SerializeField] private int autoAttackUnlockLevel = 5; // 자동 공격 해금 레벨

    [Header("자동 공격 설정")]
    [SerializeField] private int autoAttackLevel = 0;
    [SerializeField] private float baseAutoAttackInterval = 1.0f;
    [SerializeField] private bool isAutoAttackUnlocked = false;

    [Header("치명타 설정")]
    [SerializeField] private float criticalChance = 0.1f; // 10% 기본 치명타 확률

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
    private AutoAttackManager autoAttackManager; // 추가

    /// <summary>
    /// 초기화: 메인 카메라 참조 및 자동 공격 상태 확인
    /// </summary>
    void Start()
    {
        mainCamera = Camera.main;
        autoAttackManager = FindObjectOfType<AutoAttackManager>(); // 추가

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
        Debug.Log("클릭됨");
        cursorManager?.PlayClickAnimation();
        // 치명타 판정
        bool isCritical = Random.Range(0f, 1f) < criticalChance;

        if (isCritical)
        {
            // 치명타 공격
            OnCriticalAttack(clickPosition);
        }
        else
        {
            // 일반 공격
            OnNormalAttack(clickPosition);
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
    void OnNormalAttack(Vector3 position)
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

        // GameManager에 공격 이벤트 전달 (다른 팀원이 구현)
        // GameManager.Instance.OnPlayerAttack(damage);
    }

    /// <summary>
    /// 치명타 공격 처리: 이펙트, 사운드 실행
    /// </summary>
    void OnCriticalAttack(Vector3 position)
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

        // GameManager에 치명타 이벤트 전달 (다른 팀원이 구현)
        // GameManager.Instance.OnPlayerCriticalAttack(criticalDamage);
    }

    /// <summary>
    /// 자동 공격 해금 조건 확인 및 처리
    /// </summary>
    void CheckAutoAttackUnlock()
    {
        if (playerLevel >= autoAttackUnlockLevel && !isAutoAttackUnlocked)
        {
            isAutoAttackUnlocked = true;
            Debug.Log($"레벨 {autoAttackUnlockLevel} 달성! 자동 공격이 해금되었습니다!");

            // 자동 공격 레벨 1로 설정하여 시작
            SetAutoAttackLevel(1);

            // AutoAttackManager에게 해금 알림 - 추가
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
            // 자동 공격 간격 계산 (레벨이 높을수록 빠르게)
            float attackInterval = baseAutoAttackInterval / (1 + (autoAttackLevel - 1) * 0.1f);
            yield return new WaitForSeconds(attackInterval);

            if (!isPaused)
            {
                // 화면 중앙에 자동 공격
                Vector3 centerPosition = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10f));

                // 자동 공격은 항상 일반 공격으로 처리
                OnNormalAttack(centerPosition);
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
            Debug.Log($"자동 공격은 레벨 {autoAttackUnlockLevel}에 해금됩니다!");
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
    /// 치명타 확률 설정
    /// </summary>
    public void SetCriticalChance(float chance)
    {
        criticalChance = Mathf.Clamp01(chance);
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
    public float GetCurrentAutoAttackInterval()
    {
        if (autoAttackLevel <= 0) return 0f;
        return baseAutoAttackInterval / (1 + (autoAttackLevel - 1) * 0.1f);
    }

    /// <summary>
    /// 자동 공격 해금 여부 반환
    /// </summary>
    public bool IsAutoAttackUnlocked()
    {
        return isAutoAttackUnlocked;
    }
}