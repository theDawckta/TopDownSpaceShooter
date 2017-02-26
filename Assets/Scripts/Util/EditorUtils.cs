using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorUtils : MonoBehaviour
{
    public GameObject Target1;
    public GameObject Target2;

    public bool OutputDistance = false;

    void Update()
    {
        WriteDistance();
    }

    public void WriteDistance()
    {
        if(Target1 != null && Target2 != null && OutputDistance)
        Debug.Log(Vector3.Distance(Target1.transform.position, Target2.transform.position));
    }
}