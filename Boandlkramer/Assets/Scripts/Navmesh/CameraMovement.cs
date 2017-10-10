using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    // reference to the player object that the camera focuses on
    public GameObject player;

    // Zoom Range
    public float minZoom = 30f;
    public float maxZoom = 60f;

    // Zoom Speed
    public float zoomSpeed = 0.4f;

    // initial distance from camera to player object
    private Vector3 offset;

    // Use this for initialization
    void Start ()
    {
        // save initial distance vector from player to camera 
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        // Zoom with mouse wheel
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView >= minZoom)
                Camera.main.fieldOfView -= zoomSpeed;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= maxZoom)
                Camera.main.fieldOfView += zoomSpeed;
        }

    }
	
	void LateUpdate ()
    {
        // adjust the camera position
        transform.position = player.transform.position + offset;
    }
}
