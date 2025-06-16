using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TitleScene에서 MenuScene으로 이동을 돕는 UI 제어 스크립트입니다.<br/>
/// - 시작 버튼을 클릭하면 SceneLoader를 통해 MenuScene을 로드합니다.
/// </summary>
public class MenuUI : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private Button startButton; // 인스펙터 창에 버튼 드래그

    /// <summary>
    /// 컴포넌트가 활성화될 때 버튼 클릭 이벤트를 등록합니다.
    /// </summary>
    private void Awake()
    {
        startButton.onClick.AddListener(OnClickStart);
    }

    /// <summary>
    /// 시작 버튼 클릭 시 호출되어 MenuScene으로 씬을 전환합니다.
    /// </summary>
    private void OnClickStart()
    {
        cursorManager.OnOtherUIOpen();
        SFXManager.Instance.Play(SFXType.ButtonClick);
        SceneLoader.LoadWithFade(SceneType.MenuScene);
    }
}
