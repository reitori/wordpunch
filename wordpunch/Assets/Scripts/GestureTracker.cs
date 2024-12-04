using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureTracker : MonoBehaviour
{
    private OVRHand hand;

    void Start()
    {
        hand = GetComponent<OVRHand>();
    }

    void Update()
    {
        if (hand != null && hand.IsTracked)
        {
            if (IsPointing())
            {
                Debug.Log($"{gameObject.name} is pointing!");
            }
            else if (IsFistClosed())
            {
                Debug.Log($"{gameObject.name} made a fist!");
                PerformPunch();
            }
        }
    }

    public bool IsPointing()
    {
        // Check if the index finger is extended
        bool indexExtended = hand.GetFingerIsPinching(OVRHand.HandFinger.Index) == false;

        // Check if all other fingers are NOT extended
        bool thumbRelaxed = !hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
        bool middleRelaxed = !hand.GetFingerIsPinching(OVRHand.HandFinger.Middle);
        bool ringRelaxed = !hand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
        bool pinkyRelaxed = !hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky);

        // Return true if index finger is extended and other fingers are relaxed
        return indexExtended && thumbRelaxed && middleRelaxed && ringRelaxed && pinkyRelaxed;
    }

    private bool IsFistClosed()
    {
        // Check if all fingers are pinching (or curled in a fist-like manner)
        bool thumbClosed = hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
        bool indexClosed = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool middleClosed = hand.GetFingerIsPinching(OVRHand.HandFinger.Middle);
        bool ringClosed = hand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
        bool pinkyClosed = hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky);

        // Return true if all fingers are closed
        return thumbClosed && indexClosed && middleClosed && ringClosed && pinkyClosed;
    }

    private void PerformPunch()
    {
        // Punching logic here
        Debug.Log($"{gameObject.name} performed a punch!");
        // Example: Play a punching animation, trigger an event, or apply force to an object
    }
}
