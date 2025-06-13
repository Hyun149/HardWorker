using UnityEngine;

/// <summary>
/// <b>MonoBehaviour 기반의 범용 싱글톤 클래스</b><br/>
/// T 타입을 상속받은 클래스는 자동으로 인스턴스를 보장받습니다.
/// - GameObject가 없으면 자동 생성됩니다.
/// - 기본적으로 DontDestroyOnLoad가 적용되어 씬 전환 시에도 유지됩니다.
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;                     // 싱글톤 인스턴스. 처음 접근 시 자동 생성되거나 기존 인스턴스를 반환합니다.
    private static bool isShuttingDown = false;    // 종료 시점에 싱글톤 접근을 방지하기 위한 플래그입니다.
    private static object lockObj = new object();  // 멀티스레드 환경에서의 안전한 인스턴스 생성을 위한 락 객체입니다.

    /// <summary>
    /// 전역 접근 가능한 인스턴스
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShuttingDown) return null;  // 애플리케이션 종료 중이라면 null 반환

            lock (lockObj)
            {
                if (instance == null) // 인스턴스가 없으면 찾거나 생성
                {
                    instance = FindAnyObjectByType<T>();

                    if (instance == null)
                    {
                        // 없으면 새 GameObject를 생성하고 컴포넌트를 추가
                        var singletonObj = new GameObject(typeof(T).Name);
                        instance = singletonObj.AddComponent<T>();

                        // 씬이 바뀌어도 유지되도록 설정
                        DontDestroyOnLoad(singletonObj);
                    }
                }

                return instance;
            }
        }
    }

    /// <summary>
    /// 인스턴스 초기화 및 중복 방지를 위한 Awake 처리입니다.<br/>
    /// DontDestroyOnLoad를 기본 적용합니다.
    /// </summary
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 애플리케이션이 종료될 때 호출되어,
    /// 싱글톤 접근을 방지하는 플래그를 설정합니다.
    /// </summary>
    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    /// <summary>
    /// 싱글톤 오브젝트가 파괴될 때, 인스턴스가 자신이라면 종료 플래그를 설정합니다.
    /// </summary>
    private void OnDestroy()
    {
        if (instance == this)
        {
            isShuttingDown = true;
        }
    }
}
