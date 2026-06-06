using UnityEngine;

/// <summary>
/// MonoBehaviour 기반 싱글톤 베이스 클래스
/// 상속받은 클래스는 게임 내에서 하나만 존재하도록 보장한다.
/// </summary>
public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>

    /// <summary>
    /// 씬 전환 시 파괴 여부
    /// false : DontDestroyOnLoad 적용
    /// true : 씬 전환 시 파괴
    /// </summary>
    protected bool m_IsDestroyOnLoad = false;

    /// <summary>
    /// 싱글톤 인스턴스
    /// </summary>
    protected static T m_Instance;

    /// <summary>
    /// 외부 접근용 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            // 아직 생성되지 않은 경우 경고 출력
            if (m_Instance == null)
            {
                Debug.LogWarning($"{typeof(T).Name} has not been initialized yet.");
            }
            return m_Instance;
        }
    }
    
    /// <summary>
    /// Unity 생명주기 Awake
    /// 싱글톤 초기화 수행
    /// </summary>
    protected virtual void Awake()
    {
        Init();
    }

    /// <summary>
    /// 싱글톤 초기화
    /// 중복 객체 제거 및 인스턴스 등록
    /// </summary>
    protected virtual void Init()
    {
        // 이미 다른 인스턴스가 존재하면 자신 제거
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 현재 객체를 싱글톤으로 등록
        m_Instance = (T)this;

        // 씬 전환 시 유지
        if (!m_IsDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 객체 파괴 시 인스턴스 정리
    /// </summary>
    protected virtual void OnDestroy()
    {
        // 자신이 현재 싱글톤일 때만 제거
        if (m_Instance == this)
            m_Instance = null;
    }

    /// <summary>
    /// 수동 해제용 함수
    /// 상속 클래스에서 추가 정리 작업 가능
    /// </summary>
    protected virtual void Dispose()
    {
        m_Instance = null;
    }
}
