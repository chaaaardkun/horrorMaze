using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public bool triggerEntered = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
			if(triggerEntered == false){
				Debug.Log("changing to true");
				triggerEntered = true;
			}
			else{
				Debug.Log("Game Over!");
			}
		}
		
	}
}
