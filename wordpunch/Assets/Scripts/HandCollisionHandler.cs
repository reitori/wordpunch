using System;
using Unity.VisualScripting;
using UnityEngine;

public class HandCollisionHandler : MonoBehaviour
{
    public void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object this collider hits
        Debug.Log("Hand hit: " + other.gameObject.GetComponent<Tile>().letter + " (" + other.gameObject.GetComponent<Tile>().x + ", " + other.gameObject.GetComponent<Tile>().y + ")");
        Renderer objectRenderer = other.GetComponent<Renderer>();
        // if (objectRenderer != null)
        // {
        // Change the color to indicate highlighting (MAKE THIS A FUNCTION DOWN THE LINE)
            objectRenderer.material.color = Color.yellow; // Change the color to red
        // }
    }
}