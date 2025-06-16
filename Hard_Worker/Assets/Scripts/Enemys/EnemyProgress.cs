using System.Collections;
using UnityEngine;

/// <summary>
/// 진행도를 관리하는 스크립트입니다.
/// </summary>
public class EnemyProgress : MonoBehaviour
{
    EnemyManager enemyManager;

    [SerializeField] private float curProgress; // 현재 진행도
    [SerializeField] private float maxProgress; // 최대 진행도
    private float targetProgress;

    public UnityEngine.UI.Slider progressBarPrefab;
    public UnityEngine.UI.Slider progressBar;
    public GameObject boosProgressBar;

    Camera cam;

    public Transform target; // Enemy 위치
    public Vector3 offset = new Vector3(0, 1.5f,0); 
    private RectTransform rt;

    private Coroutine hpCoroutine;

    public float CurProgress => curProgress;
    public float MaxProgress => maxProgress;
    public float TargetProgress => targetProgress;

    void Start()
    {
        cam = Camera.main;
        enemyManager = GetComponent<EnemyManager>();
        // 진행도 UI 생성
        progressBar = Instantiate(progressBarPrefab,GameObject.Find("Canvas").transform);
        rt = progressBar.GetComponent<RectTransform>();
    }
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
            target = boosProgressBar.transform;
            rt.sizeDelta = new Vector2(900, 200);
        }
        else
        {
            rt.sizeDelta = new Vector2(361.8f, 100);
        }
            // 위치 변환
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

        // 위치 갱신
        rt.position = screenPos;
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
    public void SetMaxProgress(float progress)
    {
        maxProgress = progress;

        if (enemyManager.enemy.enemyData.IsBoss == true)
        {
            maxProgress = maxProgress * 5;
        }
        UpdateProgressBar(curProgress, maxProgress);
    }
}
