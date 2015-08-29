﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int rows = 32;
	public int columns = 32;
	public Count coinCount = new Count (4, 10);
	public Count blockingCount = new Count (5, 20);
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	//need game objects for collectible items obstacles etc
	public GameObject[] coins;
	public GameObject[] blocks;

	private List <Vector3> gridPositions = new List<Vector3> ();

	private GameObject[,] rooms;

	void InitializeList () {
		gridPositions.Clear ();
		for (int x = 1; x < columns - 1; x ++) {
			for(int y = 1; y < rows - 1; y ++){
				gridPositions.Add(new Vector3 (x, y, 0f));
			}
		}
	}

	void RoomSetup () {
		GameObject room = new GameObject ("Room");
		this.rooms [0, 0] = room;
		Transform roomHolder = room.transform;

		for (int x = 0; x < columns; x ++) {
			for(int y = 0; y < rows; y ++){
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				if ((x == 0 || x == columns - 1 || y == 0 || y == rows - 1) && x != 15 && y != 15) {
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				float width = this.columns;
				float height = this.rows;

				float tileWidth = 1;
				float tileHeight = 1;

				GameObject instance = Instantiate (toInstantiate, new Vector3(x + tileWidth / 2 - width / 2, y + tileHeight / 2 - height / 2, 0f),Quaternion.identity) as GameObject;
				instance.transform.SetParent (roomHolder);
			}
		}
	}

	Vector3 RandomPosition () {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}

	}

	public void SetupRoom () {
		this.rooms = new GameObject[1, 1];
		RoomSetup ();
		InitializeList ();

		//this is where we would call LayoutObjectAtRandom, we don't have any health, items, weapons, etc. yet
		//we do have an item though so...

		LayoutObjectAtRandom (coins, coinCount.minimum, coinCount.maximum);
		LayoutObjectAtRandom (blocks, blockingCount.minimum, blockingCount.maximum);

	}

	public GameObject GetRoom (int x, int y)
	{
		return this.rooms [x, y];
	}
	
}
