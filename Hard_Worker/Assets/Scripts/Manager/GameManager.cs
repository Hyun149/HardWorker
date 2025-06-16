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
        System.IO.File.WriteAllText(path,json);
    }

    /// <summary>
    /// 저장된 데이터를 불러오거나, 없으면 새로 초기화합니다.
    /// </summary>
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/" + SaveFileName;

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            playerData = new PlayerData();
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
        }
        else
        {
            Debug.LogWarning("WeaponInventory not found in scene");
        }
    }
}
