using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// AudioMixer와 연동되어 UI 슬라이더 값을 기반으로 BGM과 SFX의 볼륨을 제어하는 클래스입니다.
/// </summary>
public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider sliderBGM;
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
