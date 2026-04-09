using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBackGround : MonoBehaviour
{
    private Camera mainCam;

    [SerializeField] private float parallaxEffect;

    private float postionX;
    private float paddingX;


	void Start()
    {
        mainCam = Camera.main;

        paddingX = GetComponent<SpriteRenderer>().bounds.size.x;
        postionX = transform.position.x;
    }

    void Update()
    {
		float distanceMoved = mainCam.transform.position.x * (1 - parallaxEffect);
		float distanceToMove = mainCam.transform.position.x * parallaxEffect;

		transform.position = new Vector3(postionX + distanceToMove, transform.position.y);

		if (distanceMoved > postionX + (paddingX * 2))
			postionX += (paddingX * 3);
		else if (distanceMoved < postionX - (paddingX * 2))
			postionX -= (paddingX * 3);
	}
}
