using System;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private GameObject hand;
    private float rayLength = 10f;

    HandCollisionHandler handCollider = GameObject.Find("Hand").GetComponent<HandCollisionHandler>();
    void Start()
    {
        // Initialize the collider box with a BoxCollider component
        hand = new GameObject("Hand");
        BoxCollider boxCollider = hand.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;  // Set as trigger to detect collisions without physics response

        // Add a component to handle the collision events
        hand.AddComponent<HandCollisionHandler>();

        // Set the scale of the collider to make it a thin box along the z-axis
        hand.transform.localScale = new Vector3(0.1f, 0.1f, rayLength);
        hand.SetActive(false); // Disable initially
    }

    void Update()
    {
        // Get input for horizontal and vertical axis
        float horizontalRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float verticalRotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        Debug.Log(Input.GetAxis("Horizontal"));
        Debug.Log(Input.GetAxis("Vertical"));
        Debug.Log(Input.GetKey(KeyCode.A));

        // Rotate around the y-axis for horizontal input (left and right)
        transform.Rotate(0, horizontalRotation, 0, Space.World);

        // Rotate around the x-axis for vertical input (up and down)
        transform.Rotate(-verticalRotation, 0, 0, Space.Self);

        // Activate the ray collider box and position it along the ray when the mouse button is held down
        if (Input.GetMouseButton(0))
        {
            PositionHandCollider();
            // Debug.Log("Mouse Button Pressed");
        }
        else
        {
            // hand.SetActive(false); // Hide the collider when not pressing the mouse
        }
    }

    void PositionHandCollider()
    {
        // Activate the collider
        hand.SetActive(true);

        // Get the direction from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Position the collider at the midpoint of the ray
        hand.transform.position = ray.origin + ray.direction * (rayLength / 2);
        
        // Rotate the collider to align with the ray direction
        hand.transform.rotation = Quaternion.LookRotation(ray.direction);
    }
}

