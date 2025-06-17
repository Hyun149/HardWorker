using UnityEngine;

/// <summary>
/// 적 애니메이션을 관리할 스크립트입니다.
/// </summary>
public class EnemyAni : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 재료를 칼로 자르는 애니메이션을 재생합니다.
    /// </summary>
    public void Cut(int value)
    {
        animator.SetInteger("cut",value);
    }
}
