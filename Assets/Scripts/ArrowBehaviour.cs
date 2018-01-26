using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

	public WindForce wf;

	private Vector3 direction3D = new Vector3();
	// Use this for initialization
	void Start () {
		wf = GetComponentInParent<WindForce> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		/*direction3D.z = wf.direction.x;
		direction3D.y = wf.direction.y;
		transform.rotation = Quaternion.LookRotation (direction3D);*/

		//var dir = WorldPos - transform.position;
		var angle = Mathf.Atan2(wf.direction.y, wf.direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
