using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
	[SerializeField] protected LayerMask layerMask;
	[SerializeField] protected GameObject outline;
	[SerializeField] protected bool isHidden = false;
	protected Player player;


	protected virtual void Start()
	{
		outline.SetActive(false);
	}

	protected virtual void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Interact();
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Player>(out player))
		{
			ShowOutline();
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Player>(out player))
		{
			HideOutline();
			player = null;
		}
	}

	protected void ShowOutline()
	{
		outline.SetActive(true);
	}

	protected void HideOutline()
	{
		outline.SetActive(false);
	}

	protected abstract void Interact();
}
