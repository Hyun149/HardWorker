using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 음식 주문과 관련된 정보를 표시하는 스크립트 입니다.
/// </summary>
public class CustomerUI : MonoBehaviour
{
    public TMP_Text stageText; // 현재 스테이지 텍스트

    public Image foodImage; // 주문한 음식 이미지
    public TMP_Text foodText; // 주문한 음식 텍스트

    private void Start()
    {
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
    /// <param name="food"></param>
    public void ShowOrderImage(FoodData food)
    {
        foodImage.gameObject.SetActive(true);
        foodImage.sprite = Instantiate(food.FoodImage, GameObject.Find("Canvas").transform);
        foodText.text = food.FoodName;
    }
}
