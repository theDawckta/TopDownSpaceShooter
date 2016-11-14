using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour 
{
    [HideInInspector]
    [NonSerialized]
    public Vector3 myTransform;

    protected virtual void Awake()
    {
        if (myTransform != null)
            myTransform = Vector3.zero;
    }

    protected virtual void FixedUpdate()
    {
        AddRotation();
    }

    void AddRotation()
    {
        Quaternion rotate;

        rotate = Quaternion.FromToRotation(Vector3.up, myTransform - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, 10.0f);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);
    }
}
