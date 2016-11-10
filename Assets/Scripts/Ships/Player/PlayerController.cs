using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : StarShip
{
    public float GunCoolDown = 1.0f;
    public GameObject Barrel1;
	public GameObject Barrel2;
    public GameObject PlayerBullet;
    public delegate void OnPlayerDiedEvent();
    public event OnPlayerDiedEvent OnPlayerDied;
    [HideInInspector]
    public bool PlayerEnabled = false;
    private bool firing = false;
    private bool barrelCycler = true;
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
        if (Input.GetButton("Fire1") && !firing)
        {
            StartCoroutine("FireGun");
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

    IEnumerator FireGun()
    {
        firing = true;
        barrelCycler = !barrelCycler;
        GameObject playerBullet;

        if(barrelCycler)
			playerBullet = Instantiate<GameObject>(PlayerBullet, Barrel1.transform.position, Barrel1.transform.rotation);
		else
			playerBullet = Instantiate<GameObject>(PlayerBullet, Barrel2.transform.position, Barrel2.transform.rotation);

        float timePassed = 0.0f;
        while ((timePassed / GunCoolDown) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        firing = false;
    }

    float AngleFromAToB(Vector3 angleA, Vector3 angleB)
    {
        if (angleA == Vector3.zero || angleB == Vector3.zero)
            return 0.0f;
        Vector3 axis = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(angleA, angleB);
        float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(angleA, angleB)));

        // angle in [-179,180]
        float signed_angle = angle * sign;
        return signed_angle;
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