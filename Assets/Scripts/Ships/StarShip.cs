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
    public GameObject StarShipTarget;

    private Vector3 _positionYZerodOut;
    private Vector3 _targetPositionYZerodOut;
    private int barrelIndex = 0;
    private float yVelocity = 0.0f;
    private float newYAngle = 0.0f;
    private float _hitPoints = 0.0f;

    protected virtual void Awake()
	{
		ShipRigidbody = transform.GetComponent<Rigidbody>();
        ShipCollider = transform.GetComponent<Collider>();
        StarShipTarget = new GameObject();
        StarShipTarget.name = "StarShipTarget";
        StarShipTarget.transform.parent = this.transform;
        _hitPoints = HitPoints;
    }

    protected virtual void Start () 
	{
		
	}

    protected virtual void Update()
    {

    }

	protected virtual void FixedUpdate()
	{
        AddRotation();

        Vector3 newRollTarget = GetRollTargetAngles();
        newYAngle = Mathf.SmoothDampAngle(RollGameObject.transform.localEulerAngles.z, newRollTarget.z, ref yVelocity, rollSmooth);
        RollGameObject.transform.localEulerAngles = new Vector3(newRollTarget.x, newRollTarget.y, newYAngle);
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
        Vector2 shipDirection;
		float angleOffset;

        velocityAngle = ShipRigidbody.velocity.normalized;
        shipDirection = Vector3.Normalize(transform.position - StarShipTarget.transform.position);
        angleOffset = UtilityFunctions.AngleFromAToB(velocityAngle, shipDirection);

		if ((angleOffset > 0.0f && angleOffset < 180.0f))
        {
			return new Vector3(0.0f, 0.0f, -(180 - Mathf.Abs(angleOffset)));
        }
		else
        {
			return new Vector3(0.0f, 0.0f, (180 - Mathf.Abs(angleOffset)));
        }
	}

	void AddRotation()
	{
		Quaternion rotate;

        _targetPositionYZerodOut = new Vector3(StarShipTarget.transform.position.x, 0.0f, StarShipTarget.transform.position.z);
        _positionYZerodOut = new Vector3(transform.position.x, 0.0f, transform.position.z);

        rotate = Quaternion.FromToRotation(Vector3.forward, _targetPositionYZerodOut - _positionYZerodOut);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, TurnSpeed);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
	}

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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
        Vector3 startingPos = StarShipTarget.transform.position;
        while (t < time)
        {
            t += Time.deltaTime * (Time.timeScale / time);
            StarShipTarget.transform.position = Vector3.Lerp(startingPos, newPosition, t);
            Debug.DrawLine(startingPos, StarShipTarget.transform.position, Color.red);
            yield return 0;
        }
    }
}
