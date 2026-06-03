using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

/// <summary>
/// 플레이어 주변에 날씨 파티클을 생성하고 관리하는 클래스
/// 플레이어 이동 시 파티클을 재배치하여
/// 항상 플레이어 주변에 날씨 효과가 보이도록 유지한다.
/// </summary>
public class WeatherParticleController : MonoBehaviour
{
    [Header("Weather Particle")]
    [SerializeField] private GameObject weatherParticle; // 생성할 날씨 파티클 프리팹

    [Header("Settings")]
    [SerializeField] private int particleCount = 3;          // 유지할 파티클 개수
    [SerializeField] private float particleHeightOffset = 9f; // 플레이어 머리 위 생성 높이

    private PlayerControll player; // 플레이어 참조

    // 생성된 파티클 오브젝트
    private GameObject[] particles;

    // 성능 최적화를 위해 미리 캐싱한 컴포넌트
    private VisualEffect[] visualEffects;
    private ParticleSystem[] particleSystems;

    // 다음에 재배치할 파티클 인덱스
    private int currentIndex;

    private void Start()
    {
        player = PlayerManager.Instance.PlayerController;

        // 파티클 생성 및 초기화
        InitWeatherParticle();
    }

    private void OnEnable()
    {
        // 인게임 씬에서만 파티클 재생
        if (SceneManager.GetActiveScene().name == "In_Game_Scene")
        {
            PlayParticle();
        }
        else
        {
            StopParticle();
        }
    }

    private void OnDisable()
    {
        // 오브젝트 비활성화 시 모든 파티클 정지
        StopParticle();
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거 영역을 벗어나면
        // 가장 오래된 파티클을 플레이어 위치로 이동
        if (!other.CompareTag("Player"))
            return;

        RepositionParticle(currentIndex);
    }

    /// <summary>
    /// 플레이어 머리 위 기준 위치 반환
    /// </summary>
    private Vector3 GetPlayerWeatherPosition()
    {
        return player.transform.position + Vector3.up * particleHeightOffset;
    }

    /// <summary>
    /// 파티클 생성 및 필요한 컴포넌트 캐싱
    /// </summary>
    private void InitWeatherParticle()
    {
        particles = new GameObject[particleCount];
        visualEffects = new VisualEffect[particleCount];
        particleSystems = new ParticleSystem[particleCount];

        Vector3 spawnPos = GetPlayerWeatherPosition();

        for (int i = 0; i < particleCount; i++)
        {
            // 파티클 생성
            particles[i] = Instantiate(
                weatherParticle,
                spawnPos,
                Quaternion.identity);

            // 자주 사용하는 컴포넌트 캐싱
            // 이후 GetComponent 호출을 줄여 성능 개선
            visualEffects[i] = particles[i].GetComponent<VisualEffect>();
            particleSystems[i] = particles[i].GetComponent<ParticleSystem>();
        }
    }

    /// <summary>
    /// 특정 파티클을 플레이어 위치로 이동
    /// 순환 방식으로 파티클을 재사용하여
    /// 넓은 영역의 날씨 효과를 연출
    /// </summary>
    private void RepositionParticle(int index)
    {
        particles[index].transform.position = GetPlayerWeatherPosition();

        // 다음 파티클 선택
        currentIndex++;

        // 마지막 인덱스에 도달하면 처음으로 순환
        if (currentIndex >= particles.Length)
        {
            currentIndex = 0;
        }
    }

    /// <summary>
    /// 모든 파티클 재생
    /// Visual Effect Graph와 Particle System 모두 지원
    /// </summary>
    private void PlayParticle()
    {
        Vector3 playerPos = GetPlayerWeatherPosition();

        for (int i = 0; i < particles.Length; i++)
        {
            // VFX Graph 사용 시
            if (visualEffects[i] != null)
            {
                visualEffects[i].Play();
            }
            // 일반 Particle System 사용 시
            else if (particleSystems[i] != null)
            {
                particleSystems[i].Play();
            }

            // 플레이어 위치 기준으로 정렬
            particles[i].transform.position = playerPos;
        }
    }

    /// <summary>
    /// 모든 파티클 정지
    /// </summary>
    private void StopParticle()
    {
        // 아직 초기화되지 않은 경우 종료
        if (particles == null)
            return;

        for (int i = 0; i < particles.Length; i++)
        {
            // VFX Graph 정지
            if (visualEffects[i] != null)
            {
                visualEffects[i].Stop();
            }
            // 일반 Particle System 정지
            else if (particleSystems[i] != null)
            {
                particleSystems[i].Stop();
            }
        }
    }
}
