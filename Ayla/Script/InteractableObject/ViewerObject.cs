using UnityEngine;
public enum ViewerUIType
{
    Safe,
    Diary,
	Pendant
}

public class ViewerObject : InteractableObject
{
    [SerializeField] private ViewerUIType viewerType;


	public ViewerUIType ViewerType { get { return viewerType; } }

	protected override void Interact()
	{
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

		if (hit.collider != null && hit.collider.gameObject == this.gameObject)
		{
			UIManager.Instance.ShowViewer(viewerType);
		}

	}
}
