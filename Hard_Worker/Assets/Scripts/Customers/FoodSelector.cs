using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 스테이지에 따라 난이도 기반으로 음식 주문을 랜덤하게 선택하는 클래스입니다.
/// - 손님에게 랜덤한 요리를 부여하고, 관련 UI와 보상 설정을 수행합니다.
/// </summary>
public class FoodSelector : MonoBehaviour
{
    private CustomerManager manager;
    private CustomerUI customerUI;

    private void Start()
    {
        manager = GetComponent<CustomerManager>();
        customerUI = GetComponent<CustomerUI>();
    }

    /// <summary>
    /// 스테이지 난이도에 따라 랜덤한 음식 주문을 생성합니다.
    /// - 난이도 낮은 음식은 자주, 높은 음식은 드물게 등장합니다.
    /// - 선택된 음식은 손님과 UI에 반영되며, 기본 보상도 설정됩니다.
    /// </summary>
    public void RamdomOrder()
    {
        // 스테이지가 올라갈 수록 난이도가 높은 음식이 나오게
        List<FoodData> probabilityFoods = new List<FoodData>();

        foreach (var food in manager.foods)
        {
            int stage = StageManager.Instance.Stage;

            if (food.Difficulty <= stage + 1)
            {
                // 난이도가 높은 음식일 수록 더 적게 나오게
                int weight = Mathf.Clamp((stage + 1) - food.Difficulty + 1, 1, 10);// 최대치 제한

                for (int i = 0; i < weight; i++)
                {
                    probabilityFoods.Add(food);
                }
            }
        }
        // 음식이 없다면 전체 중에서 랜덤
        if (probabilityFoods.Count == 0)
        {
            probabilityFoods.AddRange(manager.foods);
        }

        int value = Random.Range(0, probabilityFoods.Count);
        FoodData _food = probabilityFoods[value];
        manager.food = _food;
        manager.curCustomer.foodData = _food;
        // 음식 이미지 표시
        customerUI.ShowOrderImage(_food);

        // 기본 골드 보상 설정
        StageManager.Instance.SetBaseReward(_food.Difficulty * 100);
    }
}
