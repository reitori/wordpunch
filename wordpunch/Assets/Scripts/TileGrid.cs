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
            return true;
        }
        return false;
    }


    public bool IsAdjacentX(int headX, int newX) {
        int XLeft = (((headX - 1) % xSize) + xSize) % xSize; //C# does not do "true" mod. This accounts for when headX-1 = -1. Then (mod xSize) is xSize - 1
        int XRight = (headX + 1) % xSize;

        if (Math.Abs(XLeft - XRight) > 2) //out of bounds
        {
            return false;
        }
        if (newX == XLeft|| newX == headX || newX == XRight) {
            return true;
        }
        return false;
    }

    public bool IsAdjacentY(int headY, int newY) {
        int YLeft = (((headY - 1) % ySize) + ySize) % ySize; 
        int YRight = (headY + 1) % ySize;

        if (newY == YLeft || newY == headY || newY == YRight) {
            return true;
        }
        return false;
    }
    
    public int FloorMod(int a, int b)
    {
        return a - b * Mathf.FloorToInt((float)a / b);
    }

}
