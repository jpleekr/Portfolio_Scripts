using UnityEngine;

public class Door : InteractableObject
{
	private CamConfinerChanger camConfinerChanger;

	[SerializeField] private Door Exit;
	[SerializeField] private bool isPassable = true;

	public void SetIsPassable(bool passable) { isPassable = passable; }

	protected override void Start()
	{
		base.Start();
		camConfinerChanger = GetComponent<CamConfinerChanger>();
	}

	public void SetExit(Door door)
	{
		Exit = door;
	}

	public void EnterRoom()
	{
		if (isPassable == false) return;

		UIManager.Instance.FadeOut();
		player.transform.position = new Vector3( Exit.transform.position.x, Exit.transform.position.y - 4.0f, Exit.transform.position.z);
		camConfinerChanger.ChangeCam(player.gameObject);
		UIManager.Instance.FadeIn();
	}

	protected override void Interact()
	{
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

		if (hit.collider != null && hit.collider.gameObject == this.gameObject)
		{
			EnterRoom();
		}

	}
}
