using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SafeUI : ViewerUI
{
	[SerializeField] private SafeDoorAnimator SafeDoorAnimator;
	[SerializeField] private Button OkBtn;

	[SerializeField] private List<int> Password = new List<int>();
	private List<int> CurrentPassword = new List<int>();

	[SerializeField] float ShakeDuration = 0.15f;
	[SerializeField] float ShakeStrength = 10.0f;

	private RectTransform rect;
	private Vector2 originPos;

	private bool isClear = false;

	public List<int> GetCurrentPassword() { return CurrentPassword; }
	public void SetUpCurrentPassword(int index, int value) { CurrentPassword[index] = value; }
	public bool GetIsclear() { return isClear; }


	private void Awake()
	{
		InitCurrentPassword();
		rect = GetComponent<RectTransform>();
		originPos = rect.anchoredPosition;
	}

	private void InitCurrentPassword()
	{
		for (int i = 0; i < Password.Count; i++)
		{
			CurrentPassword.Add(0);
		}
	}

	public override void HideUI()
	{
		UIManager.Instance.HideViewer(ViewerUIType.Safe);
	}

	public void CheckPassword()
	{
		bool isSame = Password.SequenceEqual(CurrentPassword);

		if (isSame)
		{
			SuccessdRelease();
		}
		else
		{
			FailRelease();
		}
	}

	private void SuccessdRelease()
	{
		isClear = true;
		SafeDoorAnimator.PlayAnim();
		HideUI();
	}

	private void FailRelease()
	{
		Shake();
	}

	public void Shake()
	{
		StopAllCoroutines();
		StartCoroutine(ShakeX(ShakeDuration, ShakeStrength));
	}

	IEnumerator ShakeX(float duration, float strength)
	{
		float time = 0f;

		OkBtn.interactable = false;

		while (time < duration)
		{
			float x = Mathf.Sin(time * 40f) * strength;
			rect.anchoredPosition = originPos + new Vector2(x, 0);

			time += Time.deltaTime;
			yield return null;
		}

		rect.anchoredPosition = originPos;

		OkBtn.interactable = true;
	}
}
