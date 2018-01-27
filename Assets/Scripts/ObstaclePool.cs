using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour {

	public float minSecondsBetweenSpawning = 2.0f;
	public float maxSecondsBetweenSpawning = 6.0f;
	private float savedTime;
	private float secondsBetweenSpawning;
	private int randNum;


	public GameObject obstacle;

	public int pooledAmount = 15;
	List<GameObject> obstacles;


	// Use this for initialization
	void Start () 
	{
		obstacles = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (obstacle);
			obj.SetActive (false);
			obstacles.Add (obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - savedTime >= secondsBetweenSpawning) // is it time to spawn again?
		{
			MakeThingToSpawn ();
			savedTime = Time.time; // store for next spawn
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
		}
	}

	void MakeThingToSpawn ()
	{
		for (int i = 0; i < obstacles.Count; i++) {
			if (!obstacles [i].activeInHierarchy) {
				obstacles [i].transform.position = transform.position;
				obstacles [i].transform.rotation = transform.rotation;
				obstacles [i].SetActive (true);
				break;
			}
		}
	}
}
