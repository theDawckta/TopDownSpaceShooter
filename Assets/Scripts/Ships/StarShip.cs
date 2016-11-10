using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : MonoBehaviour {

	public float MaxSpeed = 10.0f;
	public float Acceleration = 10.0f;
    public float TurnSpeed = 10.0f;
    public float HitPoints = 10.0f;
	public GameObject RollGameObject;

    [HideInInspector]
    public GameObject Target;
    [HideInInspector]
	public Rigidbody2D shipRigidbody;
    [HideInInspector]
    public bool Alive;

	protected virtual void Awake()
	{
        Target = new GameObject();
		shipRigidbody = transform.GetComponent<Rigidbody2D>();
        Alive = true;
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		
	}

	protected virtual void FixedUpdate()
	{
		AddRotation();
		AddRoll();
	}

	public void AddThrust(Vector3 direction)
	{
		float xVel = transform.InverseTransformDirection(shipRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(shipRigidbody.velocity).y;

		if ((xVel + yVel) < MaxSpeed)
        {
			shipRigidbody.AddForce(direction * Acceleration);
        }
	}

	public void MoveTarget(float time, Vector3 newPosition)
	{
		StartCoroutine(TransitionTarget(time, newPosition));
	}

	void AddRoll()
	{
		Vector3 velocityAngle;
        Vector2 shipDirection;
		float angleOffset;

        velocityAngle = shipRigidbody.velocity.normalized;
        shipDirection = transform.position - Target.transform.position;
        angleOffset = AngleFromAToB(velocityAngle, shipDirection);
		if ((angleOffset > 0.0f && angleOffset < 180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(angleOffset)), 0.0f);
        }
		else if ((angleOffset < 0.0f && angleOffset > -180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, (180 - Mathf.Abs(angleOffset)), 0.0f);
        }

		//Debug.Log( "direction: " + direction + "     velocityAngle" + velocityAngle + "     angle:" + RollRotation.transform.localEulerAngles.y);
	}

	void AddRotation()
	{
		Quaternion rotate;
		
		rotate = Quaternion.FromToRotation(Vector3.up, Target.transform.position - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, TurnSpeed);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            HitPoints = HitPoints - 1;
            if (HitPoints <= 0)
            {
                Alive = false;
                Debug.Log("ship down");
            }
        }
    }

	IEnumerator TransitionTarget(float time, Vector3 newPosition)
	{
	    float t = 0.0f;
	    Vector3 startingPos = Target.transform.position;
	    while (t < time)
	    {
			t += Time.deltaTime * (Time.timeScale/time);
			Target.transform.position = Vector3.Lerp(startingPos, newPosition, t);
			Debug.DrawLine(startingPos, Target.transform.position ,Color.red);
			yield return 0;
		}

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
}
