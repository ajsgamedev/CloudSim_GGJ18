using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

	public GameObject cloudPrefab;

	public float nextCloud = 0.0f; // this is if you wish to allow projectile from                      //the beginning of the game it self.   
	public float cloudCoolDown = 2.0f; // if the cooldown is say 5 secs

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.FindGameObjectsWithTag ("Cloud").Length < 10000) {


			if (Input.GetMouseButtonDown (0) && Time.time > nextCloud) {
				float cloudOffset = 0.2f;
				float cloudOffsetNear = 0.16f;
				createCloud (new Vector2 (0, 0));
				createCloud (new Vector2 (cloudOffset, 0));
				createCloud (new Vector2 (0, cloudOffset));
				createCloud (new Vector2 (-cloudOffset, 0));
				createCloud (new Vector2 (0, -cloudOffset));
				createCloud (new Vector2 (cloudOffsetNear, cloudOffsetNear));
				createCloud (new Vector2 (-cloudOffsetNear, -cloudOffsetNear));
				createCloud (new Vector2 (-cloudOffsetNear, cloudOffsetNear));
				createCloud (new Vector2 (cloudOffsetNear, -cloudOffsetNear));
				nextCloud = Time.time + cloudCoolDown;
			}
		}
	}

	void createCloud(Vector2 offsetFromMouse) {
		Vector3 mousePos = Input.mousePosition;
		Vector3 objectPos = Camera.main.ScreenToWorldPoint (mousePos);
		objectPos.x += offsetFromMouse.x;
		objectPos.y += offsetFromMouse.y;
		objectPos.z = 0.0f;   
		GameObject cloudObject = Instantiate (cloudPrefab, objectPos, transform.rotation) as GameObject;
		cloudObject.transform.parent = GameObject.Find ("Clouds").transform;
	}

}
