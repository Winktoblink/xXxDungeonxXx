using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	private Transform mBoardHolder;     //An object to hold everything on the board for clean up
	private List<Vector3> mGridPositions = new List<Vector3>();

	private Map mBoardMap;

	//Initialize a list of positions to track if something is in a certain square
	void InitializeList() {
		mGridPositions.Clear();

		//For every column and row, initialize a new grid position
		for(int x = 0; x < mBoardMap.getWidth(); x++) {
			for(int y = 0; y < mBoardMap.getHeight(); y++) {
				mGridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	//Sets up background of the gameboard
	void BoardSetup() {
		mBoardHolder = new GameObject("Board").transform;

		readMapFile ("Maps/emptyMap");

		if (mBoardMap != null) {
			int[][] grid = mBoardMap.getGrid();
			for (int x = 0; x < mBoardMap.getWidth(); x++) {
				for (int y = 0; y < mBoardMap.getHeight(); y++) {
					GameObject toInstantiate;
					if (grid [x] [y] == 0) {
						toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
					} else {
						toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
					}

					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (mBoardHolder);
				}
			}
		}
	}

	private void readMapFile(String filePath) {
		TextAsset mapJson = (TextAsset) Resources.Load<TextAsset>(filePath);
		mBoardMap = JsonUtility.FromJson<Map>(mapJson.text);
	}

	public void SetupScene(int levelNumber) {
		BoardSetup();
		InitializeList();

		//TODO: Add functions for adding player, enemies, items, etc to map
	}

	//TODO: Update the Json layout as Unity's JSON deserializer does not support arrays.
	[Serializable]
	public class Map{
		private int mWidth;
		private int mHeight;
		private int[][] mGrid;

		public int getWidth(){
			return mWidth;
		}
		public int getHeight(){
			return mHeight;
		}
		public int[][] getGrid(){
			return mGrid;
		}
	}
}
