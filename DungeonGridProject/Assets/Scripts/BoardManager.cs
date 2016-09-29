using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public int columns;
    public int rows;

    private Transform mBoardHolder;     //An object to hold everything on the board for clean up
    private List<Vector3> mGridPositions = new List<Vector3>();

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

        //TODO: Insert logic for reading map file and filling in tiles
    }

    private void readFile() {
        //TODO: Read map data in from a file
    }

    public void SetupScene(int floorNumber) {
        BoardSetup();
        InitializeList();

        //TODO: Add functions for adding player, enemies, items, etc to map
    }
}
