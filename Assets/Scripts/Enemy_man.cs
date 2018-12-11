using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_man : MonoBehaviour {
    public GameObject Man;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void resetMan()
    {
        Man.SetActive(true);
    }
    public void Die()
    {
        Man.SetActive(false);
    }
}
