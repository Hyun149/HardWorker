using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 적(음식 재료)을 관리하는 스크립트입니다.
/// </summary>
public class Enemy : MonoBehaviour
{
    private CustomerManager customerManager;
    private EnemyManager enemyManager;
    private SpriteRenderer spriteRenderer;
    private EnemyProgress enemyProgress;
    private EnemyAni enemyAni;
    public EnemyData enemyData;

    public event Action<float,float> onChangeProgress; // 진행도 변경시 이벤트
    public event Action completedCooking; // 요리 완료시 이벤트

    public Vector3 throwDirection = new Vector3(-2f, -2f, 0f); // 왼쪽 아래 방향
    public float throwDistance = 3f; // 던지는 거리
    public float duration = 0.5f; // 던지는데 걸리는 시간
    public Ease easing = Ease.OutQuad; // 움직임 곡선

    /// <summary>
    /// 적을 초기화합니다.
    /// </summary>
    public void Init( )
    {
        customerManager = FindObjectOfType<CustomerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyProgress = FindObjectOfType<EnemyProgress>();
        enemyAni = GetComponent<EnemyAni>();

        SetImage(); // 적 이미지 변경
        StageManager.Instance.RegisterCookingCompleteEvent();

        SetProgress(); // 진행도 UI 설정
    }
    /// <summary>
    /// 적 이미지를 변경합니다.
    /// </summary>
    void SetImage()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyData.EnemyImage;
    }
    /// <summary>
    /// 요리 진행도 UI를 설정합니다.
    /// </summary>
    void SetProgress()
    {
        // 진행도 UI 설정
        enemyProgress.progressBar.gameObject.SetActive(true);
        enemyProgress.SetMaxProgress(StageManager.Instance.UpdateMaxProgress());
        enemyProgress.Init(); // 진행도 0으로 변경

        onChangeProgress += enemyProgress.UpdateProgressBar;
        enemyProgress.SetTarget(this.transform);
        enemyProgress.UpdateProgressBarPos();// 진행도 UI 위치 갱신
    }
    /// <summary>
    /// 적 피격시(마우스 왼쪽 클릭시) 발생하는 스크립트 입니다.
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(float value)
    {
        // 진행도 증가
        float progress = enemyProgress.CurProgress;
        progress = Mathf.Min(progress + value, enemyProgress.MaxProgress);
        enemyProgress.SetProgress(progress);

        // 진행도 표시
        onChangeProgress?.Invoke(enemyProgress.CurProgress, enemyProgress.MaxProgress);
     
        // 적 애니메이션 재생
        EnemyAni(progress);
    }
   /// <summary>
   /// 적 애니메이션을 재생합니다.
   /// </summary>
   /// <param name="progress"></param>
    void EnemyAni(float progress)
    {
        float ratio = progress / enemyProgress.MaxProgress;

        if (ratio >= 0.9f)
        {
            enemyAni.Cut(3);
        }
        else if (ratio >= 0.6f)
        {
            enemyAni.Cut(2);
        }
        else if (ratio >= 0.3f)
        {
            enemyAni.Cut(1);
        }
        if (progress >= enemyProgress.MaxProgress) // 진행도가 최고치일 경우
        {
            // 보상 지급
            StartCoroutine(Ingredient());
        }
    }
    /// <summary>
    /// 적을 던지는 효과를 재생합니다.
    /// </summary>
    void Throw()
    {  
        // 카메라 기준으로 방향 변환
        Vector3 camRelativeDir = Camera.main.transform.TransformDirection(throwDirection.normalized);

        camRelativeDir.z = 0;

        Vector3 targetPos = transform.position + camRelativeDir.normalized * throwDistance;
        transform.DOMove(targetPos, duration).SetEase(easing);
    }
    /// <summary>
    /// 진행 완료시 보상을 지급합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Ingredient()
    {
        Throw();
        enemyProgress.progressBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(1F);

        // 잡을 적이 남았을 경우 다음 적 생성
        if (enemyManager.EnemyCount < customerManager.food.Enemys.Count)
        { 
            enemyManager.SpawnEnemy();
        }
        // 전부 다 잡은 경우
        else
        {
            // 요리 완성 확인
            completedCooking?.Invoke();
        }
        enemyProgress.SetProgress(0);

       Destroy(this.gameObject);
     }
}
