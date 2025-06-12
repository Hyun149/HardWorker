using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName ="Data/EnemyData")]
public class EnemyData : ScriptableObject
{
   [SerializeField] private string enemyName;
   [SerializeField] private float curProgress; // 현재 진행도
    [SerializeField] private float maxProgress; // 최대 진행도
    [SerializeField] private Sprite ingredient; // 재료 이미지
   [SerializeField] private bool isBoss;
   public GameObject effectPrefab;

    public string EnemyName => enemyName;
    public float CurProgress => curProgress;
    public float MaxProgress => maxProgress;
    public Sprite Ingredient => ingredient;
    public bool IsBoss => isBoss;

    public void SetProgress(float progress)
    {
        curProgress = progress;
    }
}
