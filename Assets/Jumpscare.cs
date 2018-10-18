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
                StartCoroutine (EndJump ());
                /* 
                Instantiate(e, transform.position -(transform.forward*3), transform.rotation);
                source.PlayOneShot (alarm);  
                Destroy(gameObject, alarm.length);*/
        }
    }
    private void OnTriggerExit()
    {
        entered = false;
    }
    IEnumerator EndJump(){
        yield return new WaitForSeconds(2.03f);
        scareModel.SetActive(false);
        flicker.SetActive(false);
    }

}
