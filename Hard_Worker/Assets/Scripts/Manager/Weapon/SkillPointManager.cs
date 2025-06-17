using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 스킬 포인트(SP)를 관리하는 매니저 클래스입니다.
/// - SP 획득, 소비, 변경 알림 이벤트를 처리합니다.
/// - 싱글톤으로 구성되어 어디서든 접근 가능합니다.
/// </summary>
public class SkillPointManager : MonoSingleton<SkillPointManager>
{
    [SerializeField] private int currentSP; // 현재 보유 중인 스킬 포인트
    public UnityEvent OnSPChanged;          // SP 변경 시 호출되는 이벤트 (UI 연동 등)
    

    /// <summary>
    /// 시작 시 저장된 플레이어 SP 정보를 로드하여 초기화합니다.
    /// </summary>
    private void Start()
    {
        SetSP(GameManager.Instance.playerData.currentSkillPoint);
        currentSP =  GameManager.Instance.playerData.currentSkillPoint;
    }

    /// <summary>
    /// 특정 양의 SP가 충분히 존재하는지 확인합니다.
    /// </summary>
    /// <param name="amount">요구하는 SP 수치</param>
    /// <returns>충분하면 true, 부족하면 false</returns>
    public bool HasEnough(int amount) => currentSP >= amount;

    /// <summary>
    /// SP를 소비합니다. 충분한 경우에만 차감됩니다.
    /// </summary>
    /// <param name="amount">소비할 SP 수치</param>
    /// <returns>성공적으로 소비되면 true</returns>
    public bool SpendSP(int amount)
    {
        if (!HasEnough(amount)) return false;
        currentSP -= amount;

        GameManager.Instance.playerData.currentSkillPoint = currentSP;
        GameManager.Instance.SaveGame();

        OnSPChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// SP를 추가합니다.
    /// - SFX 재생 및 저장 반영, 이벤트 호출 포함
    /// </summary>
    /// <param name="amount">추가할 SP 수치</param>
    public void AddSP(int amount)
    {
        currentSP += amount;
        SFXManager.Instance.Play(SFXType.AddSP);
        GameManager.Instance.playerData.currentSkillPoint = currentSP;
        GameManager.Instance.SaveGame();

        OnSPChanged?.Invoke();
    }

    /// <summary>
    /// 현재 SP 값을 반환합니다.
    /// </summary>
    public int GetSP() => currentSP;

    /// <summary>
    /// SP 값을 외부에서 직접 설정합니다.
    /// - 이벤트가 자동으로 호출됩니다.
    /// </summary>
    /// <param name="amount">설정할 SP 수치</param>
    public void SetSP(int amount)
    {
        currentSP = amount;
        OnSPChanged?.Invoke();
    }
}
