using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public float ZoomOutMultiplier;
    public float CameraZoomSmoothTime;

    private float cameraOriginalSize;
    private GameObject player;
    private StarShip playerStarShip;
    private float maxSize;
    private Rigidbody2D playerRigidbody;
    private float cameraSizeVelocity;

    void Awake ()
    {
        player = GameObject.FindWithTag("Player");
        playerStarShip = player.GetComponent<StarShip>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        maxSize = playerStarShip.MaxSpeed * ZoomOutMultiplier;
    }

	void Start () 
    {
        cameraOriginalSize = Camera.main.orthographicSize;
	}

    void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    void Update ()
    {
        float newSize = cameraOriginalSize + (playerRigidbody.velocity.magnitude * ZoomOutMultiplier);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, newSize, ref cameraSizeVelocity, CameraZoomSmoothTime);
    }
}
