using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// AudioMixerController : AudioMixer와 연동되어 UI 슬라이더를 통해 BGM과 SFX 볼륨을 제어하는 클래스입니다.
/// - 슬라이더 값(0.0 ~ 1.0)을 데시벨 단위로 변환해 AudioMixer에 적용합니다.
/// - 설정된 볼륨 값을 다시 0.0 ~ 1.0 사이의 값으로 되돌릴 수 있습니다.
/// </summary>
public class AudioMixerController : MonoBehaviour
{
    [Header("오디오 믹서 연결")]
    [Tooltip("BGM/SFX 볼륨 조절에 사용할 AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI 슬라이더")]
    [Tooltip("BGM 볼륨을 조절할 슬라이더")]
    [SerializeField] private Slider sliderBGM;

    [Tooltip("SFX 볼륨을 조절할 슬라이더")]
    [SerializeField] private Slider sliderSFX;

    /// <summary>
    /// BGM 볼륨을 설정합니다.
    /// 슬라이더 값(0~1)을 받아 데시벨로 변환 후 AudioMixer에 전달합니다.
    /// </summary>
    /// <param name="value">슬라이더의 현재 값 (0.0 ~ 1.0)</param>
    public void SetBGMVolume(float value)
    {
        float dB = VolumeToDecibel(value);
        audioMixer.SetFloat("BGM", VolumeToDecibel(value));
    }

    /// <summary>
    /// 효과음(SFX) 볼륨을 설정합니다.
    /// 슬라이더 값(0~1)을 받아 데시벨로 변환 후 AudioMixer에 전달합니다.
    /// </summary>
    /// <param name="value">슬라이더의 현재 값 (0.0 ~ 1.0)</param>
    public void SetSFXVolume(float value)
    {
        float dB = VolumeToDecibel(value);
        audioMixer.SetFloat("SFX", VolumeToDecibel(value));
    }

    /// <summary>
    /// 현재 AudioMixer에 설정된 BGM 볼륨 값을 0~1 범위로 환산하여 반환합니다.
    /// </summary>
    /// <returns>BGM 볼륨 (0.0 ~ 1.0)</returns>
    public float GetBGMVolume()
    {
        if (audioMixer.GetFloat("BGM", out float dB))
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        return 1f;
    }

    /// <summary>
    /// 현재 AudioMixer에 설정된 BGM 볼륨 값을 0~1 범위로 환산하여 반환합니다.
    /// </summary>
    /// <returns>BGM 볼륨 (0.0 ~ 1.0)</returns>
    public float GetSFXVolume()
    {
        if (audioMixer.GetFloat("SFX", out float dB))
        {
            return Mathf.Pow(10f, dB / 20f);
        }
        return 1f;
    }

    /// <summary>
    /// 슬라이더 값(0.0 ~ 1.0)을 데시벨 값으로 변환합니다.
    /// AudioMixer는 데시벨 단위를 사용하므로 로그 변환이 필요합니다.
    /// </summary>
    /// <param name="Volume">슬라이더 값 (0.0001 이상)</param>
    /// <returns>데시벨 값 (최소 -80dB)</returns>
    private float VolumeToDecibel(float Volume)
    {
        if (Volume <= 0.0001f)
        {
            return -80f;
        }

        return Mathf.Log10(Volume) * 20;
    }
}
