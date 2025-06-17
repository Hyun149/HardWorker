using System.Collections.Generic;

/// <summary>
/// 플레이어의 전체 데이터를 저장하고 관리하는 클래스입니다.
/// - 진행 중인 스테이지, 보유 골드, 스킬 포인트, 스탯 강화 수치, 장착 무기 및 보유 무기 리스트 등의 정보를 포함합니다.
/// - 게임 저장 및 불러오기 시 직렬화 대상이 됩니다.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public int stageIndex;                     // 현재 진행 중인 스테이지 인덱스
    public int currentGold;                    // 현재 보유 중인 골드
    public int currentSkillPoint = 1000;       // 보유 스킬 포인트 (기본값 1000)

    public List<SerializableStat> statLevels = new();  // 스탯별 강화 레벨 정보 리스트
    public string equippedWeaponId;                    // 현재 장착 중인 무기 ID
    public List<WeaponSaveData> ownedWeapons = new();  // 소유한 무기 목록

    /// <summary>
    /// PlayerData 클래스의 생성자입니다.
    /// - StatType의 모든 값을 순회하며, 초기 스탯 레벨 0으로 설정합니다.
    /// - 초기 골드 0, 스테이지 1, 장착 무기 "0"으로 초기화합니다.
    /// </summary>
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

    /// <summary>
    /// 특정 스탯의 현재 레벨을 반환합니다.
    /// </summary>
    public int GetStatLevel(StatType type)
    {
        var stat = statLevels.Find(s => s.StatType == type);
        return stat != null ? stat.level : 0;
    }

    /// <summary>
    /// 특정 스탯의 레벨을 지정된 값으로 설정합니다.
    /// </summary>
    public void SetStatLevel(StatType type, int value)
    {
        var stat = statLevels.Find(s => s.StatType == type);
        if (stat != null) stat.level = value;
    }
}

/// <summary>
/// 개별 스탯과 강화 레벨 정보를 저장하는 구조체입니다.
/// </summary>
[System.Serializable]
public class SerializableStat
{
    public StatType StatType;  // 스탯의 종류
    public int level;          // 강화 레벨
}
