using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager : 게임 전반의 데이터를 관리하고 저장/불러오기 기능을 제공하는 싱글톤 클래스입니다.
/// - 플레이어 데이터(PlayerData)의 영속적 저장 및 로드를 담당합니다.
/// - WeaponInventory를 포함한 씬 내 구성 요소들과의 연동도 처리합니다.
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    /// <summary>
    /// 현재 플레이어 데이터입니다.
    /// - 진행도, 골드, 무기 정보 등을 포함합니다.
    /// </summary>
    public PlayerData playerData { get; private set; }

    [SerializeField] private WeaponInventory weaponInventory;

    private const string SaveFileName = "playerData.json";

    /// <summary>
    /// GameManager가 생성될 때 호출됩니다.
    /// - 싱글톤 초기화 및 게임 데이터 로드를 수행합니다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        LoadGame();
    }

    /// <summary>
    /// 현재 플레이어 데이터를 JSON으로 저장합니다.
    /// - SaveFileName 경로에 저장됩니다.
    /// </summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(playerData, true);
        string path = Application.persistentDataPath + "/" + SaveFileName;
        System.IO.File.WriteAllText(path,json);
    }

    /// <summary>
    /// 저장된 플레이어 데이터를 불러오거나, 없으면 새로 초기화합니다.
    /// - 저장 파일이 존재하지 않을 경우, 새로운 PlayerData를 생성합니다.
    /// - 무기 인벤토리 복원은 별도 코루틴에서 처리됩니다.
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

    /// <summary>
    /// GameScene이 완전히 로드된 후 WeaponInventory를 복원하는 코루틴입니다.
    /// - 씬 로드 완료까지 대기 후 인벤토리를 찾아 데이터를 로드합니다.
    /// </summary>
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
