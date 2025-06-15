using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 음식 주문과 관련된 정보를 표시하는 스크립트 입니다.
/// </summary>
public class CustomerUI : MonoBehaviour
{
    CustomerManager manager;
    public TMP_Text stageText; // 현재 스테이지 텍스트

    public Image foodImage; // 주문한 음식 이미지
    public TMP_Text foodText; // 주문한 음식 텍스트

    public Image[] enemyInfos = new Image[4];
    public Image[] enemyInfoImages = new Image[4];

    private void Awake()
    {
        manager = GetComponent<CustomerManager>();

        if (StageManager.Instance != null)
        {
            StageManager.Instance.onStageChanged -= ShowStageText;
            StageManager.Instance.onStageChanged += ShowStageText;
            ShowStageText(StageManager.Instance.Stage + 1);
        }   
    }
    /// <summary>
    /// 현재 스테이지 상태를 보여줍니다.
    /// </summary>
    /// <param name="stage"></param>
    public void ShowStageText(int stage)
    {
        stageText.text ="Stage "+ stage.ToString();
    }
    /// <summary>
    /// 주문한 음식의 정보를 보여줍니다.
    /// </summary>
    public void ShowOrderImage(FoodData food)
    {
        foodImage.gameObject.SetActive(true);
        foodImage.sprite = Instantiate(food.FoodImage, GameObject.Find("Canvas").transform);
        foodText.text = food.FoodName;

        TurnOffEnemyInfo();

        for (int i = 0; i < manager.food.Enemys.Count; i++)
        {
            // 재료 개수만큼 UI 표시
            enemyInfos[i].gameObject.SetActive(true);
            enemyInfoImages[i].sprite = manager.food.Enemys[i].GetComponent<Enemy>().enemyData.EnemyImage;
        }
    }
    /// <summary>
    /// 재료 UI 끄기
    /// </summary>
    void TurnOffEnemyInfo()
    {
        for (int i = 0; i < enemyInfos.Length; i++)
        {
            // 재료 개수만큼 UI 비활성화
            enemyInfos[i].gameObject.SetActive(false);   
        }
    }
}
