using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class FSMStateTest
{
    
}

class ChasePlayerStateTest : FSMStateTest
{

}

class AttackPlayerStateTest : FSMStateTest
{
    
}

public class TestScript : MonoBehaviour
{

    private List<FSMStateTest> myList = new List<FSMStateTest>();
    
	void Start ()
    {
        myList.Add(new ChasePlayerStateTest());
        myList.Add(new AttackPlayerStateTest());

        foreach (FSMStateTest state in myList)
        {
            Debug.Log("WHAY CAN'T I FIND ONE!");
        }
	}
}
