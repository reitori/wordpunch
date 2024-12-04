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
    // public LinkedList<GameObject> highlightedTiles;
    public List<Tile> highlightedTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        //This gets the Main Camera from the Scene and enables it
        mainCam = Camera.main;
        mainCam.enabled = true;

        Debug.Log("Starting");
        gridGenerator = gameObject.AddComponent<GridGenerator>();
        letterGrid = gridGenerator.GenerateGrid(); // create and store the grid of random letters
        Debug.Log(letterGrid.ToString());
        tileGrid = new TileGrid(letterGrid); // instantiate an instance of TileGrid using letterGrid 
        letterSpawner.InitializeGrid(letterGrid, tileGrid); // spawn the letter tiles and store them in tileGrid
        
        // highlightedTiles = new LinkedList<GameObject>();
        highlightedTiles = new List<Tile>();
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
    
    public void highlightTile(int x, int y, Color color)
    {
        GameObject tileObject = tileGrid.tiles[x, y];
        // change the emission color of the material of the tile
        Renderer tileRenderer = tileObject.GetComponent<Renderer>();
        tileRenderer.material.SetColor("_EmissionColor", color * 5f);
    }

    public bool wordValid(string word) {
        return gridGenerator.IsWordValid(word);
    }


    public void explodeWord() {
        HandCollisionHandler hand = GameObject.Find("Hand").GetComponent<HandCollisionHandler>();
        hand.selectLettersMode = false;
        changeGridTransparancy(0.0f);

        foreach (Tile tile in highlightedTiles) {
            highlightTile(tile.x, tile.y, Color.green);
        }
        Invoke("restoreTiles", 3f);
    }

    public void changeGridTransparancy(float normTransparency)
    {
        for(int i = 0; i < tileGrid.xSize; i++)
        {
            for(int j = 0; j < tileGrid.ySize; j++)
            {
                GameObject tileObject = tileGrid.tiles[i, j];
                Renderer tileRenderer = tileObject.GetComponent<Renderer>();
                Color color = tileRenderer.material.color;
                Debug.Log(color);
                color.a = normTransparency;
                tileRenderer.material.color = color;
            }
        }
    }

    public void invalidWarn() {
        HandCollisionHandler hand = GameObject.Find("Hand").GetComponent<HandCollisionHandler>();
        hand.selectLettersMode = false;

        foreach (Tile tile in highlightedTiles) {
            highlightTile(tile.x, tile.y, Color.red);
        }
        // add effect
        Invoke("restoreTiles", 3f);
    }

    public void restoreTiles() {
        changeGridTransparancy(1.0f);

        foreach (Tile tile in highlightedTiles) {
            highlightTile(tile.x, tile.y, Color.white);
            tile.isHighlighted = false;
        }
        highlightedTiles.Clear();

        HandCollisionHandler hand = GameObject.Find("Hand").GetComponent<HandCollisionHandler>();
        hand.selectLettersMode = true;
    }
    
}
