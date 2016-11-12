using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : StarShip
{
    public delegate void OnPlayerDiedEvent();
    public event OnPlayerDiedEvent OnPlayerDied;
    [HideInInspector]
    public bool PlayerEnabled = false;
    private Vector3 originalPosition;

	void Awake()
    {
        originalPosition = gameObject.transform.position;
        base.Awake();
    }

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            FireGun();
        }
    }

    void FixedUpdate()
    {
		base.Target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

		if (Input.GetAxisRaw("Vertical") > 0)
        {
            base.AddThrust(transform.up);
            Debug.Log("ShipThrust " +  Acceleration);
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