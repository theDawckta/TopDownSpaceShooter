using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : StarShip
{
    public delegate void OnPlayerFuelPickupEvent();
    public event OnPlayerFuelPickupEvent OnPlayerFuelPickup;

    public ParticleSystem[] FrontEngines;
    public ParticleSystem[] RearEngines;
    public ParticleSystem[] RearRightEngines;
    public ParticleSystem[] RearLeftEngines;

    [HideInInspector]
    public bool PlayerEnabled = false;
    [HideInInspector]
    public float PlayerFuelLevel = 0.0f;

    private Vector3 _originalPosition;
    private Vector3 _positionYZerodOut;
    private Vector3 _targetPositionYZerodOut;

    protected override void Awake()
    {
        _originalPosition = gameObject.transform.position;
        base.Awake();
    }

    protected override void Start()
    {
        if(StarShipAnimator != null)
            StarShipAnimator.enabled = false;
        if(StarShipNavMeshAgent != null)
            StarShipNavMeshAgent.enabled = false;
    }

    protected override void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            FireGun();
        }
    }

    protected override void FixedUpdate()
    {
		var newTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
        StarShipDestTarget.transform.position = new Vector3(newTarget.x, 0f, newTarget.z);

        AddRotation();

        if (Input.GetKey(KeyCode.W))
            base.AddThrust(RearEngines);
        if (Input.GetKey(KeyCode.S))
            base.AddThrust(FrontEngines);
        if (Input.GetKey(KeyCode.D))
            base.AddThrust(RearRightEngines);
        if (Input.GetKey(KeyCode.A))
            base.AddThrust(RearLeftEngines);
        base.FixedUpdate();
   	}

    void AddRotation()
    {
        Quaternion rotate;

        _targetPositionYZerodOut = new Vector3(StarShipDestTarget.transform.position.x, 0.0f, StarShipDestTarget.transform.position.z);
        _positionYZerodOut = new Vector3(transform.position.x, 0.0f, transform.position.z);

        rotate = Quaternion.FromToRotation(Vector3.forward, _targetPositionYZerodOut - _positionYZerodOut);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, TurnSpeed);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        base.OnCollisionEnter(collision);
    }

	void OnTriggerEnter2D(Collider2D collider)
    {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Fuel"))
        {
			FuelController thisFuel = collider.gameObject.GetComponent<FuelController>();
			PlayerFuelLevel = PlayerFuelLevel + thisFuel.Value;
			thisFuel.Alive = false;
			OnPlayerFuelPickup();
        }
    }

    void PlayerDied(StarShip ship)
    {
        
    }

    public void EnablePlayer()
    {
        gameObject.SetActive(true);
    }

    public void DisablePlayer()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _originalPosition;
    }

    public void OnEnable()
    {
        base.OnDeath += PlayerDied;
    }

    public void OnDisable()
    {
        base.OnDeath -= PlayerDied;
    }
}