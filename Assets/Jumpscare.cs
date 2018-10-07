using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour {
    public GameObject e;
    public bool entered;
	// Use this for initialization
    public AudioClip alarm;
    private AudioSource source;
 
     // Use this for initialization
     void Start () {
         entered = false;
         source = GetComponent<AudioSource>();
     }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {   
        //checks whether the player has already entered the trigger
        if(entered == false){
            if(other.tag == "Player")
            {
                //sets to true so it plays only once
                entered = true;
                Instantiate(e, transform.position, transform.rotation);
                source.PlayOneShot (alarm);  
                Destroy(gameObject, alarm.length);
                
            }
        }
    }
}
