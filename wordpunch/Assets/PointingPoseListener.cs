using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class PointingPoseListener : MonoBehaviour
{
    [SerializeField] ActiveStateSelector pointingPose;

    // private void Update(){
    //     if (pointingPose != null)
    //     {
    //         Debug.Log("Updating");
    //         pointingPose.WhenSelected += OnPointingPoseActivated;
    //         pointingPose.WhenUnselected += OnPointingPoseDeactivated;
    //     }
    // }
    // private void OnEnable()
    // {
    //     Debug.Log("Enabled");
    //     // Subscribe to changes
    //     if (pointingPose != null)
    //     {
    //         Debug.Log("Pose not null");
    //         pointingPose.WhenSelected += OnPointingPoseActivated;
    //         pointingPose.WhenUnselected += OnPointingPoseDeactivated;
    //     }
    // }

    // private void OnDisable()
    // {
    //     // Unsubscribe from changes
    //     if (pointingPose != null)
    //     {
    //         pointingPose.WhenSelected -= OnPointingPoseActivated;
    //         pointingPose.WhenUnselected -= OnPointingPoseDeactivated;
    //     }
    // }

    public void OnPointingPoseActivated()
    {
        Debug.Log("PointingPose is active!");
    }

    private void OnPointingPoseDeactivated()
    {
        Debug.Log("PointingPose is no longer active.");
    }
}
