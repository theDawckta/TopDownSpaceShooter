using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public GameObject[] Backgrounds;
    public float BackgroundSpeedMultiplier;
    private Material[] BackgroundMaterials;
    private float[] BackgroundSpeeds;

	void Start () 
    {
        BackgroundMaterials = new Material[Backgrounds.Length];
        BackgroundSpeeds = new float[Backgrounds.Length];
        for (int i = 0; i < Backgrounds.Length; i++)
        {
            BackgroundMaterials[i] = Backgrounds[i].GetComponent<Renderer>().material;
            BackgroundSpeeds[i] = BackgroundSpeedMultiplier * (Backgrounds.Length - i);
        }
	}
	
	void Update () 
    {
        for (int i = 0; i < BackgroundMaterials.Length; i++ )
        {
            Vector2 offset = new Vector2(transform.position.x / Backgrounds[i].transform.localScale.x, transform.position.y / Backgrounds[i].transform.localScale.y);
            BackgroundMaterials[i].mainTextureOffset = new Vector2(offset.x % 1, offset.y % 1);
        }
            
	}
}
