using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public float ZoomOutMultiplier;
    public float CameraZoomSmoothTime;
    public GameObject Subject;

    private float cameraOriginalSize;
    
    private Rigidbody2D playerRigidbody;
    private float cameraSizeVelocity;

    void Awake ()
    {
        playerRigidbody = Subject.GetComponent<Rigidbody2D>(); 
    }

	void Start () 
    {
        cameraOriginalSize = Camera.main.orthographicSize;
	}

    void FixedUpdate()
    {
        transform.position = new Vector3(Subject.transform.position.x, Subject.transform.position.y, transform.position.z);
    }

    void Update ()
    {
        float newSize = cameraOriginalSize + (playerRigidbody.velocity.magnitude * ZoomOutMultiplier);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, newSize, ref cameraSizeVelocity, CameraZoomSmoothTime);
    }
}
