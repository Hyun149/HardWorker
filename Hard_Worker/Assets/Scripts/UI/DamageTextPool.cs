using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 재료 공격 데미지 텍스트 Pool 스크립트입니다.
/// 텍스트를 생성하고 Pool을 관리합니다.
/// </summary>
public class DamageTextPool : MonoBehaviour
{
    public Transform canvasTransform;
    public Transform damageTexts; // damageText 부모
    public GameObject damageTextPrefab;
    public int poolSize = 10; // Pool 사이즈
    private Queue<DamageText> damageTextPool = new Queue<DamageText>();

    private void Start()
    {
        CreateDamageText();
    }
    /// <summary>
    /// poolSize의 수만큼 텍스트 생성
    /// </summary>
    public void CreateDamageText()
    {
        for (int i = 0; i < poolSize; i++)
        {
            InstantiateText();
        }
    }
    /// <summary>
    /// Pool 확장
    /// </summary>
    /// <returns></returns>
    public DamageText Get()
    {
        if (damageTextPool.Count == 0)
        {
            // 풀 부족 시 새로 생성
            InstantiateText();
        }
        return damageTextPool.Dequeue();
    }
    /// <summary>
    /// Pool 반납
    /// </summary>
    /// <returns></returns>
    public void Return(DamageText dt)
    {
        dt.gameObject.SetActive(false);
        damageTextPool.Enqueue(dt);
    }
    /// <summary>
    /// 텍스트 생성
    /// </summary>
    /// <returns></returns>
    void InstantiateText()
    {
        GameObject obj = Instantiate(damageTextPrefab, canvasTransform);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(damageTexts);
        
        damageTextPool.Enqueue(obj.GetComponent<DamageText>());
    }
}
