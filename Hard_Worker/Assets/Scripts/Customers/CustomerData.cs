using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData")]
public class CustomerData : ScriptableObject
{
    [SerializeField] private string customerName;
    [SerializeField] private List<FoodData> foods = new List<FoodData>();
    [SerializeField] private int reward;

    public string CustomerName => customerName;
    public List<FoodData> Foods => foods;
    public int Reward => reward;

}
