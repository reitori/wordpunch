using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HandCollisionHandler : MonoBehaviour
{
    public GameManager gameManager;
    public bool selectLettersMode = true;
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
                    //handColliderOn = false;
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
        if (selectLettersMode)
        {
            Debug.Log("DETECT: " + other.gameObject.GetComponent<Tile>().letter + " (" +
                      other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
            colliding = true;
            uncollideTime = 0f;

            Tile hitTile = other.gameObject.GetComponent<Tile>();

            if (gameManager.highlightedTiles.Count() != 0)
            {
                if (hitTile.isHighlighted == false && gameManager.tileGrid.ValidNewTile(gameManager.highlightedTiles.Last<Tile>(), hitTile))
                {
                    gameManager.highlightedTiles.Add(hitTile);
                    gameManager.highlightTile(hitTile.x, hitTile.y, Color.yellow);
                    Debug.Log("Hand hit: " + other.gameObject.GetComponent<Tile>().letter + " (" +
                              other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
                    hitTile.isHighlighted = true;
                    return;
                }
            }
            else
            {
                gameManager.highlightedTiles.Add(hitTile);
                gameManager.highlightTile(hitTile.x, hitTile.y, Color.yellow);
                Debug.Log("Hand hit: " + other.gameObject.GetComponent<Tile>().letter + " (" +
                    other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
                hitTile.isHighlighted = true;
            }
        }
        else //punching
        {

        }

    }

    private void OnTriggerExit(Collider other)
    {
        print("exit");
        colliding = false;
    }
    
}