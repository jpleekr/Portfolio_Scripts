using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class RockUI : MonoBehaviour
{
	[SerializeField] private List<Sprite> PasswordImages = new List<Sprite>();
	[SerializeField] private int PasswordIndex = 0;

	private Image RockImage;
	[SerializeField] private int passwordValue = 0;

	private void Awake()
	{
		RockImage = GetComponent<Image>();
		RockImage.sprite = PasswordImages[UIManager.Instance.GetSafeUI().GetCurrentPassword()[PasswordIndex]];
	}

	public void UpPassword()
	{

		if (passwordValue <= 0)
		{
			passwordValue = PasswordImages.Count - 1;
		}
		else
		{
			passwordValue--;
		}

		UIManager.Instance.GetSafeUI().SetUpCurrentPassword(PasswordIndex, passwordValue);
		RockImage.sprite = PasswordImages[passwordValue];
	}

	public void DownPassword()
	{
		if (passwordValue >= PasswordImages.Count - 1)
		{
			passwordValue = 0;
		}
		else
		{
			passwordValue++;
		}

		UIManager.Instance.GetSafeUI().SetUpCurrentPassword(PasswordIndex, passwordValue);
		RockImage.sprite = PasswordImages[passwordValue];
	}
}
