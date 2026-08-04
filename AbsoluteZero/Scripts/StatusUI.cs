using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
	// 플레이어 상태 UI
	[SerializeField] private Image uiPlayerHpBar;
	[SerializeField] private Image uiPlayerHungerBar;
	[SerializeField] private Image uiPlayerTirstBar;
	[SerializeField] private Image uiPlayerMentalityBar;
	[SerializeField] private Image uiPlayerColdBar;

	void Update()
	{
		// 플레이어 상태에 맞게 UI 갱신
		UpdateUI();
	}

	private void UpdateUI()
	{
		// PlayerStatusManager가 아직 생성되지 않았으면 종료
		if (PlayerStatusManager.Instance == null) return;

		// 각 스탯의 비율을 UI에 반영
		uiPlayerHpBar.fillAmount = PlayerStatusManager.Instance.CurrentHpPercent;
		uiPlayerHungerBar.fillAmount = PlayerStatusManager.Instance.CurrentHungerPercent;
		uiPlayerTirstBar.fillAmount = PlayerStatusManager.Instance.CurrentThirstPercent;
		uiPlayerMentalityBar.fillAmount = PlayerStatusManager.Instance.CurrentMentalityPercent;
		uiPlayerColdBar.fillAmount = PlayerStatusManager.Instance.CurrentColdPercent;
	}
}
