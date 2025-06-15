using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;

/// <summary>
/// 효과음 재생과 볼륨 조절을 관리하는 매니저 클래스입니다.
/// - 싱글톤 구조로 전역에서 접근 가능
/// - AudioMixer 연동을 통한 볼륨 조절 지원
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

    public void SetVolume(float volume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(sfxVolumeParam, dbVolume);
    }

    public float GetVolume()
    {
        if (audioMixer.GetFloat(sfxVolumeParam, out float dbVolume))
        {
            return Mathf.Pow(10f, dbVolume / 20f);
        }

        return 1f;
    }
}

[System.Serializable]
public class SFXData
{
    public SFXType type;
    public AudioClip clip;
}
