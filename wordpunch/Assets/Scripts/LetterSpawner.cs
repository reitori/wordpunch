using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class LetterSpawner : MonoBehaviour
{
    public GameObject[] letterPrefabs;

    public float distanceAboveGround = 5.0f;

    private Dictionary<char, int> letterWeights = new Dictionary<char, int>
    {
        { 'A', 8 }, { 'B', 2 }, { 'C', 3 }, { 'D', 4 }, { 'E', 13 },
        { 'F', 2 }, { 'G', 2 }, { 'H', 6 }, { 'I', 7 }, { 'J', 1 },
        { 'K', 1 }, { 'L', 4 }, { 'M', 3 }, { 'N', 7 }, { 'O', 8 },
        { 'P', 2 }, { 'Q', 1 }, { 'R', 6 }, { 'S', 6 }, { 'T', 9 },
        { 'U', 3 }, { 'V', 1 }, { 'W', 2 }, { 'X', 1 }, { 'Y', 2 }, { 'Z', 1 }
    };

    private List<char> weightedLetters; // letter pool

    public float letterSpacing = 0.5f; // Spacing between letters

    public float letterScale = 0.5f;

    private Vector3 GetPositionOnCylinder(int row, int col, int rows, int cols, float radius)
    {
        float angle = (2 * Mathf.PI / cols) * col; // Calculate angle for each column
        float height = row * letterSpacing + distanceAboveGround;

        // Convert polar coordinates (angle, radius) to Cartesian coordinates (x, z)
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(x, height, z);
    }

    public void InitializeGrid(char[,] letterGrid, TileGrid tileGrid)
    {
        int rows = letterGrid.GetLength(0); // Number of rows in the 2D array
        int cols = letterGrid.GetLength(1); // Number of columns in the 2D array

        tileGrid.xSize = rows;
        tileGrid.ySize = cols;


        float cylinderHeight = rows * letterSpacing;
        float cylinderRadius = (cols * letterSpacing) / (2 * Mathf.PI);

        // Spawn letters in a grid pattern around the cylinder
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                char letter = letterGrid[row, col];
                GameObject letterPrefab = GetLetterPrefab(letter);

                if (letterPrefab != null)
                {
                    Vector3 spawnPosition = GetPositionOnCylinder(row, col, rows, cols, cylinderRadius);
                    GameObject letterInstance = Instantiate(letterPrefab, spawnPosition, Quaternion.identity);
                    
                    // Renderer renderer = letterInstance.GetComponent<Renderer>(); //Enables Transparancy 
                    // get letterInstance's child's renderer
                    // Renderer renderer = letterInstance.transform.GetChild(0).GetComponent<Renderer>();

                    
                    // Material material = renderer.material;

                    // material.SetFloat("_Mode", 3); // For Standard Shader; sets rendering mode to Transparent
                    // material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    // material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    // material.SetInt("_ZWrite", 0);
                    // material.DisableKeyword("_ALPHATEST_ON");
                    // material.EnableKeyword("_ALPHABLEND_ON");
                    // material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    // material.renderQueue = 3000;

                    // // destroy the mesh collider so we can rely on the boxcollider
                    // Destroy(letterInstance.GetComponent<MeshCollider>());
                    // Collider BCollider = letterInstance.AddComponent<BoxCollider>();
                    // BCollider.transform.localScale = new Vector3(2f, 2f, 0.5f);
                    
                    // // create a rigidbody so that the hand "trigger" can detect the tile
                    // Rigidbody rigidbody = letterInstance.AddComponent<Rigidbody>();
                    // rigidbody.isKinematic = true;
                    // rigidbody.useGravity = false;

                    // RayInteractable rayInteractable = letterInstance.AddComponent<RayInteractable>();
                    // InteractableUnityEventWrapper eventWrapper = letterInstance.AddComponent<InteractableUnityEventWrapper>();
                    // eventWrapper.InjectInteractableView(rayInteractable);
                    //AddHoverEvents(eventWrapper, letterInstance);

                    
                    // add a "Tile" script to the gameobject's child
                    Tile tile = letterInstance.transform.GetChild(0).gameObject.AddComponent<Tile>();
                    tile.initializeTile(row, col, letter);

                    tileGrid.SetTile(row, col, letterInstance.transform.GetChild(0).gameObject); // assigns tileGrid[x, y] to the GameObject instance from the letter 

                    // Scale the letter
                    letterInstance.transform.localScale = new Vector3(letterScale, letterScale, letterScale);
                    Vector3 targetPosition = new Vector3(0, spawnPosition.y, 0);
                    letterInstance.transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition);
                    // TODO:Store row and column index in the letter's script or component
                }
            }
        }
    }

    public void SpawnLetterAt(int x, int y, char[,] letterGrid, TileGrid tileGrid, char selectedLetter)
    {
        int rows = letterGrid.GetLength(0);
        int cols = letterGrid.GetLength(1);

        float cylinderHeight = rows * letterSpacing;
        float cylinderRadius = (cols * letterSpacing) / (2 * Mathf.PI);

        GameObject letterPrefab = GetLetterPrefab(selectedLetter);

        if (letterPrefab != null)
        {
            Vector3 spawnPosition = GetPositionOnCylinder(x, y, rows, cols, cylinderRadius);

            GameObject letterInstance = Instantiate(letterPrefab, spawnPosition, Quaternion.identity);

            // set dissolve amount to 1
            Renderer renderer = letterInstance.transform.GetChild(0).GetComponent<Renderer>();
            Material material = renderer.material;
            material.SetFloat("_DissolveAmount", 1);

            // add tile
            Tile tile = letterInstance.transform.GetChild(0).gameObject.AddComponent<Tile>();
            tile.initializeTile(x, y, selectedLetter);

            // Add the letter to the tile grid
            tileGrid.SetTile(x, y, letterInstance.transform.GetChild(0).gameObject);


            // Scale the letter
            letterInstance.transform.localScale = new Vector3(letterScale, letterScale, letterScale);
            Vector3 targetPosition = new Vector3(0, spawnPosition.y, 0);
            letterInstance.transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition);

            // Store the letter in the grid
            letterGrid[x, y] = selectedLetter;

            // de-dissolve the letter
            StartCoroutine(DissolveCo(material, 0.04f, 0.08f));
        }
    }

    IEnumerator DissolveCo(Material mat, float dissolveRate, float refreshRate)
    {
        float counter = 1;
        while (mat.GetFloat("_DissolveAmount") > 0)
        {
            counter -= dissolveRate;
            mat.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    GameObject GetLetterPrefab(char letter)
    {
        int index = letter - 'A';
        if (index >= 0 && index < letterPrefabs.Length)
        {
            return letterPrefabs[index];
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Function to handle hover and unhover events
    private void AddHoverEvents(InteractableUnityEventWrapper eventWrapper, GameObject letterInstance)
    {
        // Add listeners for hover and unhover
        eventWrapper.WhenHover.AddListener(() => OnLetterHover(letterInstance));
        eventWrapper.WhenUnhover.AddListener(() => OnLetterUnhover(letterInstance));
    }

    // Called when the letter is hovered over
    private void OnLetterHover(GameObject letterInstance)
    {
        // Example: Change color on hover
        Renderer renderer = letterInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }

        Debug.Log("Hovered over: " + letterInstance.name);
    }

    // Called when the letter is unhovered
    private void OnLetterUnhover(GameObject letterInstance)
    {
        // Reset color
        Renderer renderer = letterInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }

        Debug.Log("Stopped hovering over: " + letterInstance.name);
    }

    
}
