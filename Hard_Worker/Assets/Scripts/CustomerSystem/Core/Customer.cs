using UnityEngine;

/// <summary>
/// 손님 주문 데이터 스크립트입니다.
/// </summary>
public class Customer : MonoBehaviour
{
    public FoodData foodData;
    public bool isOrderComplete = false; // 주문 완료 여부
    public float moveSpeed = 10f;

    public Vector2 targetPos; // 목표 위치
    public Vector2 exitPos; // 퇴장 위치
}
