using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarScript : MonoBehaviour {

	public GameObject energyManager;
	SolarPanelManager eM;
	Slider uiSlider;

	// Use this for initialization
	void Start () {
		uiSlider = GetComponent<Slider> ();
		eM = energyManager.GetComponent<SolarPanelManager> ();
		uiSlider.maxValue = eM.neededPointsGain;

	}

	// Update is called once per frame
	void Update () {
		uiSlider.value = eM.pointsGained;
	}
}
