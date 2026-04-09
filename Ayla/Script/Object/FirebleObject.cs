using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirebleObject : MonoBehaviour
{
    private Animator anim;
    private Light2D lightCompo;

	[SerializeField] private GameObject FireObject;

	[SerializeField] private bool defaultFire;
	[SerializeField] private float luminosity;

	private bool isFire;
	private bool playerInRange;

	private void Start()
	{
		InitComponent();
		InitFire();
	}

	private void Update()
	{
		FideInLight();

		if (playerInRange && Input.GetKeyDown(KeyCode.L))
		{
			if(isFire)
			{
				FireOff();
			}
			else
			{
				FireOn();
			}
		}

	}

	private void InitComponent()
	{
		anim = GetComponentInChildren<Animator>();
		lightCompo = GetComponentInChildren<Light2D>();
	}

	private void InitFire()
	{
		isFire = defaultFire;
		lightCompo.intensity = 0;
		FireObject.SetActive(defaultFire);
	}
	private void FideInLight()
	{
		if(isFire)
		{
			lightCompo.intensity = Mathf.Lerp(lightCompo.intensity, luminosity, Time.deltaTime);
		}
	}

	public void FireOn()
    {
		isFire = true;
		FireObject.SetActive(isFire);
		//anim.SetBool("Fire", isFire);
	}

	public void FireOff()
	{
		isFire = false;
		//anim.SetBool("Fire", isFire);
		lightCompo.intensity = 0f;
		FireObject.SetActive(isFire);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerInRange = false;
		}
	}
}
