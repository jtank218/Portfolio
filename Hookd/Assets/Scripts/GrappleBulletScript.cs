using UnityEngine;
using System.Collections;

public class GrappleBulletScript : MonoBehaviour {
	
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Can grapple")
			player.GetComponent<GrappleScript>().BeginGrapple(collision.contacts[0].point, collision.gameObject);
		else
			player.GetComponent<GrappleScript>().DestroyGrappleBullet();
	}
}
