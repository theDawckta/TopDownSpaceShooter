using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public StarShip Player;
    public float zoomOutMultiplier;

    private float cameraOriginalSize;

	void Start () 
    {
        cameraOriginalSize = Camera.main.orthographicSize;
	}

    void Update ()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        Camera.main.orthographicSize = cameraOriginalSize + (Player.shipRigidbody.velocity.magnitude * zoomOutMultiplier);
    }
}
