using System;
using System.Collections;
using UnityEngine;

public class StarShip : MonoBehaviour
{
    public delegate void OnDeathEvent(StarShip ship);
    public event OnDeathEvent OnDeath;

    public float MaxSpeed = 10.0f;
	public float Acceleration = 10.0f;
    public float TurnSpeed = 10.0f;
    public float HitPoints = 5.0f;
    public float GunCoolDown = 1.0f;
    public float rollSmooth = 0.3f;
    public GameObject[] Barrels;
    public GameObject Bullet;
	public GameObject RollGameObject;

    [HideInInspector]
	public Rigidbody ShipRigidbody;
    [HideInInspector]
    public Collider ShipCollider;
    [HideInInspector]
    public bool Firing = false;
    [HideInInspector]
    public GameObject StarShipDestTarget;

    private int barrelIndex = 0;
    private float yVelocity = 0.0f;
    private float newZAngle = 0.0f;
    private float _hitPoints = 0.0f;

    protected virtual void Awake()
	{
		ShipRigidbody = transform.GetComponent<Rigidbody>();
        ShipCollider = transform.GetComponent<Collider>();
        StarShipDestTarget = new GameObject();
        StarShipDestTarget.SetActive(false);
        StarShipDestTarget.name = "StarShipDestTarget";
        StarShipDestTarget.AddComponent<BoxCollider>().isTrigger = true;
        StarShipDestTarget.transform.SetParent(transform, false);
        StarShipDestTarget.transform.localScale = Vector3.one;
        _hitPoints = HitPoints;
    }

    protected virtual void Start() 
	{
		
	}

    protected virtual void Update()
    {

    }

	protected virtual void FixedUpdate()
	{
        Vector3 newRollTarget = GetRollTargetAngles();
        newZAngle = Mathf.SmoothDampAngle(RollGameObject.transform.localEulerAngles.z, newRollTarget.z, ref yVelocity, rollSmooth);
        RollGameObject.transform.localEulerAngles = new Vector3(newRollTarget.x, newRollTarget.y, newZAngle);
    }

    public void FireGun()
    {
        if(!Firing)
            StartCoroutine("FireGunCoroutine");
    }

    IEnumerator FireGunCoroutine()
    {
        Firing = true;

        GameObject NewBullet = (GameObject)Instantiate(Bullet, Barrels[barrelIndex].transform.position, Barrels[barrelIndex].transform.rotation);
        NewBullet.GetComponent<BulletController>().Shooter = gameObject;
        barrelIndex = (barrelIndex + 1 < Barrels.Length) ? barrelIndex + 1 : 0;

        float timePassed = 0.0f;
        while ((timePassed / GunCoolDown) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        Firing = false;
    }

    public void AddThrust(Vector3 direction)
    {
        if (ShipRigidbody.velocity.magnitude < MaxSpeed)
        {
            ShipRigidbody.AddForce(direction * Acceleration);
        }
    }

    public void AddThrust(ParticleSystem[] Engines)
    {
        for (int i = 0; i < Engines.Length; i++)
        {
            ShipRigidbody.AddForce(-Engines[i].transform.forward * Acceleration);
            for (int j = 0; j < Engines.Length; j++)
            {
                Engines[j].Emit(5);
            }
        }
    }

	public void MoveTarget(float time, Vector3 newPosition)
	{
		StartCoroutine(TransitionTarget(time, newPosition));
	}

    private Vector3 GetRollTargetAngles()
    {
        Vector3 velocityAngle;
        Vector3 shipDirection;
        float angleOffset;

        velocityAngle = ShipRigidbody.velocity.normalized;
        shipDirection = transform.position - StarShipDestTarget.transform.position;
        angleOffset = UtilityFunctions.AngleFromAToB(velocityAngle, shipDirection);

        if ((angleOffset > 0.0f && angleOffset < 180.0f))
        {
            return new Vector3(0.0f, 0.0f, (180 - Mathf.Abs(angleOffset)));
        }
        else
        {
            return new Vector3(0.0f, 0.0f, -(180 - Mathf.Abs(angleOffset)));
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TRIGGER COLLISION " + other.name );
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        BulletController bullet;

        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "EnemyBullet" || LayerMask.LayerToName(collision.collider.gameObject.layer) == "PlayerBullet")
        {
            bullet = collision.collider.transform.GetComponent<BulletController>();
            if(bullet.Shooter != null && LayerMask.LayerToName(bullet.Shooter.gameObject.layer) != LayerMask.LayerToName(gameObject.layer))
            {
                _hitPoints = _hitPoints - 1;
                if (_hitPoints <= 0)
                {
                    ShipCollider.enabled = false;
                    OnDeath(this);
                    _hitPoints = HitPoints;
                }
            }
        } 
    }

    IEnumerator TransitionTarget(float time, Vector3 newPosition)
    {
        float t = 0.0f;
        Vector3 startingPos = StarShipDestTarget.transform.position;
        while (t < time)
        {
            t += Time.deltaTime * (Time.timeScale / time);
            StarShipDestTarget.transform.position = Vector3.Lerp(startingPos, newPosition, t);
            Debug.DrawLine(startingPos, StarShipDestTarget.transform.position, Color.red);
            yield return 0;
        }
    }
}
