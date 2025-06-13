using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum 기반으로 씬을 로드하는 씬 매니저 클래스입니다.
/// - 하드코딩된 문자열 사용을 방지하고, 유지보수를 쉽게 합니다.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Enum 기반으로 씬을 로드하는 씬 매니저 클래스입니다.
    /// - 하드코딩된 문자열 사용을 방지하고, 유지보수를 쉽게 합니다.
    /// </summary>
    private static readonly Dictionary<SceneType, string> sceneMap = new Dictionary<SceneType, string>
    {
        { SceneType.TitleScene, "TitleScene"},
        { SceneType. MenuScene, "MenuScene"},
        { SceneType.GameScene, "GameScene" }
    };

    /// <summary>
    /// 지정한 씬 타입으로 씬을 로드합니다.
    /// </summary>
    /// <param name="sceneType">전환할 씬의 타입</param>
    public static void Load(SceneType sceneType)
    {
        if (sceneMap.TryGetValue(sceneType, out string sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"[SceneLoader] {sceneType}에 대한 씬 이름이 등록되지 않았습니다.");
        }
    }

    /// <summary>
    /// 페이드 아웃 후 지정된 씬으로 전환하고, 전환 완료 후 페이드 인합니다.
    /// </summary>
    /// <param name="sceneType">전환할 씬의 타입</param>
    public static void LoadWithFade(SceneType sceneType)
    {
        if (sceneMap.TryGetValue(sceneType, out string sceneName))
        {
            FadeManager.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(sceneName);
                FadeManager.Instance.FadeIn();
            });
        }
        else
        {
            Debug.LogError($"[SceneLoader] {sceneType}에 대한 씬 이름이 등록되지 않았습니다.");
        }
    }

    /// <summary>
    /// 현재 씬의 이름을 반환합니다.
    /// </summary>
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// 씬 이름(string)을 받아 해당하는 SceneType 열거형을 반환합니다.<br/>
    /// - sceneMap 딕셔너리를 순회하여 값이 일치하는 키를 찾아 반환합니다.
    /// </summary>
    /// <param name="sceneName">찾고자 하는 씬의 이름 (문자열)</param>
    /// <returns>해당하는 SceneType이 존재하면 반환하고, 없으면 null 반환</returns>
    public static SceneType? GetSceneType(string sceneName)
    {
        foreach (var kvp in sceneMap)
        {
            if (kvp.Value == sceneName)
            {
                return kvp.Key;
            }
        }

        return null;
    }
}
