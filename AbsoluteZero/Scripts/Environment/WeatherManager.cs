using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 내 날씨 종류
/// </summary>
public enum WeatherType
{
	Sunny,   // 맑음
	Cloudy,  // 흐림
	Foggy,   // 안개
	Snowy    // 눈내림
}

public class WeatherManager : SingletonBehaviour<WeatherManager>
{
    [SerializeField] private WeatherType currentWeatherType;  //현재 날씨

    [SerializeField] private int weatherDuration;             // 날씨 지속 시간(분)
    private int weatherStartHour;                             // 날씨 시작 시각(시)
    private int weatherStartMinute;                           // 날씨 시작 시각(분)
    private int weatherCurrentHour;                           // 날씨 경과 시간(시)
    private int weatherCurrentMinute;                         // 날씨 경과 시간(분)

    private Light lightCompo;                                 // 태양(Direction Light)
    private PlayerControll player;

    private float currentIntensity;                           // 현재 목표 광원 밝기

    [SerializeField] private GameObject fogPrefab;            // 안개 프리팹
    [SerializeField] private GameObject snowPrefab;           // 눈 프리팹
    private GameObject fog;                                   // 생성된 안개 오브젝트
    private GameObject snow;                                  // 생성된 눈 오브젝트

    [SerializeField] private Material sunnyAfternoonSkyBox;   // 맑은 낮 하늘
    [SerializeField] private Material sunnyNightSkyBox;       // 맑은 밤 하늘
    [SerializeField] private Material CloduyAfternoonSkyBox;  // 흐린 낮 하늘
    [SerializeField] private Material CloduyNightSkyBox;      // 흐린 밤 하늘

    [SerializeField] private float sunnyIntensity;           // 맑을 때 광원 세기
    [SerializeField] private float cloudyIntensity;          // 흐릴 때 광원 세기

    void Start()
	{
		InitWeather();
		SetWeather();
	}

	void Update()
    {
        UpdateWeather();
        DebugWeather();
    }

	/// <summary>
    /// 날씨 시스템 초기화
    /// </summary>
	private void InitWeather()
	{
		player = PlayerManager.Instance.PlayerController;
        lightCompo = RenderSettings.sun;

		fog = Instantiate(fogPrefab);
        snow = Instantiate(snowPrefab);

		fog.gameObject.SetActive(false);
		snow.gameObject.SetActive(false);
	}

	/// <summary>
    /// 랜덤으로 새로운 날씨를 설정
    /// </summary>
    private void SetWeather()
    {
        int weather = Random.Range(0, 10);
        int time = Random.Range(60, 721);    // 1시간 ~ 12시간

        weatherCurrentHour = 0;
        weatherCurrentMinute = 0;

   		// 날씨 확률 설정
        // 맑음 50%
        if (weather < 5)
        {
            currentWeatherType = WeatherType.Sunny;
            currentIntensity = sunnyIntensity;
        }
		// 흐림 30%
        else if (weather < 8)
        {
            currentWeatherType = WeatherType.Cloudy;
            currentIntensity = cloudyIntensity;
        }
		// 안개 10%
        else if (weather < 9)
        {
            currentWeatherType = WeatherType.Foggy;
            currentIntensity = cloudyIntensity;
            SpawnParticle(currentWeatherType);
        }
		// 눈 10%
        else if(weather < 10)
        {
            currentWeatherType = WeatherType.Snowy;
            currentIntensity = cloudyIntensity;
            SpawnParticle(currentWeatherType);
        }

        SetSkyBox();

        weatherDuration = time;

		// 현재 시간을 시작 시간으로 저장
        weatherStartHour = TimeManager.Instance.gameHour;
        weatherStartMinute = TimeManager.Instance.gameMinute;
    }

	/// <summary>
    /// 날씨 지속 시간 체크 및 조명 갱신
    /// </summary>
    private void UpdateWeather()
    {
        weatherCurrentHour = TimeManager.Instance.gameHour - weatherStartHour;
        weatherCurrentMinute = TimeManager.Instance.gameMinute - weatherStartMinute;

		// 설정된 지속 시간을 넘으면 새로운 날씨 생성
        if(weatherDuration < weatherCurrentHour * 60 + weatherCurrentMinute)
        {
            DestroyParticle(currentWeatherType);           
            SetWeather();
            return;
        }

		// 게임 씬에서만 광원 밝기 변경
        if(SceneManager.GetActiveScene().name == "In_Game_Scene")
        {
            lightCompo.intensity = Mathf.Lerp(lightCompo.intensity, currentIntensity, Time.deltaTime * 0.5f);
        }
    }

	/// <summary>
    /// 현재 날씨와 시간에 맞는 스카이박스 적용
    /// </summary>
    private void SetSkyBox()
    {
		// 오전 9시 이후 = 낮
        if(TimeManager.Instance.gameHour > 9)
        {
            if(currentWeatherType == WeatherType.Sunny)
            {
                RenderSettings.skybox = sunnyAfternoonSkyBox;
            }
            else
            {
                RenderSettings.skybox = CloduyAfternoonSkyBox;
            }
        }
		// 밤
        else
        {
			if (currentWeatherType == WeatherType.Sunny)
			{
				RenderSettings.skybox = sunnyNightSkyBox;
			}
			else
			{
				RenderSettings.skybox = CloduyNightSkyBox;
			}
		}

		// 라이팅 환경 갱신
        DynamicGI.UpdateEnvironment();

	}

	/// <summary>
    /// 날씨에 맞는 파티클 활성화
    /// </summary>
    private void SpawnParticle(WeatherType weatherType)
    {
        switch(weatherType)
        {
            case WeatherType.Foggy:
                fog.gameObject.SetActive(true); 
                break;
            case WeatherType.Snowy:
                snow.gameObject.SetActive(true);
                break;
            default:
                break;
		}

    }

	/// <summary>
    /// 날씨 파티클 비활성화
    /// </summary>
    private void DestroyParticle(WeatherType weatherType)
    {
        switch(weatherType)
        {
            case WeatherType.Foggy:
                fog.gameObject.SetActive(false);
                break;
            case WeatherType.Snowy:
                snow.gameObject.SetActive(false);
                break;
        }
    }

	/// <summary>
    /// 디버그용 날씨 강제 변경
    /// F1 : 맑음
    /// F2 : 흐림
    /// F3 : 안개
    /// F4 : 눈
    /// </summary>
    private void DebugWeather()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            DestroyParticle(WeatherType.Foggy);
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Sunny;
			currentIntensity = sunnyIntensity;
		}
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            DestroyParticle(WeatherType.Foggy);
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Cloudy;
			currentIntensity = cloudyIntensity;
		}
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Foggy;
			currentIntensity = cloudyIntensity;
			SpawnParticle(currentWeatherType);
		}
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            DestroyParticle(WeatherType.Foggy);
			currentWeatherType = WeatherType.Snowy;
            currentIntensity = cloudyIntensity;
			SpawnParticle(currentWeatherType);
		}

        SetSkyBox();
    }
}
