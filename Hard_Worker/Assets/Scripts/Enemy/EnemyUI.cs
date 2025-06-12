using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class EnemyUI : MonoBehaviour
{
    public UnityEngine.UI.Slider progressBarPrefab;
    UnityEngine.UI.Slider progressBar;
    Camera cam;
    Enemy enemy;
    public Vector3 offset = new Vector3(0, -2.5f);

    void OnEnable()
    {
        cam = Camera.main;

        // 진행도 UI 생성
        progressBar = Instantiate(progressBarPrefab, GameObject.Find("Canvas").transform);

        enemy = GetComponent<Enemy>();
        enemy.onChangeProgress += UpdateProgressBar;

        // 진행도 UI 위치 갱신
        UpdateProgressBarPos();
    }
    // 진행도 UI 위치 갱신
     void UpdateProgressBarPos()
    {
        if (progressBar == null || cam == null) return;

        // 위치 변환
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position + offset);

        // 위치 갱신
        progressBar.transform.position = screenPos;
    }
    // 진행도 Bar 값 갱신
    void UpdateProgressBar(float current, float max)
    {
        if (progressBar == null) return;
        progressBar.value = current / max;

    }
    // 적이 죽었을 때 진행도 UI 삭제
    void OnDestroy()
    {
        if (progressBar == null) return;
        Destroy(progressBar);
    }
}
