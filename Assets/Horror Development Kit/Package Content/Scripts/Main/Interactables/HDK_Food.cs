//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDK_Food : MonoBehaviour {

    public bool Harmful;
    public float value;
    public AudioClip[] Sounds;
    public float SoundVolume;
    HDK_InteractObject interactScript;
    HDK_PlayerHealth healthScript;
    GameObject Player;


	void Start ()
    {
        Player = GameObject.Find("Player");
        interactScript = GetComponent<HDK_InteractObject>();
        healthScript = Player.GetComponent<HDK_PlayerHealth>();
    }
	

	void Update ()
    {
        if (!Harmful)
        {
            if(healthScript.Health == 100)
            {
                interactScript.Interactable = false;
            }else
            {
                interactScript.Interactable = true;
            }
        }
	}
}
