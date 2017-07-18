using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public CameraPoint cameraPoint;
    CameraController cameraSize;
	// Use this for initialization
	void Start () {
        cameraPoint = FindObjectOfType<CameraPoint>();
        cameraSize = GetComponent<CameraController>();
        GetComponent<Camera>().orthographicSize = cameraPoint.zoom;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(cameraPoint.transform.position.x, cameraPoint.transform.position.y, cameraPoint.transform.position.z);
        GetComponent<Camera>().orthographicSize = cameraPoint.zoom;
	}

}
