﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpwaner : MonoBehaviour {

	public GameObject windPrefab;
	/**
	 * Scale of 1 = default gridSize x gridSize
	 */
	public float gridScale=1f;

	GameObject[,] windGrid;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public GameObject[,] MakeWindSpawn(int gridSize)
	{
		this.windGrid = new GameObject[gridSize, gridSize];

		float gridWidth = gridScale*gridSize, gridHeight= gridScale*gridSize;
		Vector2 arrowSize = windPrefab.GetComponent<BoxCollider2D> ().size/gridScale;
		Debug.Log (arrowSize.x + " " + arrowSize.y);

		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				Vector2 newSpawnPos = new Vector2 (arrowSize.x * i, arrowSize.y*j);
				GameObject wind = Instantiate (windPrefab, newSpawnPos, transform.rotation) as GameObject;
				wind.transform.parent = GameObject.Find ("Winds").transform;
				wind.transform.localScale = arrowSize;
				this.windGrid [i, j] = wind;
			}
		}
		return this.windGrid;
	}
}
