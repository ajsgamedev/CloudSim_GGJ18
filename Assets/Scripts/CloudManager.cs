using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public GameObject cloudPrefab;

	public float nextCloud = 0.0f; // this is if you wish to allow projectile from                      //the beginning of the game it self.   
	public float cloudCoolDown = 2.0f; // if the cooldown is say 5 secs

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.FindGameObjectsWithTag ("Cloud").Length < 10) {


			if (Input.GetMouseButtonDown (0) && Time.time > nextCloud) {
				nextCloud = Time.time + cloudCoolDown;
				Vector3 mousePos = Input.mousePosition;
				Vector3 objectPos = Camera.main.ScreenToWorldPoint (mousePos);
				objectPos.z = 0.0f;   
				GameObject cloudObject = Instantiate (cloudPrefab, objectPos, transform.rotation) as GameObject;
				cloudObject.transform.parent = GameObject.Find ("CloudsManager").transform;
				nextCloud = Time.time + cloudCoolDown;
			}
		}
	}
}
