using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Exploder.Utils;

public class GameManager : MonoBehaviour
{
    public Camera mainCam;
    public GridGenerator gridGenerator;
    public LetterSpawner letterSpawner;
    public char[,] letterGrid;
    public TileGrid tileGrid;
    // public LinkedList<GameObject> highlightedTiles;
    public List<Tile> highlightedTiles;
    public bool selectLettersMode = true;

    public bool isPunching;
    
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
        isPunching = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Thought: utilize update() to create color flow effect on highlighted tiles
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
    
    public void highlightTile(int x, int y, Color baseColor, Color emissionColor)
    {
        // yellow_emission: r:190, g:90, b:30, a:255, intensity: 5, yellow_base: r:190, g:40, b:0, a:150, intensity: 1
        GameObject tile = tileGrid.tiles[x, y];
        var tileRenderer = tile.GetComponent<Renderer>();
        // change '_BaseColor' of the material
        // intensity = 1f;
        // factor = Mathf.Pow(2, intensity);
        tileRenderer.material.SetColor("_BaseColor", baseColor);
        // change '_EmissionColor' of the material
        // float intensity = 1f;
        // float factor = Mathf.Pow(2, intensity);
        tileRenderer.material.SetColor("_EmissionColor", emissionColor);
    }

    public bool wordValid(string word) {
        return gridGenerator.IsWordValid(word);
    }


    public void explodeWord() {
        // selectLettersMode = false; -> done in gesture detector?
        // changeGridTransparancy(0.0f);

        foreach (Tile tile in highlightedTiles) {
            GameObject tileObject = tileGrid.tiles[tile.x, tile.y];
            ExploderSingleton.Instance.ExplodeObject(tileObject);
        }
        Invoke("refillTiles", 2f);
    }

    public void refillTiles() {
        foreach (Tile tile in highlightedTiles) {
            char selectedLetter = gridGenerator.GetRandomLetter();
            letterSpawner.SpawnLetterAt(tile.x, tile.y, letterGrid, tileGrid, selectedLetter);
        }
        highlightedTiles.Clear();

        selectLettersMode = true;
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
        selectLettersMode = false;

        foreach (Tile tile in highlightedTiles) {
            //basecolor black, emissioncolor red
            Color baseColor = new Color(0, 0, 0);
            Color emissionColor = new Color(255, 0, 0);
            highlightTile(tile.x, tile.y, baseColor, emissionColor);
        }
        // invoke restoreTiles after 3 seconds with material
        Invoke("restoreTile", 2f);
    }

    public void restoreTile()
        {
        // changeGridTransparancy(1.0f);

        foreach (Tile tile in highlightedTiles) {
            // basecolor r:65, g:85, b:110, a:150, intensity: 0
            // emissioncolor r:100, g:135, b:190, a:255, intensity: 2.5
            // Color baseColor = new Color(65, 85, 110);
            // Color emissionColor = new Color(100, 135, 190);

            // GameObject tileObject = tileGrid.tiles[tile.x, tile.y];
            // Renderer tileRenderer = tileObject.GetComponent<Renderer>();
            // tileRenderer.material.SetColor("_EmissionColor", emissionColor);
            // tileRenderer.material.SetColor("_BaseColor", baseColor);
            // tile.isHighlighted = false;

            //set back to original material
            tileGrid.tiles[tile.x, tile.y].GetComponent<Renderer>().material = Resources.Load("Materials/dissolve", typeof(Material)) as Material;
        }
        highlightedTiles.Clear();
        
        selectLettersMode = true;
    }
    
}
