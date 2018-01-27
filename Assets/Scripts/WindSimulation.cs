using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSimulation : MonoBehaviour {

	public WindSpwaner windSpawner;

	WindCell[,] windCells;
	GameObject[,] windGrid;
	PressureCell[] pressureCells;


	int gridSize = 50;
	int numberPressureCells = 10;

	int warmUpSteps = 200;
	float minInitDirection = -1f;
	float maxInitDirection = 1f;

	float minPressure = -20.0f;
	float maxPressure = 20f;

	//bigger value means bigger impact of pressure differences on wind direction
	float pressureImpact = 0.001f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float pressureBalancingSpeed = 1.001f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float windBalancing = 1.005f;
	float coriolisAngle = 6f;

	float minSpeedPressureCell = 0.1f;
	float maxSpeedPressureCell = 1f;
	float minPressureCellSpeedToAdd = -0.001f;
	float maxPressureCellSpeedToAdd = 0.001f;

	float minPressureToAdd = -0.1f;
	float maxPressureToAdd = 0.1f;



	// Use this for initialization
	void Start () {
		initializeWindGrid ();
		initializePressureCells ();
		warmUp ();
	}

	public void initializeWindGrid() {
		windCells = new WindCell[gridSize, gridSize];
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				windCells [x, y] = new WindCell(
					new Vector2(Random.Range (minInitDirection, maxInitDirection),Random.Range (minInitDirection, maxInitDirection)),
					Random.Range (minPressure, maxPressure)
				);
			}
		}
		windGrid = windSpawner.MakeWindSpawn (gridSize);
	}

	void initializePressureCells() {
		pressureCells = new PressureCell[numberPressureCells];
		for (int i = 0; i < numberPressureCells; i++) {
			pressureCells[i] = new PressureCell(
				new Vector2(Random.Range(0, gridSize-1), Random.Range(0, gridSize-1)),
				new Vector2(
					Random.Range(minSpeedPressureCell, maxSpeedPressureCell),
					Random.Range(minSpeedPressureCell, maxSpeedPressureCell)
				),
				Random.Range(minPressureToAdd, maxPressureToAdd)
			);
		}
	}

	 void warmUp() {
		//Do some simulation steps to get the simulation into a typical state
		for (int i = 0; i < warmUpSteps; i++) {
			doSimulationStep ();
		}
	}

	// Update is called once per frame
	void Update () {
		doSimulationStep ();
	}

	void doSimulationStep() {
		calculateWindCellInteraction ();
		calculatePressureCells ();
	}

	void calculateWindCellInteraction() {
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				windCells[x,y] = Interchange (windCells [x,y], windCells [Mathf.Max(0,x-1),y], new Vector2 (-1, 0));
				windCells[x,y] = Interchange (windCells [x,y], windCells [Mathf.Min(gridSize-1,x+1),y], new Vector2 (1, 0));
				windCells[x,y] = Interchange (windCells [x,y], windCells [x,Mathf.Max(0, y-1)], new Vector2 (0, -1));
				windCells[x,y] = Interchange (windCells [x,y], windCells [x,Mathf.Min(gridSize-1, y+1)], new Vector2 (0, 1));
					
				windGrid [x, y].GetComponent<WindForce> ().direction = windCells [x, y].direction;
			}
		}
	}

	void calculatePressureCells() {
		foreach(PressureCell pressureCell in pressureCells) {
			int nearestX = (int)Mathf.Round (pressureCell.position.x) - 1;
			int nearestY = (int)Mathf.Round (pressureCell.position.y) - 1;

			windCells [nearestX, nearestY].pressure += pressureCell.pressureToAdd;
			windCells [nearestX, nearestY].pressure = Mathf.Max (minPressure, windCells [nearestX, nearestY].pressure);
			windCells [nearestX, nearestY].pressure = Mathf.Min (maxPressure, windCells [nearestX, nearestY].pressure);

			pressureCell.position += pressureCell.direction;
			pressureCell.position.x = Mathf.Max (0, pressureCell.position.x);
			pressureCell.position.x = Mathf.Min (gridSize, pressureCell.position.x);
			pressureCell.position.y = Mathf.Max (0, pressureCell.position.y);
			pressureCell.position.y = Mathf.Min (gridSize, pressureCell.position.y);

			pressureCell.direction += new Vector2 (
				Random.Range (minPressureCellSpeedToAdd, maxPressureCellSpeedToAdd),
				Random.Range (minPressureCellSpeedToAdd, maxPressureCellSpeedToAdd)
			);
			pressureCell.direction.x = Mathf.Max (minSpeedPressureCell, pressureCell.direction.x);
			pressureCell.direction.x = Mathf.Min (maxSpeedPressureCell, pressureCell.direction.x);
			pressureCell.direction.y = Mathf.Max (minSpeedPressureCell, pressureCell.direction.y);
			pressureCell.direction.y = Mathf.Min (maxSpeedPressureCell, pressureCell.direction.y);
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
			* (Vector2)(
				
				Quaternion.AngleAxis (coriolisAngle, Vector3.forward) *

				directionNeighbor);
	}

	Vector2 adaptDirectionToCoriolisAngle(Vector2 direction) {
		return Quaternion.AngleAxis (coriolisAngle, Vector3.forward) * direction;
	}

	float adaptPressure(float pressure, float pressureNeighbor) {
		
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

	public class PressureCell {
		public Vector2 position;
		public Vector2 direction;
		public float pressureToAdd;

		public PressureCell(Vector2 position, Vector2 direction, float pressureToAdd)
		{
			this.position = position;
			this.direction = direction;
			this.pressureToAdd = pressureToAdd;
		}
	}
}

