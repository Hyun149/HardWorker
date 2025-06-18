using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// 일시적으로 경고 메시지를 화면에 표시하는 UI 제어 클래스입니다.
/// - MonoSingleton을 상속하여 전역 접근이 가능합니다.
/// - 지정된 시간 동안 메시지를 보여준 후 자동으로 숨깁니다.
/// </summary>
public class WarningUI : MonoSingleton<WarningUI>
{
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float showDuration = 1.5f;

    private Coroutine currentRoutine;

    /// <summary>
    /// 초기화 시 경고 패널을 비활성화합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        warningPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 지정된 메시지를 일정 시간 동안 화면에 표시합니다.
    /// 이미 표시 중인 메시지가 있다면 중단 후 새 메시지를 출력합니다.
    /// </summary>
    /// <param name="message">표시할 경고 메시지</param>
    public void Show(string message)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(ShowMessage(message));
    }

    /// <summary>
    /// 실제로 메시지를 보여주고, 일정 시간 후 패널을 비활성화하는 코루틴입니다.
    /// </summary>
    /// <param name="message">표시할 텍스트 내용</param>
    private IEnumerator ShowMessage(string message)
    {
        warningText.text = message;
        warningPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        warningPanel.gameObject.SetActive(false);
    }
}
