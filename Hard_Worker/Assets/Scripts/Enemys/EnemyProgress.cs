using UnityEngine;

/// <summary>
/// 진행도를 관리하는 스크립트입니다.
/// </summary>
public class EnemyProgress : MonoBehaviour
{
    [SerializeField] private float curProgress; // 현재 진행도
    [SerializeField] private float maxProgress; // 최대 진행도

    public UnityEngine.UI.Slider progressBarPrefab;
    public UnityEngine.UI.Slider progressBar;

    Camera cam;

    public Transform target; // Enemy 위치
    public Vector3 offset = new Vector3(0, 1.5f,0); 
    private RectTransform rt;

    public float CurProgress => curProgress;
    public float MaxProgress => maxProgress;

    void Start()
    {
        cam = Camera.main;
     
        // 진행도 UI 생성
        progressBar = Instantiate(progressBarPrefab,GameObject.Find("GameCanvas").transform);
        rt = progressBar.GetComponent<RectTransform>();
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
    /// 진행도 Bar 값 갱신
    /// </summary>
    public void UpdateProgressBar(float current, float max)
    {
        if (progressBar == null) return;
        progressBar.value = current / max;
    }
    /// <summary>
    /// 현재 진행도를 변경합니다.
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        curProgress = progress;
        UpdateProgressBar(curProgress,maxProgress);
    }
    /// <summary>
    /// 최대 진행도를 변경합니다.
    /// </summary>
    /// <param name="progress"></param>
    public void SetMaxProgress(float progress)
    {
        maxProgress = progress;
        UpdateProgressBar(curProgress, maxProgress);
    }
}
