using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanelEnergy : MonoBehaviour {

	public static int maxEnergyProd=10;

	int energyProd =maxEnergyProd;
	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {


	}

	public int energyProduction
	{
		get {return energyProd; }
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Cloud" && isCloudCenterTouching(coll)) {
			energyProd = 0;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		energyProd = maxEnergyProd;
	}

	bool isCloudCenterTouching(Collider2D coll)
	{
		return this.gameObject.GetComponent<PolygonCollider2D> ().bounds.Contains (coll.GetComponent<Rigidbody2D> ().transform.position);
	}
}
