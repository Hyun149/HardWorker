using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", VolumeToDecibel(value));
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", VolumeToDecibel(value));
    }

    private float VolumeToDecibel(float Volume)
    {
        Volume = Mathf.Clamp(Volume, 0.0001f, 1f);
        return Mathf.Log10(Volume) * 20;
    }
}
