//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class HDK_CharacterAnimator : MonoBehaviour {

    public Animator controller;
    FirstPersonController Player;

    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }

    void Update()
    {
        bool security = HDK_RaycastManager.UsingSecurityCam;
        if (Player.enabled)
        {
            //Idle 
            if (Player.m_CharacterController.velocity.sqrMagnitude == 0)
            {
                controller.SetFloat("speed", 0);
                controller.SetBool("run", false);
            }

            //Walking
            if (!Player.isRunning && Player.m_CharacterController.velocity.sqrMagnitude > 0)
            {
                controller.SetFloat("speed", 1);
                controller.SetBool("run", false);
            }

            //Walking
            if (Player.isRunning && !Player.CanRun)
            {
                controller.SetFloat("speed", 1);
                controller.SetBool("run", false);
            }

            //Running
            if (Player.isRunning && Player.m_CanRun && Player.m_CharacterController.velocity.sqrMagnitude > 0)
            {
                controller.SetBool("run", true);
            }
        }
        else
        {
            //Idle while using security camera
            if (security)
            {
                controller.SetFloat("speed", 0);
                controller.SetBool("run", false);
            }
        }
    }
}