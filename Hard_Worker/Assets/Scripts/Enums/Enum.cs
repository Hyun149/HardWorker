/// <summary>
/// <b>씬 전환 시 사용할 씬 이름의 열거형(Enum) 정의입니다.</b><br/>
/// - 문자열 하드코딩을 방지하여 유지보수를 쉽게 합니다.<br/>
/// - SceneLoader와 함께 사용되어, Enum → 문자열 변환 방식으로 씬을 로드합니다.
/// </summary>
public enum SceneType 
{
    MainScene,
    // 추후 추가 가능
    // Chapter1,
    // BattleScene,
    // EndingScene,
    // LoadingScene
}

/// <summary>
/// 게임의 상태를 정의한 열거형입니다.<br/>
/// 상태별로 GameManager가 각기 다른 흐름을 제어합니다.
/// </summary>
public enum GameState
{
    None,
    Title,
    InGame,
    // Pause, Resume, Clear    
}