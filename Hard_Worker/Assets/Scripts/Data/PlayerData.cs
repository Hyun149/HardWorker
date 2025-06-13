using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 전체 데이터를 관리하는 클래스입니다.
/// - 진행도, 재화, 스탯 강화 현황, 장비 상태 등을 포함합니다.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public int stageIndex;
    public int currentGold;

    public Dictionary<StatType, int> statLevels = new();

    public List<string> equippedItemIDs = new();

    public PlayerData()
    {
        foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
        {
            statLevels[stat] = 0;
        }
        currentGold = 0;
        stageIndex = 1;
    }
}
