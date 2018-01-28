using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpwaner : MonoBehaviour {

	public GameObject windPrefab;
	/**
	 * Scale of 1 = default gridSize x gridSize
	 */
	public float gridScale=5f;
	private int defaultGridSizeX = 19;
	private int defaultGridSizeY = 13;
	GameObject[,] windGrid;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public GameObject[,] MakeWindSpawn()
	{

		int gridWidth = (int)(gridScale * defaultGridSizeX);
		int gridHeight= (int)(gridScale*defaultGridSizeY);

		float worldScreenHeight = Camera.main.orthographicSize * 2.6f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		this.windGrid = new GameObject[gridWidth, gridHeight];
		Vector2 arrowSize = windPrefab.GetComponent<BoxCollider2D> ().size/gridScale;
		float x = GetComponent<Transform> ().position.x;
		float y = GetComponent<Transform> ().position.y;

		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				Vector2 newSpawnPos = new Vector2 (
					x  + arrowSize.x * i,
					y + arrowSize.y*j
				);
				GameObject wind = Instantiate (windPrefab, newSpawnPos, transform.rotation) as GameObject;
				wind.transform.parent = GameObject.Find ("Winds").transform;
				wind.transform.localScale = arrowSize;
				//wind.transform.parent = GetComponent<Transform> ().parent;
				this.windGrid [i, j] = wind;
			}
		}
		return this.windGrid;
	}
}
