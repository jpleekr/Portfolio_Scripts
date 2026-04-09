using UnityEngine;

public class MovebleObject : MonoBehaviour
{
	private Rigidbody2D rb;
	private Collider2D col;

	[SerializeField] private float moveSpeed = 3f;

	private void Awake()
	{
		InitComponent();		
	}
	private void Start()
	{
		FreezeObject(true);
	}

	private void InitComponent()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
	}

	public void FreezeObject(bool isFreezed)//True이면 Rigidbody를 얼리고 false이면 Rigidbody를 푼다
	{
		if (isFreezed)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public void SetTrigger(bool _isTrigger)
	{
		if(_isTrigger)
			col.isTrigger = _isTrigger;
		else 
			col.isTrigger = _isTrigger;
	}

	public void MoveObject(float moveDir)//moveDir방향으로 오브젝트를 움직인다
	{
		transform.position += new Vector3(moveSpeed * moveDir * Time.deltaTime, 0, 0);
	}
}
