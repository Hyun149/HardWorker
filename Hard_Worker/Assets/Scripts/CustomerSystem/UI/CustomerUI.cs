using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 손님의 음식 주문 UI와 반응 효과를 표시하는 클래스입니다.
/// - 현재 스테이지, 음식 정보, 재료 이미지, 보상 팝업 등을 담당합니다.
/// </summary>
public class CustomerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText; // 현재 스테이지 텍스트

    [SerializeField] private Image foodImage; // 주문한 음식 이미지
    [SerializeField] private TMP_Text foodText; // 주문한 음식 텍스트

    [SerializeField] private Image[] enemyInfos = new Image[4];
    [SerializeField] private Image[] enemyInfoImages = new Image[4];

    [SerializeField] private GameObject heartEffectPrefab;
    [SerializeField] private GameObject rewardPopupPrefab;
    
    private CustomerManager manager;

    /// <summary>
    /// 시작 시 StageManager 이벤트를 구독하고 현재 스테이지를 표시합니다.
    /// </summary>
    private void Start()
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
    /// 현재 스테이지 번호를 UI에 표시합니다.
    /// </summary>
    /// <param name="stage">스테이지 번호</param>
    public void ShowStageText(int stage)
    {
        stageText.text ="Stage "+ stage.ToString();
    }

    /// <summary>
    /// 음식 주문 정보를 UI에 표시합니다.
    /// - 음식 이미지와 이름, 사용되는 재료 이미지들을 표시합니다.
    /// </summary>
    /// <param name="food">주문 음식 데이터</param>
    public void ShowOrderImage(FoodData food)
    {
        foodImage.gameObject.SetActive(true);
        foodImage.sprite = Instantiate(food.FoodImage, GameObject.Find("BackGroundCanvas").transform);
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
    /// 재료 UI를 모두 비활성화합니다.
    /// </summary>
    void TurnOffEnemyInfo()
    {
        for (int i = 0; i < enemyInfos.Length; i++)
        {
            // 재료 개수만큼 UI 비활성화
            enemyInfos[i].gameObject.SetActive(false);   
        }
    }

    /// <summary>
    /// 손님이 음식을 받고 만족하는 하트 이펙트를 표시합니다.
    /// </summary>
    public void ShowServeReaction()
    {
        Vector3 basePos = manager.curCustomer.transform.position + Vector3.up;

        //하트 이펙트
        GameObject heart = Instantiate(heartEffectPrefab, basePos + new Vector3(0, -0.3f, 0), Quaternion.identity);
        Destroy(heart, 1.5f);
        
    }

    /// <summary>
    /// 보상 획득 시 골드 증가 팝업을 띄웁니다.
    /// </summary>
    /// <param name="goldAmount">획득한 골드 수치</param>
    public void ShowRewardReaction(int goldAmount)
    {
        Vector3 spawnPos = manager.curCustomer.transform.position + Vector3.up * 1.2f;

        GameObject popup = Instantiate(rewardPopupPrefab, spawnPos, Quaternion.identity);

        // 텍스트 세팅
        TMPro.TextMeshPro text = popup.GetComponentInChildren<TMPro.TextMeshPro>();
        if (text != null)
            text.text = $"+{goldAmount}";

        // 위로 뜨는 애니메이션
        popup.transform.DOMoveY(spawnPos.y + 0.5f, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(popup, 0.2f));
    }
}
