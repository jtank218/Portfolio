using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {
	
	public float range = 0;
	public Vector3 velocity;
	
	private Vector3 startPosition = Vector3.zero;
	private Vector3 prevPosition = Vector3.zero;
	private
	// Use this for initialization
	void Start () {
		startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		prevPosition = transform.position;
		this.transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + range * Mathf.Sin(Time.time));
		velocity = (transform.position - prevPosition) / Time.deltaTime;
	}
}
