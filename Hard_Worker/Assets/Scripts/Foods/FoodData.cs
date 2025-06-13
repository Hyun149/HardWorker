using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FoodData", menuName ="Data/FoodData")]
public class FoodData : ScriptableObject
{
    [SerializeField] private string foodName;
    [SerializeField] private Sprite foodImage;

    public string FoodName => foodName;
    public Sprite FoodImage => foodImage;
}
