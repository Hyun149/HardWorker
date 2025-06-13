using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventHandler : MonoBehaviour
{
    [Header("클릭 설정")]
    [SerializeField] private bool isPaused = false;

    [Header("자동 공격 설정")]
    [SerializeField] private int autoAttackLevel = 0;
    [SerializeField] private float baseAutoAttackInterval = 1.0f;

    [Header("치명타 설정")]
    [SerializeField] private float criticalChance = 0.1f; // 10% 기본 치명타 확률

    [Header("파티클 시스템")]
    [SerializeField] private ParticleSystem normalAttackParticle;
    [SerializeField] private ParticleSystem criticalAttackParticle;

    [Header("오디오")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip normalAttackSound;
    [SerializeField] private AudioClip criticalAttackSound;

    private Camera mainCamera;
    private Coroutine autoAttackCoroutine;

    void Start()
    {
        mainCamera = Camera.main;

        // 자동 공격 시작
        if (autoAttackLevel > 0)
        {
            StartAutoAttack();
        }
    }

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

    void PerformClick()
    {
        Vector3 clickPosition = GetClickWorldPosition();

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

    Vector3 GetClickWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // 카메라로부터의 거리
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

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

    // 자동 공격 시작
    public void StartAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        if (autoAttackLevel > 0)
        {
            autoAttackCoroutine = StartCoroutine(AutoAttackCoroutine());
        }
    }

    // 자동 공격 중지
    public void StopAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null;
        }
    }

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

    // 일시정지 설정
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    // 자동 공격 레벨 설정
    public void SetAutoAttackLevel(int level)
    {
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

    // 치명타 확률 설정
    public void SetCriticalChance(float chance)
    {
        criticalChance = Mathf.Clamp01(chance);
    }
}