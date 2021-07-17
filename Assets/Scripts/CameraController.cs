using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public float ZoomOutMultiplier;
    public float CameraZoomSmoothTime;
    public GameObject Subject;

    private float cameraOriginalSize;
    
    private Rigidbody playerRigidbody;
    private float cameraSizeVelocity;

    void Awake ()
    {
        playerRigidbody = Subject.GetComponent<Rigidbody>(); 
    }

	void Start () 
    {
        cameraOriginalSize = Camera.main.orthographicSize;
	}

    void FixedUpdate()
    {
        transform.position = new Vector3(Subject.transform.position.x, transform.position.y, Subject.transform.position.z);
    }

    void Update ()
    {
        //var newSize = cameraOriginalSize + (playerRigidbody.velocity.magnitude * ZoomOutMultiplier);
        //Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, newSize, ref cameraSizeVelocity, CameraZoomSmoothTime);
    }
}
