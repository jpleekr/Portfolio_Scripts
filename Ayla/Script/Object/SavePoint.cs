using UnityEngine;

public class SavePoint : BaseTrigger
{
	private bool isActivated = false;
    protected override void OnPlayerEnter()
	{
		SetPoint();
	}
	private void SetPoint()
	{
		if (isActivated) return;
		isActivated = true;
		GameManager.Instance.SetSavePoint(transform.position);
	}
}
