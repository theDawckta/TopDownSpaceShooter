using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour 
{
	public float Value;
	[HideInInspector]
	public bool Alive = true;

	void FixedUpdate()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        
		Quaternion rotate;

		rotate = Quaternion.FromToRotation(Vector3.up, mousePosition - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, 10.0f);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);
	}
}
