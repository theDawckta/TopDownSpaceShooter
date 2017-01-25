using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    public GameObject Drop;
   
    [HideInInspector]
    public bool Alive = true;

    private List<FuelController> dropList = new List<FuelController>();

    void Start ()
    {
		
	}
	
	void Update ()
    {
        for (int i = 0; i < dropList.Count; i++)
        {
            if (!dropList[i].Alive)
            {
                Destroy(dropList[i].gameObject);
                dropList.Remove(dropList[i]);
            }
        }
    }

    public void MakeDrop(Vector3 location)
    {
        GameObject newDrop = (GameObject)Instantiate(Drop, location, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        dropList.Add(newDrop.GetComponent<FuelController>());
    }

    public void RemoveAllDrops()
    {
        foreach (FuelController fuel in dropList)
        {
            fuel.Alive = false;
        }
    }
}
