using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    public Light flashlight;
    public bool lowbat = false;
    public float batterylife = 5f;
    public bool on = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
        {
            while(lowbat == false)
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
                    batterylife -= 1f;
                    Debug.Log(batterylife);
                    if(batterylife == 0)
                    {
                        lowbat = true;
                    }
                }
            }
        }
	}
}
