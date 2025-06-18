using System.Collections;
using UnityEngine;

/// <summary>
/// 적의 조리 진행도를 관리하고 UI에 표시하는 클래스입니다.
/// - 일반 적/보스 적의 진행도를 슬라이더 UI로 표시합니다.
/// - 진행도 값과 UI 위치를 실시간으로 갱신합니다.
/// </summary>
public class EnemyProgress : MonoBehaviour
{
    public GameObject bossProgressBar;               // 보스 전용 슬라이더 참조 (위치 지정용)
    public UnityEngine.UI.Slider progressBar;       // 인스턴스화된 진행도 바
    
    [SerializeField] private UnityEngine.UI.Slider progressBarPrefab; // 진행도 UI 프리팹
    
    [SerializeField] private float curProgress; // 현재 진행도 값
    [SerializeField] private float maxProgress; // 최대 진행도 값
    
    [SerializeField] private Transform target; // Enemy 위치
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f,0); 

    private EnemyManager enemyManager;
    private Camera cam;
    private RectTransform rt;
    private Coroutine hpCoroutine;

    private float targetProgress;              // 목표 진행도 (부드러운 이동용)

    public float CurProgress => curProgress;
    public float MaxProgress => maxProgress;
    public float TargetProgress => targetProgress;

    void Start()
    {
        cam = Camera.main;
        enemyManager = GetComponent<EnemyManager>();

        // 진행도 UI 생성 및 RectTransform 캐싱
        progressBar = Instantiate(progressBarPrefab,GameObject.Find("BackGroundCanvas").transform);
        rt = progressBar.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 진행도 수치와 UI를 초기화합니다.
    /// </summary>
    public void Init()
    {
        curProgress = 0;
        targetProgress = curProgress;
        progressBar.value = curProgress / maxProgress;
    }

    /// <summary>
    /// 진행도 UI의 타겟을 설정합니다.
    /// </summary>
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    /// <summary>
    /// 진행도 UI 위치를 갱신합니다.
    /// </summary>
    public void UpdateProgressBarPos()
    {
        if (progressBar == null || cam == null || target == null) return;

        // Slider의 RectTransform 가져오기
        RectTransform rt = progressBar.GetComponent<RectTransform>();

        if (target.GetComponent<Enemy>().enemyData.IsBoss == true)
        {
            target = bossProgressBar.transform;
            rt.sizeDelta = new Vector2(900, 200);
        }
        else
        {
            rt.sizeDelta = new Vector2(361.8f, 100);
        }
            // 위치 변환
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

        Vector2 localPoint;
        RectTransform canvasRect = progressBar.transform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out localPoint))
        {
            rt.localPosition = localPoint;
        }
    }

    /// <summary>
    /// 진행도 Bar 값을 갱신 코루틴을 실행합니다.
    /// </summary>
    public void UpdateProgressBar(float current, float max)
    {
        if (hpCoroutine != null)
            StopCoroutine(hpCoroutine);

        hpCoroutine = StartCoroutine(SmoothProgressBarUpdate());
    }

    /// <summary>
    /// 부드럽게 진행도 Bar 값을 갱신합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SmoothProgressBarUpdate()
    {
        float duration = 0.1f; // 부드럽게 이동할 시간
        float timer = 0f;
        float start = curProgress / maxProgress;
        float end = targetProgress / maxProgress;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            progressBar.value = Mathf.Lerp(start, end, t);
            yield return null;
        }

        curProgress = targetProgress;
        progressBar.value = end;

        hpCoroutine = null;
    }

    /// <summary>
    /// 현재 진행도를 변경합니다.
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        targetProgress = progress;
        UpdateProgressBar(curProgress,maxProgress);
    }

    /// <summary>
    /// 최대 진행도를 변경합니다.
    /// </summary>
    public void SetMaxProgress(float progress, bool isBoss)
    {
        maxProgress = progress;

        if (isBoss)
        {
            maxProgress = maxProgress * 5;
        }
        UpdateProgressBar(curProgress, maxProgress);
    }
}
