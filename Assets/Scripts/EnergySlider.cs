using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySlider : MonoBehaviour {


	public GameObject energyManager;
	SolarPanelManager eM;
	Slider uiSlider;
	// Use this for initialization
	void Start () {
		//GameObject.FindGameObjectsWithTag ("SolarPanel").Length;
		int x = SolarPanelEnergy.maxEnergyProd*GameObject.FindGameObjectsWithTag ("SolarPanel").Length;
		uiSlider = GetComponent<Slider> ();
		uiSlider.maxValue = x;
		eM = energyManager.GetComponent<SolarPanelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		uiSlider.value = eM.totalEnergyProd;
	}
}
