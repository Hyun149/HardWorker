using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 전반 데이터를 관리하고, 저장/불러오기 기능을 제공합니다.
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private WeaponInventory weaponInventory;

    public PlayerData playerData { get; private set; }

    private const string SaveFileName = "playerData.json";

    protected override void Awake()
    {
        base.Awake();

        LoadGame();
    }

    /// <summary>
    /// 현재 플레이어 데이터를 저장합니다.
    /// </summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(playerData, true);
        string path = Application.persistentDataPath + "/" + SaveFileName;
        Debug.Log($"[저장 경로] {path}");
        System.IO.File.WriteAllText(path,json);
        Debug.Log($"[저장 완료] {path}");
    }

    /// <summary>
    /// 저장된 데이터를 불러오거나, 없으면 새로 초기화합니다.
    /// </summary>
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/" + SaveFileName;
        Debug.Log($"[불러오기 경로] {path}");

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("[로드 완료] 저장 데이터를 불러옵니다.");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("[초기화] 저장 데이터가 없어 새로 생성합니다.");
        }

        StartCoroutine(WaitAndLoadWeaponInventory());
    }

    private IEnumerator WaitAndLoadWeaponInventory()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");

        yield return new WaitForSeconds(0.1f);

        var inventory = FindObjectOfType<WeaponInventory>();
        if (inventory != null)
        {
            inventory.LoadFromPlayerData(playerData);
            Debug.Log("[무기 인벤토리 복원 완료]");
        }
        else
        {
            Debug.LogWarning("WeaponInventory not found in scene");
        }
    }
}
