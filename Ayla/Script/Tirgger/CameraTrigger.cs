using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cam;

	private CinemachineCamera vcam;
	private CinemachinePositionComposer positionComposer;
	private CinemachineConfiner2D confiner;

	[Header("ScreenPosition")]
	[SerializeField] private Vector2 cameraScreenPos;
	[SerializeField] private Vector2 damping;//카메라를 더 부드럽고 자연스럽게 이동 값이 높을수록 느려지고 부드러워짐

	[Header("BoundingShape")]
	[SerializeField] private Collider2D boundingShape;
	[SerializeField] private bool isTeleport;

    private void Start()
	{
		InitComponent();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
            SetScreenPosition();
			SetBoundingShape(collision);
		}
	}

	private void InitComponent()
	{
        vcam = cam.GetComponent<CinemachineCamera>();
        positionComposer =vcam.TryGetComponent<CinemachinePositionComposer>(out var comp) ? comp : null;
		confiner = vcam.GetComponent<CinemachineConfiner2D>();
    }

	private void SetScreenPosition()
	{
		positionComposer.Composition.ScreenPosition = cameraScreenPos;
		positionComposer.Damping = damping;
	}

	private void SetBoundingShape(Collider2D collision)
	{
		//confiner.enabled = false;

		//confiner.BoundingShape2D = boundingShape;
		//confiner.InvalidateBoundingShapeCache();


		//Vector3 clampedPos = boundingShape.ClosestPoint(collision.transform.position);
		//vcam.ForceCameraPosition(clampedPos, Quaternion.identity);

		//confiner.enabled = true;
		if (isTeleport)
		{
			confiner.enabled = false;

			confiner.BoundingShape2D = boundingShape;
			confiner.InvalidateBoundingShapeCache();

			Vector3 clampedPos = boundingShape.ClosestPoint(collision.transform.position);
			vcam.ForceCameraPosition(clampedPos, Quaternion.identity);

            confiner.enabled = true;
        }
        else
		{
			confiner.BoundingShape2D = boundingShape;
		}
	}
}
