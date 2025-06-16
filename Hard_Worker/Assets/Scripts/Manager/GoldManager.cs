using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 플레이어의 골드를 관리하는 매니저 클래스입니다.
/// - 골드 획득, 소비, 부족 시 이벤트 처리 등을 담당합니다.
/// </summary>
public class GoldManager : MonoSingleton<GoldManager>
{
    public int CurrentGold { get; private set; }

    public UnityEvent<int> onGoldChanged = new();

    /// <summary>
    /// 골드를 추가합니다.
    /// </summary>
    public void AddGold(int amount)
    {
        SFXManager.Instance.Play(SFXType.AddGold);
        CurrentGold += amount;
        onGoldChanged.Invoke(CurrentGold);
    }

    /// <summary>
    /// 골드를 소비합니다. 부족할 경우 false를 반환합니다.
    /// </summary>
    public bool SpendGold(int amount)
    {
        if (CurrentGold < amount)
        {
            WarningUI.Instance.Show("골드가 부족합니다!");
            return false;
        }

        CurrentGold -= amount;
        onGoldChanged.Invoke(CurrentGold);
        return true;
    }
}
