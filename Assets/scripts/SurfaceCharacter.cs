using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Script : MonoBehaviour
{
    public ARRaycastHit ARRaycastHit;
    public ARRaycastManager arRaycastMng;
    public bool canPlace = false;
    private Pose placementPose;

    public GameObject objectToSpawn;

    public Camera xrCamera;
    public GameObject targetIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdateTargetIndicator();

        if (canPlace && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            InstantiateObject();
        }

        if (canPlace && Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateObject();
        }
    }

    void InstantiateObject()
    {
        Instantiate(objectToSpawn, placementPose.position, placementPose.rotation);
    }

    void UpdatePlacementPose()
    {
        Vector3 screenCenter = xrCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arRaycastMng.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        canPlace = hits.Count > 0;

        if (canPlace)
        {
            placementPose = hits[0].pose;

            var cameraForward = xrCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    void UpdateTargetIndicator()
    {
        if (canPlace)
        {
            targetIndicator.SetActive(true);
            targetIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else
        {
            targetIndicator.SetActive(false);
        }
    }
}
