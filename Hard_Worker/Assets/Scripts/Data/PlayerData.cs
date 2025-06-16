using System.Collections.Generic;

/// <summary>
/// 플레이어의 전체 데이터를 관리하는 클래스입니다.
/// - 진행도, 재화, 스탯 강화 현황, 장비 상태 등을 포함합니다.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public int stageIndex;
    public int currentGold;
    
    public List<SerializableStat> statLevels = new();

    public string equippedWeaponId;  // 현재 장착 중인 무기 ID
    
    public List<WeaponSaveData> ownedWeapons = new();  // 소유 무기 전체


    public PlayerData()
    {
        foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
        {
            statLevels.Add(new SerializableStat { StatType = stat, level = 0 });
        }

        currentGold = 0;
        stageIndex = 1;
        equippedWeaponId = "0";
    }

    public int GetStatLevel(StatType type)
    {
        var stat = statLevels.Find(s => s.StatType == type);
        return stat != null ? stat.level : 0;
    }

    public void SetStatLevel(StatType type, int value)
    {
        var stat = statLevels.Find(s => s.StatType == type);
        if (stat != null) stat.level = value;
    }
}

[System.Serializable]
public class SerializableStat
{
    public StatType StatType;
    public int level;
}
