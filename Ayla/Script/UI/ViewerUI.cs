using UnityEngine;
using UnityEngine.UI;

public abstract  class ViewerUI : MonoBehaviour
{
	 protected virtual  void Start()
	{
		
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			HideUI();
		}
	}

	public abstract void HideUI();
}
