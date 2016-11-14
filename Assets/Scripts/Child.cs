using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Parent 
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        myTransform = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        base.FixedUpdate();
    }
}
