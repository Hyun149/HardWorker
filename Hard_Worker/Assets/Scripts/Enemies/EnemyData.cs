using UnityEngine;

/// <summary>
/// 적(음식 재료들)의 데이터를 담는 스크립트 입니다.
/// </summary>
[CreateAssetMenu(fileName ="EnemyData", menuName ="Data/EnemyData")]
public class EnemyData : ScriptableObject
{
   [SerializeField] private string enemyName; 
   [SerializeField] private Sprite enemyImage;

   [SerializeField] private bool isBoss;

    public string EnemyName => enemyName;
    public Sprite EnemyImage => enemyImage;
    public bool IsBoss => isBoss;
}
