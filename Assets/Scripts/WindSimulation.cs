using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSimulation : MonoBehaviour {

	public WindSpwaner windSpawner;

	WindCell[,] windCells;
	GameObject[,] windGrid;
	PressureCell[] pressureCells;


	int gridSizeX;
	int gridSizeY;
	int numberPressureCells = 9;

	int warmUpSteps = 0;
	float minInitDirection = -1f;
	float maxInitDirection = 1f;

	float minPressure = -60.0f;
	float maxPressure = 60f;

	//bigger value means bigger impact of pressure differences on wind direction
	float pressureImpact = 0.004f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float pressureBalancingSpeed = 1.15f;
	//should be somewhere between 1 (no balancing) and 2 (balancing by taking average)
	float windBalancing = 1.005f;
	float coriolisAngle = 12f;
	float windInfluenceOnPressureCell = 2f;

	float minSpeedPressureCell = -0.1f;
	float maxSpeedPressureCell = 0.1f;
	float minPressureCellSpeedToAdd = -0.01f;
	float maxPressureCellSpeedToAdd = 0.01f;

	float minPressureToAdd = -14.1f;
	float maxPressureToAdd = 14.1f;

	float bounceForce = 1f;

	float powerRegression = 1.001f;

	// Use this for initialization
	void Start () {
		initializeWindGrid ();
		initializePressureCells ();
		warmUp ();
	}

	public void initializeWindGrid() {
		windGrid = windSpawner.MakeWindSpawn ();

		gridSizeX = windGrid.GetLength (0);
		gridSizeY = windGrid.GetLength (1);

		windCells = new WindCell[gridSizeX, gridSizeY];
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				windCells [x, y] = new WindCell(
					new Vector2(Random.Range (minInitDirection, maxInitDirection),Random.Range (minInitDirection, maxInitDirection)),
					Random.Range (minPressure, maxPressure)
				);
			}
		}
	}

	void initializePressureCells() {
		pressureCells = new PressureCell[numberPressureCells];
		for (int i = 0; i < numberPressureCells; i++) {
			pressureCells[i] = new PressureCell(
				new Vector2(Random.Range(0, gridSizeX-2), Random.Range(0, gridSizeY-2)),
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
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				windCells[x,y] = Interchange (windCells [x,y], windCells [Mathf.Max(0,x-1),y], new Vector2 (-1, 0));
				windCells[x,y] = Interchange (windCells [x,y], windCells [Mathf.Min(gridSizeX-1,x+1),y], new Vector2 (1, 0));
				windCells[x,y] = Interchange (windCells [x,y], windCells [x,Mathf.Max(0, y-1)], new Vector2 (0, -1));
				windCells[x,y] = Interchange (windCells [x,y], windCells [x,Mathf.Min(gridSizeY-1, y+1)], new Vector2 (0, 1));
				windGrid [x, y].GetComponent<WindForce> ().direction = windCells [x, y].direction;

				float normalizedPressure = windCells [x, y].pressure + maxPressure / Mathf.Abs (minPressure - maxPressure);
				Color newColor = new Color (normalizedPressure, normalizedPressure, normalizedPressure);
				windGrid [x, y].GetComponentInChildren<SpriteRenderer> ().color = newColor;
			}
		}
	}

	void calculatePressureCells() {
		foreach(PressureCell pressureCell in pressureCells) {
			int nearestX = (int)Mathf.Round (pressureCell.position.x);
			int nearestY = (int)Mathf.Round (pressureCell.position.y);

			windCells [nearestX, nearestY].pressure += pressureCell.pressureToAdd;
			//windCells [nearestX, nearestY].pressure = maxPressure;
			windCells [nearestX, nearestY].pressure = Mathf.Max (minPressure, windCells [nearestX, nearestY].pressure);
			windCells [nearestX, nearestY].pressure = Mathf.Min (maxPressure, windCells [nearestX, nearestY].pressure);

			pressureCell.position += pressureCell.direction;
			//pressureCell.position += windInfluenceOnPressureCell * windCells [nearestX, nearestY].direction;
			pressureCell.position.x = Mathf.Max (0, pressureCell.position.x);
			pressureCell.position.x = Mathf.Min (gridSizeX - 1, pressureCell.position.x);
			pressureCell.position.y = Mathf.Max (0, pressureCell.position.y);
			pressureCell.position.y = Mathf.Min (gridSizeY - 1, pressureCell.position.y);

			pressureCell.direction += new Vector2 (
				Random.Range (minPressureCellSpeedToAdd, maxPressureCellSpeedToAdd),
				Random.Range (minPressureCellSpeedToAdd, maxPressureCellSpeedToAdd)
			);
			pressureCell.direction.x = Mathf.Max (minSpeedPressureCell, pressureCell.direction.x);
			pressureCell.direction.x = Mathf.Min (maxSpeedPressureCell, pressureCell.direction.x);
			pressureCell.direction.y = Mathf.Max (minSpeedPressureCell, pressureCell.direction.y);
			pressureCell.direction.y = Mathf.Min (maxSpeedPressureCell, pressureCell.direction.y);

			if (pressureCell.position.x <= 0) {
				pressureCell.direction.x += bounceForce;
			}
			if (pressureCell.position.x >= gridSizeX - 1) {
				pressureCell.direction.x -= bounceForce;
			}
			if (pressureCell.position.y <= 0) {
				pressureCell.direction.y += bounceForce;
			}

			if (pressureCell.position.y >= gridSizeY - 1) {
				pressureCell.direction.y -= bounceForce;
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
			* (Vector2)(
				
				Quaternion.AngleAxis (coriolisAngle, Vector3.forward) *

				directionNeighbor);
	}

	Vector2 adaptDirectionToCoriolisAngle(Vector2 direction) {
		return Quaternion.AngleAxis (coriolisAngle, Vector3.forward) * direction;
	}

	float adaptPressure(float pressure, float pressureNeighbor) {
		
		float averagePressure = (pressure + pressureNeighbor) / 2;
		averagePressure = (1 / this.pressureBalancingSpeed) * pressure +
			((this.pressureBalancingSpeed -1) / this.pressureBalancingSpeed) * averagePressure;

		averagePressure -= Mathf.Sign (averagePressure)
			* Mathf.Pow(((maxPressure - Mathf.Abs (averagePressure)) / 10000), powerRegression);
		return averagePressure;
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

