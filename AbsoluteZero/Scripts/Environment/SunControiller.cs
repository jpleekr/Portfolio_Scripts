using UnityEngine;

/// <summary>
/// 게임 시간에 따라 태양의 방향을 회전시키는 클래스
/// </summary>
public class SunController  : MonoBehaviour
{
	[Header("Sun Settings")]
	[SerializeField] private Light sunLight; // 현재 씬의 Directional Light(태양)

	// 00:00(자정)일 때 태양의 시작 각도
	// -90도면 지평선 아래에서 시작
	private const  float sunBaseAngle = -90f; 


	void Update()
    {
        SunRotate();
	}

	/// <summary>
    /// 게임 시간에 따라 태양 회전
    /// 24시간 = 360도 회전
    /// </summary>
    private void SunRotate()
    {
		// 현재 시간을 분 단위로 변환
		// 예) 13시 30분 = 810분
		float totalMinute = TimeManager.Instance.gameHour * 60 + TimeManager.Instance.gameMinute;

		// 하루 진행도 계산 (0 ~ 1)
		// 0 = 00:00
		// 0.5 = 12:00
		// 1 = 24:00
		float dayProgress = totalMinute / 1440f; 

		// 0~360도 범위에서 회전, 기준 각도에서 시작
		float sunAngle = sunBaseAngle + (dayProgress * 360f);

		// 태양을 X축을 중심으로 회전시킴 (동쪽에서 떠서 남쪽을 지나 서쪽으로 지는 구조)
		sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
	}
}
