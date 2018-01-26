using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour {

	public Vector2 direction;
	public int force;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
			
	}

	void OnTriggerStay2D(Collider2D coll) 
	{
		if (coll.gameObject.tag == "Cloud") {
			coll.gameObject.GetComponent<Rigidbody2D> ().AddForce (direction*force);
		}
	}
}
