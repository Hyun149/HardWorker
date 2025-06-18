using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

/// <summary>
/// SFXManager : 효과음 재생과 볼륨 조절을 관리하는 싱글톤 매니저 클래스입니다.
/// - 전역 접근 가능한 구조로 효과음 재생을 통합 관리합니다.
/// - AudioMixer 연동으로 볼륨 조절을 지원하며, 외부 UI 슬라이더와의 연결이 가능합니다.
/// </summary>
public class SFXManager : MonoSingleton<SFXManager>
{
    [Header("효과음 오디오 소스")]
    [SerializeField] private AudioSource sfxSource;

    [Header("효과음 믹서")]
    [SerializeField] private AudioMixer audioMixer;

    [Tooltip("AudioMixer에서 노출시킨 SFX 볼륨 파라미터 이름")]
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    [Header("사운드 목록")]
    [SerializeField] private List<SFXData> sfxList;

    private Dictionary<SFXType, AudioClip> sfxDict;

    protected override void Awake()
    {
        base.Awake();
        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var sfx in sfxList)
        {
            sfxDict[sfx.type] = sfx.clip;
        }
    }

    /// <summary>
    /// 효과음을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip</param>
    public void Play(SFXType type)
    {
        if (sfxDict.TryGetValue(type, out AudioClip clip) && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// AudioMixer에 설정된 SFX 볼륨을 변경합니다.
    /// - 0~1 범위의 값을 dB(-80 ~ 0)로 변환하여 적용합니다.
    /// </summary>
    /// <param name="volume">0.0 ~ 1.0 사이의 슬라이더 값</param>
    public void SetVolume(float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(sfxVolumeParam, dbVolume);
    }

    /// <summary>
    /// 현재 AudioMixer에서 설정된 SFX 볼륨 값을 가져옵니다.
    /// - dB 단위를 다시 0~1 범위로 환산합니다.
    /// </summary>
    /// <returns>현재 SFX 볼륨 (0.0 ~ 1.0)</returns>
    public float GetVolume()
    {
        if (audioMixer.GetFloat(sfxVolumeParam, out float dbVolume))
        {
            return Mathf.Pow(10f, dbVolume / 20f);
        }

        return 1f;
    }
}

/// <summary>
/// SFXData : 효과음 타입과 해당 AudioClip을 묶어 에디터에서 등록할 수 있게 해주는 구조체입니다.
/// </summary>
[System.Serializable]
public class SFXData
{
    public SFXType type;     // 효과음의 구분 타입
    public AudioClip clip;   // 재생할 오디오 클립
}
