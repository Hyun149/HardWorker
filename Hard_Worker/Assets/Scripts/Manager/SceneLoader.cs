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
        { SceneType.MainScene, "MainMenu"}
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
    /// 현재 씬의 이름을 반환합니다.
    /// </summary>
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
