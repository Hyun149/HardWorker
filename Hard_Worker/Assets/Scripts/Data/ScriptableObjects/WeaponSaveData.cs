/// <summary>
/// 무기 저장 정보를 위한 데이터 구조입니다.
/// - 무기의 고유 ID, 강화 레벨, 숙련도를 저장합니다.
/// - 세이브 파일에 직렬화되어 플레이어 보유 무기 정보를 관리합니다.
/// </summary>
[System.Serializable]
public class WeaponSaveData
{
    public string weaponId;        // 무기 고유 ID
    public int enhanceLevel;       // 무기 강화 레벨
    public int mastery;            // 무기 숙련도

    /// <summary>
    /// WeaponSaveData 생성자입니다.
    /// </summary>
    /// <param name="id">무기 고유 ID</param>
    /// <param name="level">강화 레벨</param>
    /// <param name="mastery">숙련도 (기본값 0)</param>
    public WeaponSaveData(string id, int level, int mastery = 0)
    {
        weaponId = id;
        enhanceLevel = level;
        this.mastery = mastery;
    }
}
