using UnityEngine;
using UnityEngine.Events;

public class SkillPointManager : MonoBehaviour
{
    public static SkillPointManager Instance;

    private int currentSP = 1000;

    public UnityEvent OnSPChanged;

    private void Awake()=> Instance = this;
    public bool HasEnough(int amount) => currentSP >= amount;

    public bool SpendSP(int amount)
    {
        if (!HasEnough(amount)) return false;
        currentSP -= amount;
        OnSPChanged?.Invoke();
        return true;
    }

    public void AddSP(int amount)
    {
        currentSP += amount;
        OnSPChanged?.Invoke();
    }

    public int GetSP() => currentSP;
}
