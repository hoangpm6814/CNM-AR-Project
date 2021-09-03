using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaceObjectOnPlace : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private Pose PlacementPose;
    private bool placementPoseIsValid = false;
    private bool isObjectPlaced;

    public GameObject placementIndicator;
    // public GameObject prefabToPlace;
    public GameObject objectToPlace;
    public Camera arCamera;

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isObjectPlaced) {
            UpdatePlacementPose();
            // UpdatePlacementIndicator(); 

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
        
    }

    private void PlaceObject()
    {
        // Old version did not have rotate
        Instantiate(objectToPlace, PlacementPose.position, PlacementPose.rotation);
        // One gameobject
        isObjectPlaced = true;

        // // Multiple gameobjects
        // isObjectPlaced = false;
        placementIndicator.SetActive(false);

        // // New version with rotation
        // var spawnedObject = Instantiate(objectToPlace, PlacementPose.position, PlacementPose.rotation);
        // RotateManager.GetInstance().SetPyramid(spawnedObject);
        // isObjectPlaced = true;
        // placementIndicator.SetActive(false);
    }

    // private void UpdatePlacementIndicator()
    // {
    //     if (placementPoseIsValid)
    //     {
    //         placementIndicator.SetActive(true);
    //         placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
    //     }
    //     else
	// 	{
    //         placementIndicator.SetActive(false);
	// 	}
	// }

    private void UpdatePlacementPose()
	{
        // var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.All);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
		{
            PlacementPose = hits[0].pose;
            // var cameraForward = Camera.current.transform.forward;
            var cameraForward = arCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
		}
        else
		{
            placementIndicator.SetActive(false);
		}
	}
}
