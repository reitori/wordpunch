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

        // if contains, return
        if (gameManager.highlightedTiles.Contains(hitTile))
        {
            return;
        }

        // basic logic:
        // if in selectMode, do ...
        // if in fistMode, and letter is in word, do ...

        // yellow_emission: r:190, g:90, b:30, a:255, intensity: 5, yellow_base: r:190, g:40, b:0, a:150, intensity: 1
        gameManager.highlightedTiles.Add(hitTile);
        Color baseColor = new Color(190, 40, 0, 150);
        Color emissionColor = new Color(190, 90, 30, 255);
        Debug.Log($"Highlighting tile: {hitTile.x}, {hitTile.y}");
        gameManager.highlightTile(hitTile.x, hitTile.y, baseColor, emissionColor);
        if (gameManager.highlightedTiles.Count > 2)
        {
            string word = "";
            foreach (Tile tile in gameManager.highlightedTiles)
            {
                word += tile.letter;
            }

            if (gameManager.wordValid(word))
            {
                gameManager.explodeWord();
            }
            else
            {
                // call with current material
                Material material = other.GetComponent<Renderer>().material;
                gameManager.invalidWarn(material);
            }
        }
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
