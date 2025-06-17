using UnityEngine;

/// <summary>
/// 손님의 애니메이션을 제어하는 클래스입니다.
/// - 손님이 걷거나 멈추는 애니메이션을 관리합니다.
/// </summary>
public class CustomerAni : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// 오브젝트가 활성화될 때 Animator 컴포넌트를 캐싱합니다.
    /// </summary>
    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 손님을 대기 상태(Idle) 애니메이션으로 전환합니다.
    /// </summary>
    public void Idle()
    {
        animator.SetBool("isWalk", false);
    }

    /// <summary>
    /// 손님을 걷는 상태(Walk) 애니메이션으로 전환합니다.
    /// </summary>
    public void Walk()
    {
        animator.SetBool("isWalk", true);
    }
}
