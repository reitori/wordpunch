using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public MeshRenderer mesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material mat;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (mesh != null)
        {
            mat = mesh.material;
        }
        // initialize gameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHover(GameObject other)
    {
        Debug.Log($"Hover detected!");
        // StartCoroutine(DissolveCo());
       

        Tile hitTile = other.GetComponent<Tile>();
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

    public void OnHover2()
    {
        Debug.Log($"Hover2 detected!");
    }

    IEnumerator DissolveCo()
    {
        float counter = 0;
        while (mat.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            mat.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
