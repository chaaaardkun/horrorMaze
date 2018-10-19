//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class HDK_Footsteps : MonoBehaviour 
{
	[Header ("Footsteps System")]
	public List<GroundType> GroundTypes = new List<GroundType>();
	FirstPersonController FPC;
	public string currentGround;
	public static bool CanRun;

	void Start()
	{
		FPC = gameObject.GetComponent<FirstPersonController> ();
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.transform.tag == "Concrete") {
			setGroundType (GroundTypes [2]);
		} else if (hit.transform.tag == "Dirt") {
			setGroundType (GroundTypes [1]);
		} else if (hit.transform.tag == "Wood") {
			setGroundType (GroundTypes [0]);
		}
	}

	public void setGroundType(GroundType ground)
	{
		if(currentGround != ground.name)
		{
			FPC.m_FootstepSounds = ground.footstepsounds;
			FPC.m_WalkSpeed = ground.walkSpeed;
			FPC.m_RunSpeed = ground.runSpeed;
			currentGround = ground.name;
            if (!FPC.m_onLadder)
            {
                FPC.m_CanRun = ground.canRunHere;
            }else
            {
                FPC.m_CanRun = false;
            }
			CanRun = ground.canRunHere;
		}
	}

   /* private void Update()
    {
        if (FPC.m_onLadder || FPC.falling)
        {      
            FPC.m_CanRun = false;
        }
    }*/
}

[System.Serializable]
public class GroundType
{
	public string name;
	//You need atleast 2 sounds
	public AudioClip[] footstepsounds;
	//You can add here much more, such as land and jump sounds
	public float walkSpeed = 5;
	public float runSpeed = 10;
	public bool canRunHere;
}