using System;
using UnityEngine;

/// <summary>
/// 스테이지를 관리하는 스크립트입니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    LineController lineController;
    int stage = 0; // 현재 스테이지 정보
    [SerializeField] private int reward; // 보상

    public CustomerManager customerManager;
    public event Action<int> onStageChanged; // 스테이지 변경시 event

    EnemyManager enemyManager;
    Enemy enemy;

    public int Stage => stage;
    public int Reward => reward;

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
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        lineController = GetComponent<LineController>();
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
    /// 다음 스테이지로 이동합니다.
    /// </summary>
    public void NextStage()
    {
        stage++;
        onStageChanged?.Invoke(stage + 1);

        // 손님 퇴장
        customerManager.curCustomer.CompleteOrder(true);

        // EnemyCount 초기화합니다.
        enemyManager.UpEnemyCount(-enemyManager.EnemyCount);

        // 보상을 증가시킵니다.
        UpdateReward();
    }
}
