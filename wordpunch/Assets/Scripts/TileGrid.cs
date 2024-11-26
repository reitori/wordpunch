using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TileGrid
{
    public LetterSpawner letterSpawner;
    public char[,] letterGrid;
    public GameObject[,] tiles;
    public int xSize;
    public int ySize;

    public TileGrid(char[,] letterGrid)
    {
        this.letterGrid = letterGrid;
        xSize = letterGrid.GetLength(0);
        ySize = letterGrid.GetLength(1);
        tiles = new GameObject[xSize, ySize];
    }

    public void SetTile(int x, int y, GameObject tile)
    {
        tiles[x, y] = tile;
    }

    public GameObject GetTile(int x, int y)
    {
        return tiles[x, y];
    }
    
    // public void SelectNewTile(LinkedList<GameObject> highlighted, GameObject tile) {
    //     if (highlighted.Count == 0 || ValidNewTile(highlighted.Last.Value, tile)) {
    //         highlighted.AddLast(tile);
    //     }
    // }

    public bool ValidNewTile(GameObject headTile, GameObject newTile) {
        if (IsAdjacentX(headTile.GetComponent<Tile>().x, newTile.GetComponent<Tile>().x) && IsAdjacentY(headTile.GetComponent<Tile>().y, newTile.GetComponent<Tile>().y)) {
            return true;
        }
        return false;
    }
    
    public bool ValidNewTile(Tile headTile, Tile newTile) {
        if (IsAdjacentX(headTile.x, newTile.x) && IsAdjacentY(headTile.y, newTile.y)) {
            Debug.Log("Tile is adjacent");
            return true;
        }
        return false;
    }


    public bool IsAdjacentX(int headX, int newX) {
        if (newX == headX - 1 || newX == headX || newX == headX + 1) {
            return true;
        }
        return false;
    }

    public bool IsAdjacentY(int headY, int newY) {
        if (newY > xSize - 1 || newY < 0) {
            throw new ArgumentException("y position of potential tile out of bounds ");
        }
        if (newY == headY - 1 || newY == headY || newY == headY + 1) {
            return true;
        }
        return false;


    }
    
    public int FloorMod(int a, int b)
    {
        return a - b * Mathf.FloorToInt((float)a / b);
    }

}
