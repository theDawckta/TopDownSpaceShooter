using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour 
{
    public GameObject Subject;
    public List<GameObject> Backgrounds = new List<GameObject>();
    public List<float> BackgroundSpeeds = new List<float>();

    private Material[] _backgroundMaterials;

    void Start () 
    {
        _backgroundMaterials = new Material[Backgrounds.Count];
        for (int i = 0; i < Backgrounds.Count; i++)
        {
            _backgroundMaterials[i] = Backgrounds[i].GetComponent<Renderer>().material;
        }
    }
    
    void Update () 
    {
        transform.position = Subject.transform.position;
        for (int i = 0; i < _backgroundMaterials.Length; i++)
        {
            Vector2 offset = new Vector2(Subject.transform.position.x / Backgrounds[i].transform.localScale.x, Subject.transform.position.y / Backgrounds[i].transform.localScale.y);
			_backgroundMaterials[i].SetTextureOffset("_MainTex", new Vector2((offset.x) * BackgroundSpeeds[i], (offset.y) * BackgroundSpeeds[i]));
          	_backgroundMaterials[i].SetTextureOffset("_BumpMap", new Vector2((offset.x) * BackgroundSpeeds[i], (offset.y) * BackgroundSpeeds[i]));

            Vector2 newOffset = _backgroundMaterials[i].GetTextureOffset("_MainTex");
        }   
    }
}