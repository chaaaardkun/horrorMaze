//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HDK_PlayerHealth : MonoBehaviour {

	[Header ("Health Settings")]
	public float Health = 100f;
	public float maxHealth = 100f;
	public bool Regeneration;
	public float RegenerationSpeed;

    [Header ("Falling Damage")]
    public AudioSource Falling_audioSource;
    public float FallingDamageMultiplier;
    float vol;

    [Header("Heart SFX")]
    public AudioClip HeartSound;
    public AudioSource Heart_audioSource;

    [Header("Hurt SFX")]
    public AudioClip[] HurtsSounds;
    public AudioSource Hurts_audioSource;
    public float Hurts_volume;

    [Header ("Dead Replacement")]
	public GameObject deadReplacement;
	
	Animator HUD_Health;
    GameObject damageGUI;
    GameObject HUDicon;
    CharacterController controller;
    
    void Start () 
	{
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        damageGUI = GameObject.Find ("HealthDamage");
		HUDicon = GameObject.Find ("Circle");
		HUD_Health = GameObject.Find("Heart").GetComponent<Animator>();
        Heart_audioSource.clip = HeartSound;
        Heart_audioSource.Play();
	}

	void Update () 
	{
        if (Health == 100) {
            HUD_Health.speed = 1f;
        }
        else if(Health > 70 && Health < 90)
        {
            HUD_Health.speed = 1.5f;
        }
        else if (Health > 50 && Health < 70)
        {
            HUD_Health.speed = 2f;
        }
        else if (Health > 25 && Health < 50)
        {
            HUD_Health.speed = 2.5f;
        }
        else if (Health > 0 && Health < 25)
        {
            HUD_Health.speed = 3f;
        }

        if (Regeneration) {
			Health += Time.deltaTime * RegenerationSpeed;
		}

		float difference = maxHealth - Health;
		damageGUI.GetComponent<CanvasGroup> ().alpha = difference / 100f;
        Heart_audioSource.volume = difference / 100f;

		if (Health >= maxHealth) {
			Health = maxHealth;
		} else if (Health <= 0) {
			Health = 0;
			Die();
		}

        //If you want to test or add an input to apply damage to the player
        //Just uncomment the lines below

        /*
        if (Input.GetKeyDown (KeyCode.P)) {
		    Health -= 10f;
		}
        */

		float health = Health / 100f;
		HUDicon.GetComponent<Image> ().fillAmount = health;

        //Falling sound effect
        float y = -controller.velocity.y;

        if (y > 5)
        {
            vol = Mathf.Lerp(vol, 1.0f, Time.deltaTime / 2);
        }
        else
        {
            vol = Mathf.Lerp(vol, 0.0f, Time.deltaTime * 3);
        }

        Falling_audioSource.volume = vol;
        Falling_audioSource.pitch = 1 + vol;
        //
    }

	void Die()
	{
		Instantiate(deadReplacement, transform.position, transform.rotation);
		Destroy(gameObject);
		Destroy(GameObject.Find("Canvas"));
	}

	public void Damage(float damage)
	{
		Health -= damage;
        Hurts_audioSource.clip = HurtsSounds[Random.Range(0, HurtsSounds.Length)];
        Hurts_audioSource.volume = Hurts_volume;
        Hurts_audioSource.Play();
    }
}