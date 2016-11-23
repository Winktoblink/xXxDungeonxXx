using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public int columns;
	public int rows;

	private Transform mBoardHolder;     //An object to hold everything on the board for clean up
	private List<Vector3> mGridPositions = new List<Vector3>();

	private Map mBoardMap;

	//Initialize a list of positions to track if something is in a certain square
	void InitializeList() {
		mGridPositions.Clear();

		for(int x = 0; x < columns; x++) {
			for(int y = 0; y < rows; y++) {
				mGridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup() {
		mBoardHolder = new GameObject("Board").transform;

		readMapFile ("Maps/emptyMap.json");
	}

	private void readMapFile(String filePath) {
		TextAsset asset = (TextAsset) Resources.Load(filePath, typeof(TextAsset));
		String json = asset.text;
		mBoardMap = JsonUtility.FromJson<Map>(json);
		Debug.Log (mBoardMap);
	}

	public void SetupScene(int floorNumber) {
		BoardSetup();
		InitializeList();

		//TODO: Add functions for adding player, enemies, items, etc to map
	}

	[Serializable]
	public class Map{
		private int mWidth;
		private int mHeight;
		private int[][] mGrid;
	}
}
