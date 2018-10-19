//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class HDK_Enemy : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent nav;
    Transform player;
    Animator controller;

    [Header("Walking Speed")]
	public float minSpeed;
	public float maxSpeed;

    [Header("Running Speed")]
    public float minRunSpeed;
    public float maxRunSpeed;

    [Header("Distances Settings")]
    public float stoppingDistance = 1.8f;
    public float attackRange = 2.8f;
    public float runDistance;
    public float chaseDistance;
    float distance;              
    bool chasing;
    bool run;
    bool PlayerAlive;

    [Header("Patrol Settings")]
    public Transform[] points;
    private int destPoint = 0;
    bool followingPlayer;        
    bool playerHidden;
    bool playerPaused;
    HDK_Hiding hideScript;
    HDK_PauseManager pauseScript;

    [Header("Attack Settings")]
    public float MinDamage;
	public float MaxDamage;
    public float HitShakeValue;
    public float timeToAttack;
	float attackTimer;

    [Header("Attack SFX Settings")]
    public AudioClip[] attackSounds;
	public AudioSource attackAS;
    public float attackVolume;

    [Header("Footsteps SFX Settings")]
    public AudioClip[] footstepsSounds;
    public AudioSource footstepsAS;
    public float footstepsVolume;

    [Header("Spotted SFX Settings")]
    public AudioClip[] spottedSounds;
    public AudioSource spottedAS;
    public float spottedVolume;
    bool playSpotted;
    bool spottedPlayed;

    [Header("Music SFX Settings")]
    public AudioClip[] musicSounds;
    public AudioSource musicAS;
    public float maxVolume;
    float maxVolume2;
    
    void Awake()
    {
		nav = GetComponent <UnityEngine.AI.NavMeshAgent>();
        controller = GetComponentInParent<Animator>();
        player = GameObject.Find("Player").transform;
    }

    void Start()
    {
        maxVolume2 = maxVolume;
        hideScript = player.gameObject.GetComponent<HDK_Hiding>();
        nav.autoBraking = false;
        GotoNextPoint();
        musicAS.clip = musicSounds[Random.Range(0, musicSounds.Length)];
        musicAS.Play();
        pauseScript = GameObject.Find("Canvas").GetComponent<HDK_PauseManager>();
    }

    void GotoNextPoint()
    {
        controller.SetBool("isChasing", true);
        nav.speed = Random.Range(minSpeed / 2, maxSpeed / 2);

        if (points.Length == 0)
            return;

        if (!run)
        {
            controller.SetBool("Run", false);
        }
        else
        {
            controller.SetBool("Run", true);
        }

        nav.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }

    void ChaseTarget()
    {
        GetComponent<HDK_Billboard>().enabled = true;
        playerHidden = hideScript.Hiding;

        if (distance >= stoppingDistance)
        {
            chasing = true;
        }
        else
        {
            chasing = false;
        }

        if (chasing)
        {
            nav.enabled = true;
            nav.SetDestination(player.position);
            controller.SetBool("MovingAttack", false);
            controller.SetBool("isChasing", true);
            if (!run)
            {
                nav.speed = Random.Range(minSpeed, maxSpeed);
                controller.SetBool("Run", false);
            }else
            {
                nav.speed = Random.Range(minRunSpeed, maxRunSpeed);
                controller.SetBool("Run", true);
            }

            if (!footstepsAS.isPlaying)
            {
                footstepsAS.clip = footstepsSounds[Random.Range(0, footstepsSounds.Length)];
                footstepsAS.volume = footstepsVolume;
                footstepsAS.Play();
            }
        }
        else
        {
            if (player != null && player.GetComponent<FirstPersonController>().m_CharacterController.velocity.sqrMagnitude == 0 && !playerHidden)
            {
                controller.SetBool("isChasing", false);
                controller.SetBool("MovingAttack", false);
                controller.SetBool("Run", false);
                nav.enabled = false;
            }

            controller.SetBool("MovingAttack", true);

            if (Attack())
            {
                player.SendMessageUpwards("Damage", Random.Range(MinDamage,MaxDamage), SendMessageOptions.RequireReceiver);
                player.SendMessage("Shake", HitShakeValue);
            }
        }
    }

    void PlaySpottedSound()
    {
        spottedAS.clip = spottedSounds[Random.Range(0, spottedSounds.Length)];
        spottedAS.volume = spottedVolume;
        spottedAS.Play();
        playSpotted = false;
    }
        
    void Update() 
	{
        musicAS.volume = Mathf.Lerp(musicAS.volume, maxVolume, Time.deltaTime);

        if (playSpotted)
        {
            PlaySpottedSound();
            spottedPlayed = true;
        }

        if (!followingPlayer)
        {
            if (followingPlayer)
            {
                spottedPlayed = false;
            }
        }

        playerHidden = hideScript.Hiding;

        if (player != null)
        {
            distance = Vector3.Distance(player.position, transform.position);
        }

        if (PlayerAlive)
        {
            if (distance >= chaseDistance || playerHidden)
            {
                maxVolume = 0;
                followingPlayer = false;
                nav.destination = points[destPoint].position;
                controller.SetBool("Run", false);
                controller.SetBool("MovingAttack", false);
                GetComponent<HDK_Billboard>().enabled = false;
                if (!footstepsAS.isPlaying)
                {
                    footstepsAS.clip = footstepsSounds[Random.Range(0, footstepsSounds.Length)];
                    footstepsAS.volume = footstepsVolume;
                    footstepsAS.Play();
                }
                if (nav.remainingDistance < 0.5f)
                {
                    GotoNextPoint();
                }
            }
            else
            {
                if (!spottedPlayed)
                {
                    playSpotted = true;
                }
                maxVolume = maxVolume2;
                followingPlayer = true;
                ChaseTarget();
            }

            if (followingPlayer)
            {
                if (distance >= runDistance)
                {
                    run = true;
                }else
                {
                    run = false;
                }
            }
        }

        if (!playerPaused)
        {
            if (player.gameObject != null)
            {
                PlayerAlive = true;
            }
            else
            {
                PlayerAlive = false;
            }
        }

        playerPaused = pauseScript.GamePaused;

        if (nav.enabled)
        {
            if (playerPaused)
            {
                nav.Stop();
            }
            else
            {
                nav.Resume();
            }
        }
                
        if (distance <= attackRange && !playerPaused)
        {
            attackTimer += Time.deltaTime;
        }
        else if (attackTimer > 0)
        {
            controller.SetBool("Attack", false);
            attackTimer -= Time.deltaTime * 2;
        }
        else
        {
            attackTimer = 0;
            controller.SetBool("Attack", false);
        }
	}

	bool Attack()
	{
		if(!controller.GetBool ("Attack"))
			controller.SetFloat ("AttackType",Random.Range (0,3));

		controller.SetBool ("Attack", true);

		if(!attackAS.isPlaying && attackTimer > timeToAttack)
		{
            attackAS.clip = attackSounds[Random.Range (0, attackSounds.Length)];
            attackAS.volume = attackVolume;
            attackAS.Play ();
		}

		if(attackTimer > timeToAttack)
		{
			attackTimer = 0;
			return true;
		}
		return false;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "DoorAI")
        {
            GameObject trigger = col.gameObject;
            if (trigger.GetComponentInParent<HDK_DynamicObject>().isDoor)
            {
                if (trigger.GetComponentInParent<HDK_DynamicObject>().Free && !trigger.GetComponentInParent<HDK_DynamicObject>().doorOpen)
                {
                    trigger.GetComponentInParent<HDK_DynamicObject>().doorOpenClose();
                }
            }
        }
    } 
}