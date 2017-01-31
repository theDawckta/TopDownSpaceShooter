using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : MonoBehaviour
{
    public event EventHandler DeathEvent;
    public float MaxSpeed = 10.0f;
	public float Acceleration = 10.0f;
    public float TurnSpeed = 10.0f;
    public float HitPoints = 10.0f;
    public float GunCoolDown = 1.0f;
    public ParticleSystem[] Engines;
    public GameObject[] Barrels;
    public GameObject Bullet;
	public GameObject RollGameObject;

    [HideInInspector]
	public Rigidbody2D shipRigidbody;
    [HideInInspector]
    public bool Alive;
    [HideInInspector]
    public bool firing = false;
    [HideInInspector]
    public Vector3 Target = new Vector3();
    
    private int barrelIndex = 0;

    protected virtual void Awake()
	{
		shipRigidbody = transform.GetComponent<Rigidbody2D>();
        Alive = true;
	}

    protected virtual void Start () 
	{
		
	}

    protected virtual void Update()
    {
        if (Alive == false)
        {
            Destroy(gameObject);
        }
    }

	protected virtual void FixedUpdate()
	{
        AddRotation();
		AddRoll();
	}

    public void FireGun()
    {
        if(!firing)
            StartCoroutine("FireGunCoroutine");
    }

    IEnumerator FireGunCoroutine()
    {
        firing = true;

        GameObject NewBullet = (GameObject)Instantiate(Bullet, Barrels[barrelIndex].transform.position, Barrels[barrelIndex].transform.rotation);
        NewBullet.GetComponent<BulletController>().Shooter = gameObject;
        barrelIndex = (barrelIndex + 1 < Barrels.Length) ? barrelIndex + 1 : 0;

        float timePassed = 0.0f;
        while ((timePassed / GunCoolDown) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        firing = false;
    }

	public void AddThrust(Vector3 direction)
	{
        if (shipRigidbody.velocity.magnitude < MaxSpeed)
        {
            shipRigidbody.AddForce(direction * Acceleration);
            for (int i = 0; i < Engines.Length; i++)
            {
                Engines[i].Emit(1);
            }
        }
	}

	public void MoveTarget(float time, Vector3 newPosition)
	{
		StartCoroutine(TransitionTarget(time, newPosition));
	}

    protected void OnDeath()
    {
        DeathEvent(this, EventArgs.Empty);
        Alive = false;
    }

    void AddRoll()
	{
		Vector3 velocityAngle;
        Vector2 shipDirection;
		float angleOffset;

        velocityAngle = shipRigidbody.velocity.normalized;
        shipDirection = transform.position - Target;
        angleOffset = UtilityFunctions.AngleFromAToB(velocityAngle, shipDirection);
		if ((angleOffset > 0.0f && angleOffset < 180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(angleOffset)), 0.0f);
        }
		else if ((angleOffset < 0.0f && angleOffset > -180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, (180 - Mathf.Abs(angleOffset)), 0.0f);
        }
	}

	void AddRotation()
	{
		Quaternion rotate;
		
		rotate = Quaternion.FromToRotation(Vector3.up, Target - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, TurnSpeed);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        BulletController bullet;

        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Bullet")
        {
            bullet = collision.collider.transform.GetComponent<BulletController>();
            if(LayerMask.LayerToName(bullet.Shooter.gameObject.layer) != LayerMask.LayerToName(gameObject.layer))
            {
                HitPoints = HitPoints - 1;
                if (HitPoints <= 0)
                {
                    OnDeath();
                }
            }
        } 
    }

    IEnumerator TransitionTarget(float time, Vector3 newPosition)
    {
        float t = 0.0f;
        Vector3 startingPos = Target;
        while (t < time)
        {
            t += Time.deltaTime * (Time.timeScale / time);
            Target = Vector3.Lerp(startingPos, newPosition, t);
            Debug.DrawLine(startingPos, Target, Color.red);
            yield return 0;
        }
    }
}
