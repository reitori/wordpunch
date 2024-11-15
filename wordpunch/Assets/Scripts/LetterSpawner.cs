using System.Collections;
using System.Collections.Generic;
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

    public void InitializeGrid(char[,] letterGrid)
    {
        int rows = letterGrid.GetLength(0); // Number of rows in the 2D array
        int cols = letterGrid.GetLength(1); // Number of columns in the 2D array

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

                    // Scale the letter
                    letterInstance.transform.localScale = new Vector3(letterScale, letterScale, letterScale);
                    Vector3 targetPosition = new Vector3(0, spawnPosition.y, 0);
                    letterInstance.transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition);
                    // TODO:Store row and column index in the letter's script or component
                }
            }
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
}
