using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject terrainToPlace;

    UILogicController uiLogicController;

    private ARRaycastManager arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    public bool firstHoldOrTapCompleted;

    void Start()
    {
        arOrigin = FindObjectOfType<ARRaycastManager>();
        uiLogicController = GetComponent<UILogicController>();

        // Put the terrain high so that it can't be seen.
        Vector3 position = new Vector3(0.0f, 1000.0f, 0.0f);
        firstHoldOrTapCompleted = false;
        terrainToPlace.transform.position = position;
    }

    // The update function for placing the field. Initially it moves with the placement indicator.
    // After the initial hold/tap, it won't be stuck to the marker.
    void Update()
    {
        if (uiLogicController.activePage == UILogicController.PagesEnum.SetupPage)
        {
            // Update pose for terrain and placement indicator.
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            // Handle holding on the the real world.
            bool holdOrTap = Input.touchCount > 0 && !ClickLogic.OnUI();
            if (placementPoseIsValid && (!firstHoldOrTapCompleted || holdOrTap)) {
                if (holdOrTap) {
                    firstHoldOrTapCompleted = true;
                }
                PlaceTerrain();
            }
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceTerrain()
    {
        // Instantiate(terrainToPlace, placementPose.position, placementPose.rotation);
        terrainToPlace.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid) {
            placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
