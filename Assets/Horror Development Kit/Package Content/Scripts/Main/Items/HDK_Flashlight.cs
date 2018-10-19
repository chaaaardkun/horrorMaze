//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UnityStandardAssets.Characters.FirstPerson
{
	public class HDK_Flashlight : MonoBehaviour {

	[Header ("Booleans")]						
	public bool IsWalk;
	public bool IsRun;
	public bool IsIdle;
	public bool IsDraw;
	public bool IsPutDown;

	[Header ("Animations")]						
	public GameObject ArmsAnims;
	public string WalkName;
	public string RunName;
	public string IdleName;
	public string DrawName;
	public string PutDownName;

	[Header ("Flashlight Options")]				
	public float health;
	public float MaxHealth;
    public float DecreaseSpeed;
    public float battery_quantity;
	public Light lightSource;
	public AudioClip DrawSound;
	public AudioClip PutDownSound;
	public AudioClip ChangeBattery;
	public AudioClip NoBattery;
	public float volumeSound;
	AudioSource audio_source;
	float DrawLenght;
	float UnDrawLenght;
	public bool hasFlashlight;
    public bool usingFlashlight;
	float duration = 0.2f;
	float baseIntensity;
	float alpha;
	bool one_battery;
	bool two_battery;
	bool three_battery;
	bool four_battery;
	public static bool five_battery;
	GameObject Player;
	bool WasNotCharge;
    HDK_UITextManager TextManager;
    HDK_SimpleInventory inventoryScript;

    [Header("Flickering Mode")]
    public bool Enabled;

    [Header ("UI")]
	GameObject FlashlightUI;
	GameObject BatteryQuantity;
	GameObject InteractText;
	GameObject flashlightInfo;
	GameObject batteryInfo;
	GameObject BatteryIcon;

	[Header("Battery UI")]
	Image _20percent;
	Image _40percent;
	Image _60percent;
	Image _80percent;
	Image _100percent;

	void Start ()
	{
		audio_source = this.GetComponent<AudioSource> ();	
		DrawLenght = ArmsAnims.GetComponent<Animation> ().GetClip (DrawName).length;
		UnDrawLenght = ArmsAnims.GetComponent<Animation> ().GetClip (PutDownName).length;
		baseIntensity = lightSource.intensity;
		Player = GameObject.Find("Player");
		BatteryIcon = GameObject.Find ("BatteryIcon");
		flashlightInfo = GameObject.Find ("c_flashlight");
		batteryInfo = GameObject.Find ("c_battery");
		FlashlightUI = GameObject.Find ("BatteryCounter");
		BatteryQuantity = GameObject.Find ("BatteryQuantity");
		InteractText = GameObject.Find ("t_flashlight");
		_20percent = GameObject.Find ("20%").GetComponent<Image> ();
		_40percent = GameObject.Find ("40%").GetComponent<Image> ();
		_60percent = GameObject.Find ("60%").GetComponent<Image> ();
		_80percent = GameObject.Find ("80%").GetComponent<Image> ();
		_100percent = GameObject.Find ("100%").GetComponent<Image> ();
        TextManager = Player.GetComponent<HDK_UITextManager>();
        if (Player.gameObject.GetComponent<HDK_SimpleInventory>() != null)
        {
            inventoryScript = Player.gameObject.GetComponent<HDK_SimpleInventory>();
        }
    }

    public void HasFlashlight()
	{
        InteractText.GetComponent<HDK_UIFade>().FadeIn();
		InteractText.GetComponent<Text>().text = TextManager.f_Pickup;
		flashlightInfo.GetComponent<HDK_UIFade> ().FadeIn();
        StartCoroutine (FadeOutText ());
		StartCoroutine (FadeOutInfoFlashlight ());
		hasFlashlight = true;
		//StartCoroutine(Draw ());
	}

	IEnumerator Draw()
	{
		IsDraw = true;
		ShowArms ();
		audio_source.PlayOneShot (DrawSound, volumeSound);		
		yield return new WaitForSeconds (DrawLenght - 0.35f);
		IsDraw = false;
		lightSource.enabled = true;
		if (Enabled) {
			lightSource.gameObject.GetComponent<HDK_LightFlicker> ().enabled = true;
		} else 
		{
				lightSource.gameObject.GetComponent<HDK_LightFlicker> ().enabled = false;
		}
	}

	IEnumerator Putdown()
	{
		IsPutDown = true;
		StartCoroutine (CompletePutDown ());
		yield return new WaitForSeconds (1);
		lightSource.enabled = false;
		audio_source.PlayOneShot (PutDownSound, volumeSound);
		if (Enabled)
        {
		    lightSource.gameObject.GetComponent<HDK_LightFlicker> ().enabled = false;
		}
	}
	
	IEnumerator CompletePutDown()
	{
		yield return new WaitForSeconds (UnDrawLenght);
		IsPutDown = false;
		ShowOffArms ();
	}

	void ShowArms()
	{
		ArmsAnims.SetActive (true);
	}

	void ShowOffArms()
	{
		ArmsAnims.SetActive (false);
	}

	IEnumerator FadeOutText()
	{
		yield return new WaitForSeconds (2);
		InteractText.GetComponent<HDK_UIFade> ().FadeOut();
	}

	IEnumerator FadeOutInfoFlashlight()
	{
		yield return new WaitForSeconds (2);
		flashlightInfo.GetComponent<HDK_UIFade> ().FadeOut();
	}

	IEnumerator FadeOutInfoBattery()
	{
		yield return new WaitForSeconds (2);
		batteryInfo.GetComponent<HDK_UIFade> ().FadeOut();
    }

	public void AddBattery()
	{
		if (battery_quantity < 5) {
			battery_quantity += 1;
            InteractText.GetComponent<Text>().text = TextManager.b_Pickup;
			batteryInfo.GetComponent<HDK_UIFade> ().FadeIn();
			InteractText.GetComponent<HDK_UIFade> ().FadeIn();
            StartCoroutine (FadeOutText ());
			StartCoroutine (FadeOutInfoBattery ());
		} else 
		{
			InteractText.GetComponent<HDK_UIFade> ().FadeIn();
            InteractText.GetComponent<Text>().text = TextManager.b_NegativePickup;
            StartCoroutine (FadeOutText ());
		}
	}		
	
	void Update () {

			if (health == 0) 
			{
				_20percent.enabled = false;
				_40percent.enabled = false;
				_60percent.enabled = false;
				_80percent.enabled = false;
				_100percent.enabled = false;
			}
			else if (health <= 20) 		
			{
				_20percent.enabled = true;
				_40percent.enabled = false;
				_60percent.enabled = false;
				_80percent.enabled = false;
				_100percent.enabled = false;
			}
			else if (health <= 40 && health > 20) 		
			{
				_20percent.enabled = true;
				_40percent.enabled = true;
				_60percent.enabled = false;
				_80percent.enabled = false;
				_100percent.enabled = false;
			}
			else if (health <= 60 && health > 40) 		
			{
				_20percent.enabled = true;
				_40percent.enabled = true;
				_60percent.enabled = true;
				_80percent.enabled = false;
				_100percent.enabled = false;
			}
			else if (health <= 80 && health > 60) 		
			{
				_20percent.enabled = true;
				_40percent.enabled = true;
				_60percent.enabled = true;
				_80percent.enabled = true;
				_100percent.enabled = false;
			}
			else if (health <= 100 && health > 80) 		
			{
				_20percent.enabled = true;
				_40percent.enabled = true;
				_60percent.enabled = true;
				_80percent.enabled = true;
				_100percent.enabled = true;
			}

			if (battery_quantity == 1) 
			{
				one_battery = true;
				two_battery = false;
				three_battery = false;
				four_battery = false;
				five_battery = false;
			}
			if (battery_quantity == 2) 
			{
				one_battery = false;
				two_battery = true;
				three_battery = false;
				four_battery = false;
				five_battery = false;
			}
			if (battery_quantity == 3) 
			{
				one_battery = false;
				two_battery = false;
				three_battery = true;
				four_battery = false;
				five_battery = false;
			}
			if (battery_quantity == 4) 
			{
				one_battery = false;
				two_battery = false;
				three_battery = false;
				four_battery = true;
				five_battery = false;
			}
			if (battery_quantity == 5) 
			{
				one_battery = false;
				two_battery = false;
				three_battery = false;
				four_battery = false;
				five_battery = true;
			}

			if (!hasFlashlight) 
			{
				if(Input.GetKeyDown(KeyCode.F))
				{
					InteractText.GetComponent<HDK_UIFade> ().FadeIn();
                    InteractText.GetComponent<Text> ().text = TextManager.f_NegativeUse;
                    StartCoroutine (FadeOutText ());	
				}
			}

			if (usingFlashlight) 
			{
				if (Enabled) 
				{
					if (health <= 25f) {
						lightSource.gameObject.GetComponent<HDK_LightFlicker> ().enabled = false;
					} else 
					{
						lightSource.gameObject.GetComponent<HDK_LightFlicker> ().enabled = true;
					}
				}
			}

			if (hasFlashlight)
			{
				FlashlightUI.GetComponent<HDK_UIFade> ().FadeIn();
            }
			else 
			{
                FlashlightUI.GetComponent<HDK_UIFade>().FadeOut();
			}

			if (hasFlashlight) 
			{
				if (Input.GetKeyDown (KeyCode.Q)) 
				{
					if (battery_quantity > 0) {
						WasNotCharge = true;
					} else 
					{
						WasNotCharge = false;
					}
				}
			}

			if (hasFlashlight) {
				if (battery_quantity >= 1 && health <= 80) {
					if (Input.GetKeyDown (KeyCode.Q)) {
						battery_quantity -= 1;
						audio_source.PlayOneShot (ChangeBattery, volumeSound);
						health += 20f;
					}
				}
				if (battery_quantity == 0  && !WasNotCharge) {
					if (Input.GetKeyDown (KeyCode.Q)) {
						InteractText.GetComponent<HDK_UIFade> ().FadeIn();
                        InteractText.GetComponent<Text> ().text = TextManager.b_NegativeUse;
                        StartCoroutine (FadeOutText ());
						audio_source.PlayOneShot (NoBattery, volumeSound);
					}
				}
			}

			BatteryQuantity.GetComponent<Text>().text = battery_quantity + " / 5";

			if (lightSource.isActiveAndEnabled) {
				if (health > 0.0f) {
					health -= Time.deltaTime * DecreaseSpeed;
				}
			}

			if(health < MaxHealth/4 && lightSource.enabled){ 
				float phi = Time.time / duration * 2 * Mathf.PI;
				float amplitude = Mathf.Cos( phi ) * (float)0.5 + baseIntensity;
				lightSource.GetComponent<Light>().intensity = amplitude + Random.Range(0.1f, 1.0f) ;
			}
			lightSource.GetComponent<Light>().color = new Color(alpha/MaxHealth, alpha/MaxHealth, alpha/MaxHealth, alpha/MaxHealth);
			alpha = health;  


			if (ArmsAnims.activeSelf)
			{
				usingFlashlight = true;
			}
			else
			{
				usingFlashlight = false;	
			}

			if (health < 20) {
				BatteryIcon.GetComponent<Image> ().color = Color.red;
			} else if (health >= 20) 
			{
				BatteryIcon.GetComponent<Image> ().color = Color.white;
			}


			if (health <= 0) 
			{
			health = 0;			
			}else if(health >= MaxHealth)
			{
				health  = MaxHealth;
			}

			if (battery_quantity >= 5) 
			{
				battery_quantity = 5;
			}
			if (battery_quantity <= 0) 
			{
				battery_quantity = 0;
			}

            bool examining = HDK_RaycastManager.ExaminingObject;
            bool security = HDK_RaycastManager.UsingSecurityCam;
            bool reading = HDK_RaycastManager.ReadingPaper;

            if (hasFlashlight)
			{
				if (!IsDraw && !IsPutDown && !examining && !security && !reading && !inventoryScript.open)
				{
					if (Input.GetKeyDown (KeyCode.F))
					{
						if (usingFlashlight) {
							StartCoroutine(Putdown());
						} else
						{
							StartCoroutine(Draw ());
						}
					}
				}
			}

		if(Player.GetComponent<FirstPersonController>().m_CharacterController.velocity.sqrMagnitude > 0)
		{
			IsWalk = true;
			IsIdle = false;
			IsRun = false;
		}
        else if (Player.GetComponent<FirstPersonController>().m_CharacterController.velocity.sqrMagnitude == 0)
		{
			IsWalk = false;
			IsRun = false;
			IsIdle = true;
		}
		if (Player.GetComponent<FirstPersonController> ().isRunning) {
			IsWalk = false;
			IsRun = true;
		}
        else 
		{
			IsRun = false;
		}
		if (Player.GetComponent<FirstPersonController> ().isRunning && !Player.GetComponent<FirstPersonController> ().CanRun)
		{
			IsWalk = true;
			IsRun = false;
			IsIdle = false;
		}
        if (inventoryScript.open || reading || examining || security)
        {
            IsWalk = false;
            IsRun = false;
            IsIdle = true;
        }

		if (IsWalk)
		{
			ArmsAnims.GetComponent<Animation> ().CrossFade (WalkName, 0.3f, PlayMode.StopAll);
			ArmsAnims.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
		}
		if (IsRun) {
			ArmsAnims.GetComponent<Animation> ().CrossFade (RunName, 0.3f, PlayMode.StopAll);
			ArmsAnims.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
		}
		if (IsIdle)
		{
			ArmsAnims.GetComponent<Animation> ().CrossFade (IdleName, 0.3f, PlayMode.StopAll);
			ArmsAnims.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
		}
		if (IsDraw)
		{
			ArmsAnims.GetComponent<Animation> ().Play (DrawName, PlayMode.StopAll);
		}
		if (IsPutDown)
		{
			ArmsAnims.GetComponent<Animation> ().Play (PutDownName, PlayMode.StopAll);
		}
		}
	}
}