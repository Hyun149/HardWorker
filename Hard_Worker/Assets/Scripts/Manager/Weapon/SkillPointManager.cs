using UnityEngine;
using UnityEngine.Events;

public class SkillPointManager : MonoSingleton<SkillPointManager>
{
    [SerializeField] int currentSP;
    public UnityEvent OnSPChanged;
    
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        SetSP(GameManager.Instance.playerData.currentSkillPoint);
        currentSP =  GameManager.Instance.playerData.currentSkillPoint;
    }
    public bool HasEnough(int amount) => currentSP >= amount;
    
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
        SFXManager.Instance.Play(SFXType.AddSP);
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
