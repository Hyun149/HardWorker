using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 저장 정보를 위한 데이터 구조입니다.
/// </summary>
[System.Serializable]
public class WeaponSaveData
{
    public string weaponId;
    public int enhanceLevel;
    public int mastery; // 숙련도(임시)

    public WeaponSaveData(string id, int level, int mastery = 0)
    {
        weaponId = id;
        enhanceLevel = level;
        this.mastery = mastery;
    }
}
