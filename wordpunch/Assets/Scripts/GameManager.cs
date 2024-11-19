using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCam;
    public GridGenerator gridGenerator;
    public LetterSpawner letterSpawner;
    public char[,] letterGrid;
    public TileGrid tileGrid;
    public LinkedList<GameObject> highlightedTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        //This gets the Main Camera from the Scene and enables it
        mainCam = Camera.main;
        mainCam.enabled = true;

        Debug.Log("Starting");
        gridGenerator = new GridGenerator();
        letterGrid = gridGenerator.GenerateGrid(); // create and store the grid of random letters
        Debug.Log(letterGrid.ToString());
        tileGrid = new TileGrid(letterGrid); // instantiate an instance of TileGrid using letterGrid 
        letterSpawner.InitializeGrid(letterGrid, tileGrid); // spawn the letter tiles and store them in tileGrid
        
        highlightedTiles = new LinkedList<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    public void PrintGrid(char[,] grid)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);
    
        for (int row = 0; row < numRows; row++)
        {
            string rowString = "";
            for (int col = 0; col < numCols; col++)
            {
                rowString += grid[row, col] + " ";
            }
        
            // Print each row to the console
            Debug.Log(rowString);
        }
    }
    
    
    
}
