using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSimulation : MonoBehaviour {

	int gridSize = 50;

	public WindSpwaner windSpawner;


	float minInitPressure = -3.0f;
	float maxInitPressure = 3.0f;

	//bigger value means bigger impact of pressure differences on wind direction
	float pressureImpact = 0.001f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float pressureBalancingSpeed = 1.001f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float windBalancing = 1.005f;
	//a value of 100 would mean that the pressure has a chance of 1 in 100 to increase suddenly
	int chanceForBigPressure = 100;
	//a value of 100 would mean that the pressure has a chance of 1 in 100 to decrease suddenly
	int chanceForLowPressure = 1000;
	float averageBigPressure = 5f;
	float averageLowPressure = -5f;
	float variabilityBigPressure = 2f;
	float variabilityLowPressure = 2f;
	float coriolisAngle = 20f;

	WindCell[,] windCells;
	GameObject[,] windGrid;

	// Use this for initialization
	void Start () {
		windCells = new WindCell[gridSize, gridSize];
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				this.windCells [x, y] = new WindCell(
					new Vector2(Random.Range (-1, 1), Random.Range (-1, 1)),
					Random.Range (this.minInitPressure, this.maxInitPressure)
				);
			}
		}
		this.windGrid = this.windSpawner.MakeWindSpawn (gridSize);
	}

	// Update is called once per frame
	void Update () {
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				this.windCells[x,y] = Interchange (this.windCells [x,y], windCells [Mathf.Max(0,x-1),y], new Vector2 (-1, 0));
				this.windCells[x,y] = Interchange (this.windCells [x,y], windCells [Mathf.Min(gridSize-1,x+1),y], new Vector2 (1, 0));
				this.windCells[x,y] = Interchange (this.windCells [x,y], windCells [x,Mathf.Max(0, y-1)], new Vector2 (0, -1));
				this.windCells[x,y] = Interchange (this.windCells [x,y], windCells [x,Mathf.Min(gridSize-1, y+1)], new Vector2 (0, 1));

				this.windGrid [x, y].GetComponent<WindForce> ().direction = this.windCells [x, y].direction;
			}
		}
	}


	WindCell Interchange(WindCell currentCell, WindCell neighborCell, Vector2 positionDifference) {
		Vector2 newDirection = adaptWindDirectionToLowPressure (
			currentCell.direction,
			currentCell.pressure,
			neighborCell.pressure,
			positionDifference
		);
		newDirection = adaptDirectionToNeighborDirection (newDirection, neighborCell.direction);
		//newDirection = adaptDirectionToCoriolisAngle (newDirection);
		float newPressure = adaptPressure (currentCell.pressure, neighborCell.pressure);
		return new WindCell (newDirection, newPressure);
	}

	Vector2 adaptWindDirectionToLowPressure(
		Vector2 direction,
		float pressure,
		float pressureNeighbor,
		Vector2 positionDifference
	) {
		float pressureDifference = pressure - pressureNeighbor;
		return direction +  (pressureDifference * positionDifference * this.pressureImpact);
	}

	Vector2 adaptDirectionToNeighborDirection(Vector2 direction, Vector2 directionNeighbor) {
		return (1 / this.windBalancing) * direction +
			((this.windBalancing -1) / this.windBalancing) 
			* (Vector2)(Quaternion.AngleAxis (coriolisAngle, Vector3.forward) * directionNeighbor);
	}

	Vector2 adaptDirectionToCoriolisAngle(Vector2 direction) {
		return Quaternion.AngleAxis (coriolisAngle, Vector3.forward) * direction;
	}

	float adaptPressure(float pressure, float pressureNeighbor) {
		if (Random.Range (0, this.chanceForBigPressure) >= this.chanceForBigPressure-1) {
			return this.averageBigPressure + Random.Range (-variabilityBigPressure, variabilityBigPressure);
		}

		if (Random.Range (0, this.chanceForLowPressure) >= this.chanceForLowPressure-1) {
			return this.averageLowPressure + Random.Range (-variabilityLowPressure, variabilityLowPressure);
		}

		float averagePressure = (pressure + pressureNeighbor) / 2;
		return (1 / this.pressureBalancingSpeed) * pressure +
			((this.pressureBalancingSpeed -1) / this.pressureBalancingSpeed) * averagePressure;
	}


	public class WindCell {
		public Vector2 direction { get; set; }
		public float pressure { get; set; }

		public WindCell(Vector2 direction, float pressure)
		{
			this.direction = direction;
			this.pressure = pressure;
		}
	}
}

