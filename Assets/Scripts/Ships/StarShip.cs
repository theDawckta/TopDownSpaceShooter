using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : MonoBehaviour {

	public float MaxSpeed = 10.0f;
	public float Acceleration = 10.0f;
    public float TurnSpeed = 10.0f;
	public GameObject RollGameObject;

	private Rigidbody2D shipRigidbody;

	protected virtual void Awake()
	{
		shipRigidbody = transform.GetComponent<Rigidbody2D>();
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		
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

	public void AddRoll(Vector3 position)
	{
		Vector3 velocityAngle;
        Vector2 shipDirection;
		float direction;
		Quaternion rotate;
		
		rotate = Quaternion.FromToRotation(Vector3.up, position - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, TurnSpeed);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);

        velocityAngle = shipRigidbody.velocity.normalized;
        shipDirection = transform.position - position;
        direction = AngleFromAToB(velocityAngle, shipDirection);
        if ((direction > 0.0f && direction < 180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(direction)), 0.0f);
        }
        else if ((direction < 0.0f && direction > -180.0f))
        {
			RollGameObject.transform.localEulerAngles = new Vector3(0.0f, (180 - Mathf.Abs(direction)), 0.0f);
        }

		//Debug.Log( "direction: " + direction + "     velocityAngle" + velocityAngle + "     angle:" + RollRotation.transform.localEulerAngles.y);
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
