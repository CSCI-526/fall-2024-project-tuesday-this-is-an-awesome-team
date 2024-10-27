using UnityEngine;
using Cinemachine;
using System.Collections;

public class MapViewController : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineVirtualCamera mapCamera;

    private bool isMapView = false;

    private void Start()
    {
        followCamera.Priority = 2;
        mapCamera.Priority = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMapView();
        }
    }

    private void ToggleMapView()
    {
        isMapView = !isMapView;

        if (isMapView)
        {
            mapCamera.Priority = 3;
        }
        else
        {
            mapCamera.Priority = 1;
        }
    }

}
