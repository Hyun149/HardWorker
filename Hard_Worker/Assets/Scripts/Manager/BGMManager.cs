using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환에 따라 배경음악(BGM)을 자동으로 재생하는 싱글톤 BGM 매니저입니다.<br/>
/// - 각 SceneType에 대응되는 AudioClip을 매핑하여 관리합니다.<br/>
/// - 씬이 로드될 때 자동으로 해당 BGM을 재생합니다.
/// </summary>
public class BGMManager : MonoSingleton<BGMManager>
{
    /// <summary>
    /// 씬 타입별로 재생할 BGM 클립을 지정하기 위한 구조체입니다.
    /// </summary>
    [System.Serializable]
    public struct SceneBGM
    {
        public SceneType sceneType;
        public AudioClip bgmClip;
    }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<SceneBGM> sceneBGMs = new();

    private Dictionary<SceneType, AudioClip> bgmDict;

    /// <summary>
    /// 싱글톤 초기화 및 BGM 매핑 테이블 생성 후 씬 로드 이벤트를 등록합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        bgmDict = new Dictionary<SceneType, AudioClip>();
        foreach (var item in sceneBGMs)
        {
            if (!bgmDict.ContainsKey(item.sceneType))
            {
                bgmDict.Add(item.sceneType, item.bgmClip);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 씬 언로드 시 이벤트 연결을 해제합니다.
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 씬 로드 시 호출되며, 해당 씬 타입에 맞는 BGM을 재생합니다.
    /// </summary>
    /// <param name="scene">로드된 씬 정보</param>
    /// <param name="mode">씬 로드 방식</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var sceneType = SceneLoader.GetSceneType(scene.name);
        if (sceneType.HasValue)
        {
            PlayBGM(sceneType.Value);
        }
    }

    /// <summary>
    /// 특정 씬 타입에 대응하는 BGM을 재생합니다.<br/>
    /// 같은 클립이 이미 재생 중이면 중복 재생을 방지합니다.
    /// </summary>
    /// <param name="sceneType">재생할 씬 타입</param>
    public void PlayBGM(SceneType sceneType)
    {
        if (bgmDict.TryGetValue(sceneType, out AudioClip clip))
        {
            if (bgmSource.clip == clip) 
            {
                return;
            }
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }
    }
}
