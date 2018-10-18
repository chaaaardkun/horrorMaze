using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour { 
    public bool entered;
	// Use this for initialization
    //public AudioClip scareSound;
    public GameObject scareModel;
    public AudioSource scareSound;
    public GameObject flicker;
    public GameObject cam;
 
     // Use this for initialization
     void Start () {
         entered = false;
     }

    void OnTriggerEnter()
    {   
        //checks whether the player has already entered the trigger
        if(entered == false){
                //sets to true so it plays only once
                entered = true;

                scareSound.Play();
                //jumpCam.SetActive(true);
                scareModel.SetActive (true);
                flicker.SetActive(true);
                Camera.main.fieldOfView = 25;
                StartCoroutine (EndJump ());
                /* 
                Instantiate(e, transform.position -(transform.forward*3), transform.rotation);
                source.PlayOneShot (alarm);  
                Destroy(gameObject, alarm.length);*/
        }
    }
    IEnumerator EndJump(){
        yield return new WaitForSeconds(0.8f);
        scareModel.SetActive(false);
        yield return new WaitForSeconds(2.03f);
        Camera.main.fieldOfView = 53;
        scareModel.SetActive(false);
        flicker.SetActive(false);
    }

}
