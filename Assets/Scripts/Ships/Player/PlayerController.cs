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

    private Vector3 originalPosition;

    protected override void Awake()
    {
        originalPosition = gameObject.transform.position;
        base.Awake();
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
        StarShipTarget.transform.position = new Vector3(newTarget.x, 0f, newTarget.z);


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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        base.OnCollisionEnter2D(collision);
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
        gameObject.transform.position = originalPosition;
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