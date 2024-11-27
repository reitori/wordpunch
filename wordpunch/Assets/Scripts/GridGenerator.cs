using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public LetterSpawner letterSpawner;
    private HashSet<string> scrabbleWords = new HashSet<string>();
    private List<string> commonWords = new List<string>();
    public int numRows = 4;
    public int numCols = 20;
    public int numPreInsertWords = 2;
    private char[,] letterGrid;
    
    private Dictionary<char, int> letterWeights = new Dictionary<char, int>
    {
        { 'A', 8 }, { 'B', 2 }, { 'C', 3 }, { 'D', 4 }, { 'E', 13 },
        { 'F', 2 }, { 'G', 2 }, { 'H', 6 }, { 'I', 7 }, { 'J', 1 },
        { 'K', 1 }, { 'L', 4 }, { 'M', 3 }, { 'N', 7 }, { 'O', 8 },
        { 'P', 2 }, { 'Q', 1 }, { 'R', 6 }, { 'S', 6 }, { 'T', 9 },
        { 'U', 3 }, { 'V', 1 }, { 'W', 2 }, { 'X', 1 }, { 'Y', 2 }, { 'Z', 1 }
    };

    private List<char> weightedLetters; // letter pool



    public char[,] GenerateGrid()
    {
        letterGrid = new char[numRows, numCols];
        LoadWordsFromFile("ScrabbleWords");
        LoadWordsFromFile("CommonWords");
        GenerateWeightedLetterList();
        prepareLetterGrid();
        return letterGrid;
    }

    void LoadWordsFromFile(string fileName)
    {
        TextAsset wordFile = Resources.Load<TextAsset>(fileName);

        if (wordFile != null)
        {
            using (StringReader reader = new StringReader(wordFile.text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (fileName == "ScrabbleWords")
                    {
                        scrabbleWords.Add(line.Trim().ToUpper());
                    }
                    else if (fileName == "CommonWords")
                    {
                        commonWords.Add(line.Trim().ToUpper());
                    }
                }
            }
            Debug.Log("Words loaded: " + scrabbleWords.Count);
        }
        else
        {
            Debug.LogError("Word file not found in Resources folder.");
        }
    }

    void prepareLetterGrid()
    {
        List<string> preInsertWords = GetRandomWords(numPreInsertWords);

        foreach (string word in preInsertWords)
        {
            PlaceWordInGrid(word);
        }
        FillRemainingCells();
    }

    List<string> GetRandomWords(int numWords)
    {
        List<string> words = new List<string>();
        if (commonWords.Count == 0)
        {
            Debug.LogError("Common words not loaded.");
            return words;
        }

        for (int i = 0; i < numWords; i++)
        {
            int randomIndex = Random.Range(0, commonWords.Count);
            words.Add(commonWords[randomIndex]);
        }

        return words;
    }

    void PlaceWordInGrid(string word)
    {
        // random start position
        int startRow = Random.Range(0, numRows);
        int startCol = Random.Range(0, numCols);

        // try to find a path to place the word
        List<Vector2Int> path = CanPlaceWord(word, startRow, startCol);

        if (path != null)
        {
            Debug.Log("Word placed: " + word);
            Debug.Log("Path: " + string.Join(" -> ", path));
            // place the word on the grid
            for (int i = 0; i < path.Count; i++)
            {
                Vector2Int pos = path[i];
                letterGrid[pos.x, pos.y] = word[i];
            }
        }
    }

    List<Vector2Int> CanPlaceWord(string word, int startRow, int startCol)
    {
        Vector2Int current = new Vector2Int(startRow, startCol);
        Vector2Int prev = current;
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(current);

        char[] chars = word.ToCharArray();
        
        for (int i = 1; i < chars.Length; i++)
        {
            List<Vector2Int> neighbors = GetNeighbor(current.x, current.y);
            neighbors.Shuffle();

            bool placed = false;

            foreach (Vector2Int neighbor in neighbors)
            {
                if (letterGrid[neighbor.x, neighbor.y] != '\0')
                {
                    continue;
                }

                if (path.Contains(neighbor))
                {
                    continue;
                }

                current = neighbor;
                placed = true;
                path.Add(current);
                break;
            }

            if (!placed)
            {
                return null;
            }
        }

        return path;

    }

    List<Vector2Int> GetNeighbor(int row, int col)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
    {
        new Vector2Int(row - 1, WrapColumn(col)),
        new Vector2Int(row - 1, WrapColumn(col - 1)),
        new Vector2Int(row - 1, WrapColumn(col + 1)),
        new Vector2Int(row, WrapColumn(col - 1)),
        new Vector2Int(row, WrapColumn(col + 1)),
        new Vector2Int(row + 1, WrapColumn(col)),
        new Vector2Int(row + 1, WrapColumn(col - 1)),
        new Vector2Int(row + 1, WrapColumn(col + 1))
    };

        // keep only in-bounds neighbors vertically
        neighbors.RemoveAll(n => n.x < 0 || n.x >= numRows);

        return neighbors;
    }

    int WrapColumn(int col)
    {
        if (col < 0)
            return numCols - 1;
        if (col >= numCols)
            return 0;
        return col;
    }


    char GetRandomLetter()
    {
        int randomIndex = Random.Range(0, weightedLetters.Count);
        return weightedLetters[randomIndex];
    }

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

    public void FillRemainingCells()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (letterGrid[row, col] == '\0')
                {
                    letterGrid[row, col] = GetRandomLetter();
                }
            }
    }}

    

    // word is in capitals
    public bool IsWordValid(string word)
    {
        // can change to binary search
        return scrabbleWords.Contains(word);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
