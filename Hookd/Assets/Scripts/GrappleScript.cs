using UnityEngine;
using System.Collections;

public class GrappleScript : MonoBehaviour 
{
	public GameObject grappleBullet;
	public int grappleBulletSpeed = 0;
	public float pullSpeed = 0;
	public Texture2D crosshairTexture;
	
	private GameObject grappleHook;
	private GameObject firedGrapple;
	private GameObject grappledObject;
	
	private Vector3 grappleDisplacement;
	private Vector3 grapplePoint;
	
	private bool isGrappling = false;
	private Rect position;
	
	private Vector3 originalScale;
	private Quaternion originalRotate;
	private Transform originalTransform;
	
	// Use this for initialization
	void Start () 
	{
		grappleHook = GameObject.Find("Grapple Hook");
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - 
        	crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
		grappleHook.transform.LookAt(Camera.mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10.0f)));
		
		originalScale = this.transform.localScale;
		originalRotate = this.transform.rotation;
		originalTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			FireGrapple();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			isGrappling = false;
			this.GetComponent<CharacterMotor>().movement.gravity = 20;
			this.transform.parent = null;
			this.GetComponent<CharacterMotor>().canControl = true;
			
			if (isGrappling)
				if (grappledObject.GetComponent<PlatformScript>() == null)
					this.GetComponent<CharacterMotor>().movement.velocity += grappledObject.GetComponent<PlatformScript>().velocity;
			
		}
		if (isGrappling == true)
		{
			if (Vector3.Distance(this.transform.position, (grappledObject.transform.position + grapplePoint)) <= 3.0f)
			{
				
				grappleDisplacement = (grappledObject.transform.position + grapplePoint) - (this.transform.position + this.GetComponent<CharacterController>().center);
				this.GetComponent<CharacterMotor>().movement.velocity = Vector3.zero;
				isGrappling = false;
			}
			else
			{
				this.GetComponent<CharacterMotor>().movement.velocity = grappleDisplacement.normalized * pullSpeed;
			}
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 200, 25), "Increase Fire Speed"))
		{
			grappleBulletSpeed = (int)Mathf.Min(50.0f, grappleBulletSpeed + 2);
		}
		if (GUI.Button(new Rect(0, 25, 200, 25), "Decrease Fire Speed"))
		{
			grappleBulletSpeed = (int)Mathf.Max(0.0f, grappleBulletSpeed - 2);
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 50, 0, 100, 25), "Next Level"))
		{
			LoadNextLevel();	
		}
		GUI.Box (new Rect(Screen.width - 150, 0, 190, 25), "Level " + (Application.loadedLevel + 1).ToString() + ": " + Application.loadedLevelName);
		GUI.DrawTexture(position, crosshairTexture);
	}
	
	void FireGrapple()
	{
		if (firedGrapple != null)
		{
			Destroy(firedGrapple);	
		}
		firedGrapple = (GameObject)Instantiate(grappleBullet, grappleHook.transform.position, Quaternion.identity);
		firedGrapple.transform.forward = grappleHook.transform.forward;
		firedGrapple.rigidbody.velocity = grappleHook.transform.forward * grappleBulletSpeed;
		
	}
	
	public void BeginGrapple(Vector3 aPoint, GameObject aGameObject)
	{
		grappledObject = aGameObject;
		grapplePoint = aPoint - aGameObject.transform.position;
		grappleDisplacement = aPoint - (this.transform.position + this.GetComponent<CharacterController>().center);
		Destroy(firedGrapple);	
		isGrappling = true;
		
		this.GetComponent<CharacterMotor>().movement.gravity = 0;
		this.GetComponent<CharacterMotor>().canControl = false;
		this.GetComponent<CharacterMotor>().grounded = false;
							
		if (aGameObject.GetComponent<PlatformScript>() != null)
		{
			this.transform.parent = aGameObject.transform.FindChild("Center");
		}
		else
			this.transform.parent = null;
	}
	
	public void DestroyGrappleBullet()
	{
		Destroy(firedGrapple);	
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Goal")
		{
			LoadNextLevel();
		}
		else if (collider.gameObject.tag == "Death")
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}	
	
	void LoadNextLevel()
	{
		if (Application.loadedLevel + 1 >= Application.levelCount)
			Application.LoadLevel(0);
		else
			Application.LoadLevel(Application.loadedLevel + 1);
	}
}
