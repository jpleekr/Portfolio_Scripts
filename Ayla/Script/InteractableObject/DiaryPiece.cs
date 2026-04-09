using UnityEngine;

public class DiaryPiece : InteractableObject
{
	[SerializeField] private int PieceIndex;

	[Header("레이어 설정")]
	[SerializeField] private int DefaultLayer = 3;
	[SerializeField] private int HiddenLayer = 1;

	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetIsHiden(bool hiden) { isHidden = hiden; }

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);

		if(collision.TryGetComponent<MovebleObject>(out var movebleObject))
		{
			isHidden = true;
			spriteRenderer.sortingLayerID = HiddenLayer;
		}
	}

	protected override void OnTriggerExit2D(Collider2D collision)
	{
		base.OnTriggerExit2D(collision);

		if (collision.TryGetComponent<MovebleObject>(out var movebleObject))
		{
			isHidden = false;
			spriteRenderer.sortingLayerID = DefaultLayer;
		}
	}

	protected override void Interact()
	{
		if (isHidden == true) return;

		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, layerMask);

		if (hit.collider != null && hit.collider.gameObject == this.gameObject)
		{
			UIManager.Instance.ShowViewer(ViewerUIType.Diary);
			UIManager.Instance.GetDiaryUI().UnlockDiaryPiece(PieceIndex);
			Destroy(this.gameObject);
		}
	}
	
}
