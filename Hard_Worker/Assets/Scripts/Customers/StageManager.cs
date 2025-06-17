using System;
using UnityEngine;

/// <summary>
/// 게임 내 스테이지를 관리하는 클래스입니다.
/// - 스테이지 진행, 보상 설정 및 계산, 손님 생성 및 주문 관리 기능을 포함합니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    public event Action<int> onStageChanged; // 스테이지 변경시 event

    [SerializeField] int stage = 0; // 현재 스테이지 정보
    [SerializeField] private int reward; // 보상
    [SerializeField] private CustomerManager customerManager;

    private LineController lineController;
    private EnemyManager enemyManager;
    private Enemy enemy;

    public int Stage => stage;
    public int Reward => reward;

    /// <summary>
    /// 싱글톤 설정 및 중복 방지 처리합니다.
    /// </summary>
    private void Awake()
    {
        // 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// 스테이지 시작 시 필요한 매니저 및 라인 초기화를 수행합니다.
    /// </summary>
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        lineController = GetComponent<LineController>();

        stage = GameManager.Instance.playerData.stageIndex;

        onStageChanged?.Invoke(stage);

        StartStage();
    }

    /// <summary>
    /// 스테이지를 시작합니다.
    /// </summary>
    void StartStage()
    {    
        customerManager.SpawnCustomer(); // 손님 소환
      
        lineController.CreateLine(); // 줄세우기
        lineController.StartCoroutine(lineController.HandleOrder()); // 주문하기
    }

    /// <summary>
    /// 스테이지 계수 (1 + 0.2 * stage) 반환
    /// </summary>
    private float StageMultiplier => 1f + 0.2f * stage;

    /// <summary>
    /// 보상의 기본 값을 설정합니다.
    /// </summary>
    /// <returns></returns>
    public void SetBaseReward(int value)
    {
        reward = value;
    }

    /// <summary>
    /// 스테이지마다 보상을 증가시킵니다.
    /// </summary>
    /// <returns></returns>
    public void UpdateReward()
    {
        reward = (int)(reward * StageMultiplier);
    }

    /// <summary>
    /// 보상을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int ReturnReward()
    {
        return reward;
    }

    /// <summary>
    /// 스테이지마다 최대 진행도를 증가시킵니다.
    /// </summary>
    /// <returns></returns>
    public int UpdateMaxProgress()
    {
        return (int)(100f * StageMultiplier);
    }

    /// <summary>
    /// 요리 완성시 발생하는 event에 구독합니다.
    /// </summary>
    public void RegisterCookingCompleteEvent()
    {
        enemy = enemyManager.enemy;
        enemy.completedCooking -= NextStage;
        enemy.completedCooking += NextStage;
    }

    /// <summary>
    /// 다음 스테이지로 진입합니다.
    /// - 스테이지 증가 및 보상 갱신, 저장 처리 포함
    /// </summary>
    public void NextStage()
    {
        stage++;
        onStageChanged?.Invoke(stage);

        // EnemyCount 초기화합니다.
        enemyManager.UpEnemyCount(-enemyManager.EnemyCount);

        // 보상을 증가시킵니다.
        UpdateReward();

        GameManager.Instance.playerData.stageIndex = stage;
        GameManager.Instance.SaveGame();
    }

    /// <summary>
    /// 능력치(수익 증가율)를 반영한 최종 보상을 계산하고 골드를 지급합니다.
    /// </summary>
    /// <returns>최종 지급된 보상 금액</returns>
    public int GiveReward()
    {
        float incomeBonus = FindAnyObjectByType<PlayerStat>().GetFinalStatValue(StatType.Income);
        int finalReward = Mathf.RoundToInt(reward * (1f + (incomeBonus / 100)));

        Debug.Log($"기본 보상(능력치 반영X): {reward}, 능력치로 인한 수익 증가율:{incomeBonus}%, 최종보상: {finalReward}");
        GoldManager.Instance.AddGold(finalReward);
        return finalReward;
    }
}
