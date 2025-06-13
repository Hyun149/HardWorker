using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// 일시적으로 경고 메시지를 화면에 표시하는 UI 제어 클래스입니다.
/// </summary>
public class WarningUI : MonoSingleton<WarningUI>
{
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float showDuration = 1.5f;

    private Coroutine currentRoutine;

    protected override void Awake()
    {
        base.Awake();
        warningPanel.gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(ShowMessage(message));
    }

    private IEnumerator ShowMessage(string message)
    {
        warningText.text = message;
        warningPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        warningPanel.gameObject.SetActive(false);
    }
}
