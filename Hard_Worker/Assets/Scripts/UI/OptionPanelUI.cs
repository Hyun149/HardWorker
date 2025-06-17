using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 설정 버튼과 돌아가기 버튼을 통해 옵션 패널의 활성/비활성화를 제어합니다.
/// </summary>
public class OptionPanelUI : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private UIAnimator optionPanelAnimator;

    [Header("볼륨 설정")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixerController mixerController;

    private void Awake()
    {
        openButton.onClick.AddListener(OpenOptionPanel);
        closeButton.onClick.AddListener(CloseOptionPanel);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void Start()
    {
        bgmSlider.value = mixerController.GetBGMVolume();
        sfxSlider.value = mixerController.GetSFXVolume();
    }

    /// <summary>
    /// 옵션 패널을 화면에 표시합니다.
    /// </summary>
    private void OpenOptionPanel()
    {
        cursorManager?.OnOtherUIOpen();
        SFXManager.Instance.Play(SFXType.UIShow);
        optionPanelAnimator.Show();
    }

    /// <summary>
    /// 옵션 패널을 화면에서 숨깁니다.
    /// </summary>
    private void CloseOptionPanel()
    {
        cursorManager?.OnOtherUIClose();
        SFXManager.Instance.Play(SFXType.UIShow);
        optionPanelAnimator.Hide();
    }

    private void OnBGMVolumeChanged(float value)
    {
        
    }

    private void OnSFXVolumeChanged(float value)
    {

    }
}
