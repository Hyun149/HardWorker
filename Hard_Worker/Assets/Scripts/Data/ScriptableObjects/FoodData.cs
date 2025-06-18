using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 음식 데이터
 /// </summary>
[CreateAssetMenu(fileName ="FoodData", menuName ="Data/FoodData")]
public class FoodData : ScriptableObject
{
    [SerializeField] private string foodName;
    [SerializeField] private Sprite foodImage;
    [SerializeField] private List<GameObject> enemys; // 필요 재료들

    public string FoodName => foodName;
    public Sprite FoodImage => foodImage;
    public List<GameObject> Enemys=> enemys;
    public int Difficulty => enemys.Count;
}
