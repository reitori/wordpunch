using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLetter : MonoBehaviour
{
    public GameObject[] letterPrefabs;
    public float cylinderRadius = 30.0f;
    public float cylinderHeight = 30.0f;
    public int spawnCount = 30;

    public float minDistance = 10.0f;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private Dictionary<char, int> letterWeights = new Dictionary<char, int>
    {
        { 'A', 8 }, { 'B', 2 }, { 'C', 3 }, { 'D', 4 }, { 'E', 13 },
        { 'F', 2 }, { 'G', 2 }, { 'H', 6 }, { 'I', 7 }, { 'J', 1 },
        { 'K', 1 }, { 'L', 4 }, { 'M', 3 }, { 'N', 7 }, { 'O', 8 },
        { 'P', 2 }, { 'Q', 1 }, { 'R', 6 }, { 'S', 6 }, { 'T', 9 },
        { 'U', 3 }, { 'V', 1 }, { 'W', 2 }, { 'X', 1 }, { 'Y', 2 }, { 'Z', 1 }
    };

    private List<char> weightedLetters; // letter pool

    void GenerateWeightedLetterList()
    {
        weightedLetters = new List<char>();
        foreach (var letter in letterWeights)
        {
            for (int i = 0; i < letter.Value; i++)
            {
                weightedLetters.Add(letter.Key);
            }
        }
    }

    void SpawnLetters()
    {
        int spawnedCount = 0;
        while (spawnedCount < spawnCount)
        {
            char selectedLetter = GetRandomLetter();
            GameObject letterPrefab = GetLetterPrefab(selectedLetter);

            if (letterPrefab != null)
            {
                Vector3 spawnPosition = GetRandomPositionOnCylinder();

                if (IsPositionValid(spawnPosition))
                {
                    Instantiate(letterPrefab, spawnPosition, Quaternion.LookRotation(-spawnPosition.normalized));
                    spawnedPositions.Add(spawnPosition);
                    spawnedCount++;
                }
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false; // avoid overlapping
            }
        }
        return true;
    }

    Vector3 GetRandomPositionOnCylinder()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        float height = Random.Range(0, cylinderHeight);

        float x = Mathf.Cos(angle) * cylinderRadius;
        float z = Mathf.Sin(angle) * cylinderRadius;

        return new Vector3(x, height, z);
    }

    char GetRandomLetter()
    {
        int randomIndex = Random.Range(0, weightedLetters.Count);
        return weightedLetters[randomIndex];
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
        GenerateWeightedLetterList();
        SpawnLetters();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
