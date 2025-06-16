using UnityEngine;
using UnityEngine.Events;

public class SkillPointManager : MonoBehaviour
{
    public int currentSP = 1000;

    public UnityEvent OnSPChanged;
    
    public bool HasEnough(int amount) => currentSP >= amount;

    private void Start()
    {
        SetSP(GameManager.Instance.playerData.currentSkillPoint);
    }

    public bool SpendSP(int amount)
    {
        if (!HasEnough(amount)) return false;
        currentSP -= amount;

        GameManager.Instance.playerData.currentSkillPoint = currentSP;
        GameManager.Instance.SaveGame();

        OnSPChanged?.Invoke();
        return true;
    }

    public void AddSP(int amount)
    {
        currentSP += amount;

        GameManager.Instance.playerData.currentSkillPoint = currentSP;
        GameManager.Instance.SaveGame();

        OnSPChanged?.Invoke();
    }

    public int GetSP() => currentSP;

    public void SetSP(int amount)
    {
        currentSP = amount;
        OnSPChanged?.Invoke();
    }
}
