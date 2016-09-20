using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour 
{
	public List<GameObject> Backgrounds = new List<GameObject>();
	public List<float> BackgroundSpeeds = new List<float>();

    private Material[] backgroundMaterials;

	void Start () 
    {
		backgroundMaterials = new Material[Backgrounds.Count];
		for (int i = 0; i < Backgrounds.Count; i++)
        {
            backgroundMaterials[i] = Backgrounds[i].GetComponent<Renderer>().material;
        }
	}
	
	void Update () 
    {
        for (int i = 0; i < backgroundMaterials.Length; i++ )
        {
            Vector2 offset = new Vector2(transform.position.x / Backgrounds[i].transform.localScale.x, transform.position.y / Backgrounds[i].transform.localScale.y);
			backgroundMaterials[i].mainTextureOffset = new Vector2((offset.x % 1) * BackgroundSpeeds[i], (offset.y % 1) * BackgroundSpeeds[i]);
        }   
	}
}
