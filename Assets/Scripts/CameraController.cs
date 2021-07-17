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
        cameraOriginalSize = Camera.main.fieldOfView;
	}

    void FixedUpdate()
    {
        transform.position = new Vector3(Subject.transform.position.x, transform.position.y, Subject.transform.position.z);
    }

    void Update ()
    {
        var newSize = cameraOriginalSize + (playerRigidbody.velocity.magnitude * ZoomOutMultiplier);
        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, newSize, ref cameraSizeVelocity, CameraZoomSmoothTime);
    }
}
