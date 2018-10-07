using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    public Light flashlight;
    bool on = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
        {
            if (on == true)
            {
                flashlight.enabled = false;
                on = false;
            }
            else
            {
                flashlight.enabled = true;
                on = true;
            }
        }
	}
}
