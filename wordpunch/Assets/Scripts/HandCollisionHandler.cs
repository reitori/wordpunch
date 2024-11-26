using System;
using Unity.VisualScripting;
using UnityEngine;

public class HandCollisionHandler : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] bool colliding;
    [SerializeField] float uncollideTime = 0f;
    [SerializeField] bool mousedown;
    public void Start()
    {
        // initialize gameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        mousedown = Input.GetMouseButton(0);
        if (!colliding || !mousedown)
        {
            uncollideTime += Time.deltaTime;
        }

        if (uncollideTime > 1f && gameManager.highlightedTiles.Count > 0)
        {
            // treat currently highlighted as a word and check validity
            string word = "";

            foreach (Tile tile in gameManager.highlightedTiles)
            {
                word += tile.letter;
            }
            print(word);
            if (gameManager.wordValid(word))
            {
                // word is valid
                gameManager.explodeWord();
            }
            else
            {
                // word is invalid
                gameManager.invalidWarn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DETECT: " + other.gameObject.GetComponent<Tile>().letter + " (" +
                  other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
        colliding = true;
        uncollideTime = 0f;

        Tile hitTile = other.gameObject.GetComponent<Tile>();

        int highlightedCount = gameManager.highlightedTiles.Count;
        if (highlightedCount > 1)
        {
            // cancel last highlight if player get back to the previously selected tile
            Tile secondLastTile = gameManager.highlightedTiles[highlightedCount - 2];
            if (secondLastTile != null)
            {
                if (secondLastTile.x == hitTile.x && secondLastTile.y == hitTile.y)
                {
                    Tile lastTile = gameManager.highlightedTiles[highlightedCount - 1];
                    gameManager.highlightedTiles.Remove(lastTile);
                    gameManager.highlightTile(lastTile.x, lastTile.y, Color.white);
                    return;
                }
            }

            if (gameManager.highlightedTiles.Contains(hitTile))
            {
                // handle the case where the player selects a highlighted tile again
            }
        }

        // if (true)
            // gameManager.highlightedTiles.Count == 0
            // || gameManager.tileGrid.ValidNewTile(gameManager.highlightedTiles[highlightedCount - 1], hitTile))
        // {
            gameManager.highlightedTiles.Add(hitTile);
            gameManager.highlightTile(hitTile.x, hitTile.y, Color.yellow);
            // Log the name of the object this collider hits
            Debug.Log("Hand hit: " + other.gameObject.GetComponent<Tile>().letter + " (" +
                      other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
            // Renderer objectRenderer = other.GetComponent<Renderer>();
            // // if (objectRenderer != null)
            // // {
            // // Change the color to indicate highlighting (MAKE THIS A FUNCTION DOWN THE LINE)
            //     objectRenderer.material.color = Color.yellow; // Change the color to red
            // }
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit");
        colliding = false;
    }
    
}