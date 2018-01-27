using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public GameObject cloudPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			
			Vector3 mousePos = Input.mousePosition;
			Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
			objectPos.z = 0.0f;   
			GameObject cloudObject = Instantiate (cloudPrefab, objectPos, transform.rotation) as GameObject;
			cloudObject.transform.parent = GameObject.Find ("Clouds").transform;

		}
	}
}
