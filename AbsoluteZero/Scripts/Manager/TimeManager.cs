using UnityEngine;

/// <summary>
/// 게임 내 시간을 관리하는 싱글톤 매니저.
/// 게임 시간(일, 시, 분, 초)의 흐름을 관리하며,
/// 시간 추가, 일시정지, 디버그 기능을 제공한다.
/// </summary>
public class TimeManager : SingletonBehaviour<TimeManager>
{
	// 현재 게임 날짜
	public int gameDay { get; private set; } = 0;

	// 현재 게임 시간
	public int gameHour { get; private set; }

	// 현재 게임 분
	public int gameMinute { get; private set; }

	// 현재 게임 초
	public float gameSecond { get; private set; }

	/// <summary>
	/// 외부에서 읽기 전용으로 사용하는 시간 배율
	/// </summary>
	public float TimeScale { get { return timeScale; } }

	// 게임 시작 시간 설정
	[SerializeField] private int StartHour = 20;
	[SerializeField] private int StartMinute = 0;
	[SerializeField] private float StartSecond = 0f;

	// 현실 시간 1초당 게임 시간이 얼마나 흐를지 결정하는 배율
	[SerializeField] private float timeScale = 5f;

	// 게임 일시정지 여부
	[HideInInspector] public bool isPause;

	private void Start()
	{
		// 시작 시간 초기화
		InitTime();
	}

	private void Update()
	{
		// 매 프레임 게임 시간 갱신
		TimeUpdate();

		// 디버그 기능 실행
		DebugMode();
	}

	/// <summary>
	/// 게임 시간을 시작 시간으로 초기화한다.
	/// </summary>
	public void InitTime()
	{
		isPause = false;

		gameHour = StartHour;
		gameMinute = StartMinute;
		gameSecond = StartSecond;
	}

	/// <summary>
	/// 게임 시간을 진행시키고
	/// 초 → 분 → 시 → 일 순으로 시간을 변환한다.
	/// </summary>
	private void TimeUpdate()
	{
		// 게임 시간 증가
		gameSecond += Time.deltaTime * timeScale;

		// 생존 시간 기록
        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, Time.deltaTime * timeScale);

		// 초가 60초 이상이면 분으로 변환
		if (gameSecond > 60)
		{
			gameMinute += (int)(gameSecond / 60);
			gameSecond = gameSecond % 60;

			// 분이 60분 이상이면 시간으로 변환
			if (gameMinute > 60)
			{
				gameHour += gameMinute / 60;
				gameMinute = gameMinute % 60;

				// 시간이 24시간 이상이면 날짜 증가
				if (gameHour > 24)
				{
					gameDay += gameHour / 24;
					gameHour = gameHour % 24;
				}
			}
		}
	}

	/// <summary>
	/// 지정한 시간(hour)만큼 게임 시간을 증가시킨다.
	/// 주로 수면이나 이벤트 등에 사용된다.
	/// </summary>
	public void AddHour(int hour)
	{
		gameHour += hour;

		// 증가한 생존 시간 기록
        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, hour * 3600);

		// 날짜 변경 처리
		if (gameHour >= 24)
		{
			gameDay += gameHour / 24;
			gameHour = gameHour % 24;
		}
	}

	/// <summary>
	/// 지정한 분(minute)만큼 게임 시간을 증가시킨다.
	/// </summary>
	public void AddMinute(int minute)
	{
		gameMinute += minute;

		// 증가한 생존 시간 기록
        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, minute * 60);

		// 시간이 넘어가는 경우 처리
		if (gameMinute >= 60)
		{
			gameHour += gameMinute / 60;
			gameMinute = gameMinute % 60;

			// 날짜 변경 처리
			if (gameHour >= 24)
			{
				gameDay += gameHour / 24;
				gameHour = gameHour % 24;
			}
		}
	}

	/// <summary>
	/// 게임 일시정지 여부를 설정한다.
	/// Time.timeScale을 이용해 게임 전체를 멈추거나 재개한다.
	/// </summary>
	public void PauseGame(bool pause)
	{
		if (pause)
		{
			Time.timeScale = 0;
			isPause = pause;
		}
		else
		{
			Time.timeScale = 1;
			isPause = pause;
		}
	}

	/// <summary>
	/// 개발용 디버그 기능.
	/// PageUp 키를 누르면 게임 시간이 1시간 증가한다.
	/// </summary>
	private void DebugMode()
	{
		if (Input.GetKeyDown(KeyCode.PageUp))
			AddHour(1);
	}
}
