﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolarPanelManager : MonoBehaviour {

	private GameObject[] panels;
	public int totalEnergyProd;

	public int optimalEnergy = 10;
	public int pointsGained = 0;
	public float neededPointsGain =1000f;
	public float maxPointGainPerTimeslice = 10;
	public float minPointsGain = -10f;

	public Slider optimalSlider;
	//public Slider pointsSlider;
	public float nextCollect = 0.0f;
	public float collectCoolDown = 0.1f;
	public Timer timer;
	private AudioSource audioSource;
	 

	// Use this for initialization
	void Start () {
		panels = GameObject.FindGameObjectsWithTag ("SolarPanel");
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		calcTotalEnergy ();

		if(Time.time > nextCollect && pointsGained < neededPointsGain)
		{
			float diff = Mathf.Abs (optimalEnergy - totalEnergyProd);
			float minDifference = 0f;
			float maxDifference = optimalSlider.maxValue;


			float newGain = maxPointGainPerTimeslice + ((minPointsGain - maxPointGainPerTimeslice) / (maxDifference - minDifference)) * (diff - minDifference);

				
			pointsGained = (int) Mathf.Max(0,pointsGained + Mathf.Pow(newGain, 3));

			if (newGain < -8 && !audioSource.isPlaying) {
				audioSource.Play ();
			} else if(newGain > 0) {
				audioSource.Stop ();
			}

			nextCollect = Time.time + collectCoolDown;
		}
		checkForEnd ();
	}

	void checkForEnd() {
		if (pointsGained >= neededPointsGain) {
			timer.stop = true;
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
