using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolarPanelManager : MonoBehaviour {

	public GameObject[] panels;
	public int totalEnergyProd;

	public int optimalEnergy = 10;
	public int pointsGained = 0;
	public float neededPointsGain =1000f;
	public float maxPointGainPerTimeslice = 10;

	public Slider optimalSlider;
	//public Slider pointsSlider;
	public float nextCollect = 0.0f; // this is if you wish to allow projectile from                      //the beginning of the game it self.   
	public float collectCoolDown = 1.0f; // if the cooldown is say 5 secs
	 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		calcTotalEnergy ();

		if(Time.time > nextCollect)
		{
			float diff = Mathf.Abs (optimalEnergy - totalEnergyProd);
			Debug.Log ("difference: "+diff);
			float minDifference = 0f;
			float maxDifference = optimalSlider.maxValue;
			float minPointsGain = 1f;

			float newGain = maxPointGainPerTimeslice + ((minPointsGain - maxPointGainPerTimeslice) / (maxDifference - minDifference)) * (diff - minDifference);

			Debug.Log ("new Gain: "+newGain);
				
			pointsGained += (int)newGain;

			nextCollect = Time.time + collectCoolDown;
		}
		
	}

	void calcTotalEnergy()
	{
		totalEnergyProd = 0;

		for (int i = 0; i < panels.Length; i++) {
			totalEnergyProd += panels [i].GetComponent<SolarPanelEnergy>().energyProduction;
		}
	}

}
