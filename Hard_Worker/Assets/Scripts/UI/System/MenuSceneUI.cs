using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MenuScene에서 UI 버튼 처리를 담당하는 스크립트입니다.
/// </summary>
public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private Button startButton; 

    private void Awake()
    {
        startButton.onClick.AddListener(OnClickStart);
    }

    /// <summary>
    /// 게임 시작 버튼 클릭 시 GameScene으로 전환합니다.
    /// </summary>
    private void OnClickStart()
    {
        SFXManager.Instance.Play(SFXType.ButtonClick);
        SceneLoader.LoadWithFade(SceneType.GameScene);
    }
}
