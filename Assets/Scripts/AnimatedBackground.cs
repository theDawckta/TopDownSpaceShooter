using UnityEngine;
using System.Collections;

public class AnimatedBackground : MonoBehaviour {

	public float Speed = 10.0f;

	private Vector2 offset = Vector2.zero;
	private Material material;

	// Use this for initialization

	void Start () {
		material = transform.GetComponent<Renderer>().material;
	}

	// Update is called once per frame
	void Update () {
		Vector2 direction = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")); 
		offset = offset + (direction * (Speed * Time.deltaTime));


		material.SetTextureOffset ("_MainTex", offset);
	}
}
