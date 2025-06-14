using UnityEngine;

/// <summary>
/// 손님의 애니메이션을 관리하는 스크립트입니다.
/// </summary>
public class CustomerAni : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Idle()
    {
        animator.SetBool("isWalk", false);
    }
    public void Walk()
    {
        animator.SetBool("isWalk", true);
    }
}
