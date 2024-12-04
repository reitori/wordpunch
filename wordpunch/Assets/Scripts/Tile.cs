using UnityEngine;


public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public char letter;
    public bool isHighlighted;

    public void initializeTile(int x, int y, char letter)
    {
        this.x = x;
        this.y = y;
        this.letter = letter;
        this.isHighlighted = false;
    }
}
