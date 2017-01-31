using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : StarShip
{
    public delegate void OnPlayerDiedEvent();
    public event OnPlayerDiedEvent OnPlayerDied;
	public delegate void OnPlayerFuelPickupEvent();
    public event OnPlayerFuelPickupEvent OnPlayerFuelPickup;
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

    protected override void Start()
    {
       DisablePlayer();
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
		Target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

		if (Input.GetAxisRaw("Vertical") > 0)
        {
            base.AddThrust(transform.up);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            base.AddThrust(-transform.up);
        }

        base.FixedUpdate();
   	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

    IEnumerator PlayerDied()
    {
        OnPlayerDied();
        yield return null;
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
}