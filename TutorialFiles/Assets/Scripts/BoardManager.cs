using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    //Class to hold tile count boundaries
    public class Count {
        public int min;
        public int max;

        public Count(int min, int max) {
            this.min = min;
            this.max = max;
        }
    }

    //Dimensions for a columnxrows gameboard
    public int columns = 8;
    public int rows = 8;

    //Object to define minimum and maximum tile counts
    public Count innerWallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    //Objects to hold the different prefabs
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    //Used to keep the hierarchy clean, acts as a parent to gameObjects
    private Transform mBoardHolder;
    
    //Used to track the different possible positions on the gameboard and if it is occupied.
    private List<Vector3> mGridPositions = new List<Vector3>();

    void initializeList() {
        mGridPositions.Clear();

        //set up loop to prevent obstacles on the outer ring
        //of the level so you can always reach the end.
        for(int x = 1; x < columns - 1; x++) {
            for(int y = 1; y < rows -1; y++) {
                mGridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Lays out border wall tiles and background floor tiles.
    void boardSetup() {
        mBoardHolder = new GameObject("Board").transform;

        for(int x = -1; x < columns + 1; x++) {
            for(int y = -1; y < rows + 1; y++) {

                GameObject toInstantiate;
                if(x == -1 || x == columns || y == -1 || y == rows) {
                    //If this condition is met we are on an outer tile, so create an outer wall tile from our list.
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                } else {
                    //Otherwise, we are on an inner tile, so create a floor tile from our list
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(mBoardHolder);
            }
        }
    }

    //Selects a random positions from all the grid positions
    Vector3 randomPosition() {
        int randomIndex = Random.Range(0, mGridPositions.Count);
        Vector3 randomPosition = mGridPositions[randomIndex];

        //Make sure you don't reuse an index by removing it from the grid positions
        mGridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //Spawns our tiles at the randomly selected positions
    void layoutObjectAtRandom(GameObject[] tileArray, int min, int max) {
        int objectCount = Random.Range(min, max + 1);

        for(int i = 0; i < objectCount; i++) {
            Vector3 position = randomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, position, Quaternion.identity);
        }
    }

    //Called by GameManager when we are ready to set up the board
	public void setupScene(int level) {

        boardSetup();
        initializeList();
        layoutObjectAtRandom(wallTiles, innerWallCount.min, innerWallCount.max);
        layoutObjectAtRandom(foodTiles, foodCount.min, foodCount.max);

        int enemyCount = (int) Mathf.Log(level, 2f);
        layoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1), Quaternion.identity);
    }
}
