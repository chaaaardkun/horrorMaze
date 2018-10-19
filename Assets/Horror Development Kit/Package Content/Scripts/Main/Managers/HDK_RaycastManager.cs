//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace UnityStandardAssets.Characters.FirstPerson
{
public class HDK_RaycastManager : MonoBehaviour {

	[Header ("Crosshair")]
	public GameObject normal_Crosshair;
	public GameObject interact_Crosshair;
	
	[Header ("Raycast")]
	public float distance = 2.0f;
	public LayerMask layerMaskInteract;
    public Color RayGizmoColor;
    RaycastHit hit;

    [Header ("Tags")]
	string KeyTag = "Key";
	string FlashlightTag = "Flashlight";
	string FlashlightBatteryTag = "FlashlightBattery";
	string DoorTag = "Door";
	string PaperTag = "Paper";
	string TelecameraTag = "Telecamera";
	string LampTag = "Lamp";
	string ExamineTag = "Examine";
    string PlayAudioTag = "PlayAudio";
    string SecurityCamTag = "SecurityCamera";
    string FoodTag = "Food";

    [Header ("SFX")]
	public AudioClip[] KeyPickup;
	public AudioClip[] GeneralPickup;
	public AudioClip[] PaperPickup;
	public AudioClip[] ExaminedReveal;
	public AudioClip CantPickup;
	public float pickupVolume;
	public float revealVolume;
	
	[Header ("Tags Bools")]
	bool OnTagKey;
	bool OnTagFlashlight;
	bool OnTagFlashlightBattery;
	bool OnTagDoor;
	bool OnTagPaper;
	bool OnTagTelecamera;
	bool OnTagLamp;
	bool OnTagExamine;
    bool OnTagPlayAudio;
    bool OnTagSecurityCam;
    bool OnTagFood;

    [Header ("Other")]
	GameObject Player;					        //Player GameObject
	GameObject targetDoor;				        //The target door/drawer...
	bool hasFlashlight;					        //Do we have the flashlight?
	GameObject doorRaycasted;			        //The Door/Drawer... raycasted
	GameObject targetPaperNote;			        //The target paper note
	public GameObject raycasted_obj;            //The raycasted item
    GameObject RaycastedLamp;                   //The raycasted functional lamp
	GameObject RaycastedExamineObj;		        //The raycasted examinable item
	public static bool ExaminingObject;	        //Are we examining an item?
	public static bool ReadingPaper;            //Are we reading a paper?
    public static bool UsingSecurityCam;        //Are we using a security camera?
    GameObject ExamineObjectInfoGUI;	        //The Examine Object Info GUI
	bool ShowExaminingInfoGui;			        //Do we need to show the Examine Object Info GUI?
	public float RevealWait;			        //The time we need to wait for the item to be revealed after the examination
	GameObject ItemNameText;			        //The text that shows us the name of the item
	bool FadeInteractInfoGUI;			        //Do we need to fade the Interact Info GUI?
	GameObject examineEyeIcon;
    bool hasPeak;
    bool hasHBob;

    void Start()
	{
		Player = GameObject.Find("Player");
		ExaminingObject = false;
		ExamineObjectInfoGUI = GameObject.Find ("examineControls");
		ItemNameText = GameObject.Find ("itemName");
		examineEyeIcon = GameObject.Find ("icon_examining");
	}	

	IEnumerator RevealExamined()
	{
		yield return new WaitForSeconds (RevealWait);
		if (ExaminingObject)
        {
			ItemNameText.GetComponent<Text> ().text = RaycastedExamineObj.GetComponent<HDK_InteractObject> ().ItemName;
			RaycastedExamineObj.GetComponent<HDK_InteractObject> ().Examined = true;
			this.GetComponent<AudioSource> ().clip = ExaminedReveal [Random.Range (0, ExaminedReveal.Length)];
			this.GetComponent<AudioSource> ().volume = revealVolume;
			this.GetComponent<AudioSource> ().Play ();
			FadeInteractInfoGUI = true;
		}
	}

	void Update() {

		if (FadeInteractInfoGUI) {
			ItemNameText.GetComponent<HDK_UIFade> ().TextOut = false;
			ItemNameText.GetComponent<HDK_UIFade> ().TextIn = true;
		} else {
			if (!ExaminingObject) {
				ItemNameText.GetComponent<HDK_UIFade> ().TextOut = true;
				ItemNameText.GetComponent<HDK_UIFade> ().TextIn = false;
			}
		}

		if (ShowExaminingInfoGui) {
				ExamineObjectInfoGUI.GetComponent<HDK_UIFade> ().TextIn = true;
				ExamineObjectInfoGUI.GetComponent<HDK_UIFade> ().TextOut = false;
		} else 
		{
				ExamineObjectInfoGUI.GetComponent<HDK_UIFade> ().TextIn = false;
				ExamineObjectInfoGUI.GetComponent<HDK_UIFade> ().TextOut = true;
		}

		bool canusecamera = HDK_DigitalCamera.canUse;

        Vector3 position = transform.parent.position;
		Vector3 direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, direction * distance, RayGizmoColor);

        if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
				if (hit.transform.gameObject.GetComponent<HDK_InteractObject> ()) {
					raycasted_obj = hit.transform.gameObject;
					if (raycasted_obj.GetComponent<HDK_WithEmission> ()) 
					{
						raycasted_obj.GetComponent<HDK_WithEmission> ().rayed = true;
					}else if(!raycasted_obj.GetComponent<HDK_WithEmission>() && raycasted_obj.GetComponentInChildren<Light>() && !raycasted_obj.GetComponent<HDK_WithNoEmission>())
					{
						raycasted_obj.GetComponentInChildren<Light> ().enabled = true;
					}
					normal_Crosshair.SetActive (false);
					interact_Crosshair.SetActive (true);
					FadeInteractInfoGUI = true;
					if (raycasted_obj.GetComponent<HDK_InteractObject> ().Examined) {
						ItemNameText.GetComponent<Text> ().text = raycasted_obj.GetComponent<HDK_InteractObject> ().ItemName;
					} else 
					{
						ItemNameText.GetComponent<Text> ().text = null;
					}
				}
			}
			else
			{
				if (raycasted_obj != null) {
					if (raycasted_obj.GetComponent<HDK_WithEmission> ()) {
						raycasted_obj.GetComponent<HDK_WithEmission> ().rayed = false;
					} else if (!raycasted_obj.GetComponent<HDK_WithEmission> () && raycasted_obj.GetComponentInChildren<Light> () && !raycasted_obj.GetComponent<HDK_WithNoEmission>()) {
						raycasted_obj.GetComponentInChildren<Light> ().enabled = false;
					}
				}
				normal_Crosshair.SetActive (true);
				interact_Crosshair.SetActive (false);
				FadeInteractInfoGUI = false;
		}


		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag (KeyTag) && hit.transform.GetComponent<HDK_Key> ()) {
				targetDoor = hit.transform.GetComponent<HDK_Key> ().targetDoor;
				OnTagKey = true;

			}
			else
			{
				OnTagKey = false;
			}
		} 
		else
		{
			OnTagKey = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(FlashlightTag)){
				OnTagFlashlight = true;
			}
			else
			{
				OnTagFlashlight = false;
			}
		} 
		else
		{
			OnTagFlashlight = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(FlashlightBatteryTag)){
				OnTagFlashlightBattery = true;
			}
			else
			{
				OnTagFlashlightBattery = false;
			}
		} 
		else
		{
			OnTagFlashlightBattery = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(DoorTag)){
				OnTagDoor = true;
				doorRaycasted = hit.transform.gameObject;
			}
			else
			{
				OnTagDoor = false;
			}
		} 
		else
		{
			OnTagDoor = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
				if (hit.transform.CompareTag(PaperTag) && hit.transform.GetComponent<HDK_Note>()){
					targetPaperNote = hit.transform.GetComponent<HDK_Note> ().UI_Note;
				OnTagPaper = true;
			}
			else
			{
					OnTagPaper = false;
			}
		} 
		else
		{
				OnTagPaper = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(TelecameraTag)){
				OnTagTelecamera = true;
			}
			else
			{
				OnTagTelecamera = false;
			}
		} 
		else
		{
			OnTagTelecamera = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(LampTag) && hit.transform.GetComponent<HDK_SwitchableLamp>()){
				OnTagLamp = true;
				RaycastedLamp = hit.transform.gameObject;
			}
			else
			{
					OnTagLamp = false;
			}
		} 
		else
		{
				OnTagLamp = false;
		}

		if (Physics.Raycast (position, direction, out hit, distance, layerMaskInteract.value)) {
			if (hit.transform.CompareTag(ExamineTag) || hit.transform.GetComponent<HDK_InteractObject>()){
				OnTagExamine = true;
				RaycastedExamineObj = hit.transform.gameObject;
			}
			else
			{
				OnTagExamine = false;
			}
		} 
		else
		{
				OnTagExamine = false;
		}

        if (Physics.Raycast(position, direction, out hit, distance, layerMaskInteract.value))
        {
            if (hit.transform.CompareTag(PlayAudioTag))
            {
                OnTagPlayAudio = true;
            }
            else
            {
                OnTagPlayAudio = false;
            }
        }
        else
        {
            OnTagPlayAudio = false;
        }

        if (Physics.Raycast(position, direction, out hit, distance, layerMaskInteract.value))
        {
            if (hit.transform.CompareTag(SecurityCamTag))
            {
                OnTagSecurityCam = true;
            }
            else
            {
                OnTagSecurityCam = false;
            }
        }
        else
        {
                OnTagSecurityCam = false;
        }

        if (Physics.Raycast(position, direction, out hit, distance, layerMaskInteract.value))
        {
            if (hit.transform.CompareTag(FoodTag))
            {
                OnTagFood = true;
            }
            else
            {
                OnTagFood = false;
            }
        }
        else
        {
            OnTagFood = false;
        }

        if (ExaminingObject) 
		{
				if (Input.GetKeyDown (KeyCode.Mouse1)) 
				{
					if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera) 
					{
						Player.GetComponent<HDK_DigitalCamera> ().CameraUI.GetComponent<CanvasGroup> ().alpha = 1;
						if (Player.GetComponent<HDK_DigitalCamera> ().broken) 
						{
							Player.GetComponent<HDK_DigitalCamera> ().camera_effect.enabled = true;
							Player.GetComponent<HDK_DigitalCamera> ().brokenGUI.GetComponent<CanvasGroup> ().alpha = 1;
						}
					}
                    if (hasHBob)
                    {
                        GetComponentInParent<HeadBobController>().enabled = true;
                    }
                    if (hasPeak)
                    {
                        GetComponentInParent<HDK_Peak>().enabled = true;
                    }
                    if (Player.GetComponent<HDK_Stamina>() != null)
                    {
                        Player.GetComponent<HDK_Stamina>().Busy(false);
                    }
                    examineEyeIcon.GetComponent<HDK_UIFade> ().TextIn = false;
					examineEyeIcon.GetComponent<HDK_UIFade> ().TextOut = true;
					ExaminingObject = false;
					ShowExaminingInfoGui = false;
					Player.GetComponent<FirstPersonController> ().enabled = true;
					Player.GetComponentInChildren<HDK_ExamineRotation> ().enabled = false;
					Player.GetComponentInChildren<HDK_ExamineRotation> ().target = null;
					RaycastedExamineObj.GetComponent<HDK_InteractObject> ().ExaminableObject.SetActive (false);
					RaycastedExamineObj.SetActive (true);
					if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera && !Player.GetComponent<HDK_DigitalCamera>().broken) 
					{
						Player.GetComponentInChildren<HDK_DigitalCameraZoom> ().canZoom = true;
					}
					canusecamera = true;
					Player.GetComponentInChildren<BlurOptimized> ().enabled = false;
					if (!Player.GetComponent<HDK_DigitalCamera>().UsingCamera) 
					{
						Player.GetComponent<HDK_MouseZoom> ().canZoom = true;
					}
				}
		}

		if (OnTagExamine) {
				if (Input.GetKeyDown (KeyCode.Mouse1)) {
					if (!ExaminingObject) {
						if (RaycastedExamineObj.GetComponent<HDK_InteractObject> ().ExaminableObject == null) {
							ExaminingObject = false;
							this.GetComponent<AudioSource> ().clip = CantPickup;
							this.GetComponent<AudioSource> ().volume = 1f;
							this.GetComponent<AudioSource> ().Play ();
						} else {
							if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera) {
								Player.GetComponent<HDK_DigitalCamera> ().CameraUI.GetComponent<CanvasGroup> ().alpha = 0;
								if (Player.GetComponent<HDK_DigitalCamera> ().broken) {
									Player.GetComponent<HDK_DigitalCamera> ().brokenGUI.GetComponent<CanvasGroup> ().alpha = 0;
									Player.GetComponent<HDK_DigitalCamera> ().camera_effect.enabled = false;
								}
							}
                            if (!RaycastedExamineObj.GetComponent<HDK_InteractObject> ().Examined) {
								StartCoroutine (RevealExamined ());
							}
							if (GetComponentInParent<HDK_SimpleInventory> () != null) {
								GetComponentInParent<HDK_SimpleInventory> ().close ();
							}
                            if (GetComponentInParent<HeadBobController>() != null)
                            {
                                hasHBob = true;
                                GetComponentInParent<HeadBobController>().enabled = false;
                            }else
                            {
                                hasHBob = false;
                            }
                            if (GetComponentInParent<HDK_Peak>() != null)
                            {
                                hasPeak = true;
                                GetComponentInParent<HDK_Peak>().enabled = false;
                            }else
                            {
                                hasPeak = false;
                            }
                            if (Player.GetComponent<HDK_Stamina>() != null)
                            {
                                Player.GetComponent<HDK_Stamina>().Busy(true);
                            }
                            examineEyeIcon.GetComponent<HDK_UIFade> ().TextIn = true;
							examineEyeIcon.GetComponent<HDK_UIFade> ().TextOut = false;
							ShowExaminingInfoGui = true;
							ExaminingObject = true;
							Cursor.visible = true;
							Cursor.lockState = CursorLockMode.None;
							Player.GetComponentInChildren<BlurOptimized> ().enabled = true;
							Player.GetComponent<FirstPersonController> ().enabled = false;
							Player.GetComponentInChildren<HDK_ExamineRotation> ().enabled = true;
							Player.GetComponentInChildren<HDK_ExamineRotation> ().target = RaycastedExamineObj.GetComponent<HDK_InteractObject> ().ExaminableObject.transform;
							RaycastedExamineObj.GetComponent<HDK_InteractObject> ().ExaminableObject.SetActive (true);
							RaycastedExamineObj.SetActive (false);
							Player.GetComponentInChildren<HDK_DigitalCameraZoom> ().canZoom = false;
							canusecamera = false;
							Player.GetComponent<HDK_MouseZoom> ().SendMessage ("ZoomOut");
						}
					}
				}

				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					if (!RaycastedExamineObj.GetComponent<HDK_InteractObject> ().Interactable) {
						this.GetComponent<AudioSource> ().clip = CantPickup;
						this.GetComponent<AudioSource> ().volume = 1f;
						this.GetComponent<AudioSource> ().Play ();
					}
				}
			} 
		

		if (OnTagPaper) {
				if (!ExaminingObject) {
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						if (targetPaperNote.GetComponent<CanvasGroup> ().alpha == 0) {
							if (GetComponentInParent<HDK_SimpleInventory> () != null) {
								GetComponentInParent<HDK_SimpleInventory> ().close ();
							}
                            if (GetComponentInParent<HeadBobController>() != null)
                            {
                                hasHBob = true;
                                GetComponentInParent<HeadBobController>().enabled = false;
                            }
                            else
                            {
                                hasHBob = false;
                            }
                            if (GetComponentInParent<HDK_Peak>() != null)
                            {
                                hasPeak = true;
                                GetComponentInParent<HDK_Peak>().enabled = false;
                            }
                            else
                            {
                                hasPeak = false;
                            }
                            if (Player.GetComponent<HDK_Stamina>() != null)
                            {
                                Player.GetComponent<HDK_Stamina>().Busy(true);
                            }
                            if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
                            {
                                Player.GetComponent<HDK_DigitalCamera>().CameraUI.GetComponent<CanvasGroup>().alpha = 0;
                                if (Player.GetComponent<HDK_DigitalCamera>().broken)
                                {
                                    Player.GetComponent<HDK_DigitalCamera>().brokenGUI.GetComponent<CanvasGroup>().alpha = 0;
                                    Player.GetComponent<HDK_DigitalCamera>().camera_effect.enabled = false;
                                }
                            }
                            ReadingPaper = true;
							Player.GetComponent<HDK_MouseZoom> ().SendMessage ("ZoomOut");
							Player.GetComponentInChildren<HDK_DigitalCameraZoom> ().canZoom = false;
							canusecamera = false;
							Cursor.visible = true;
							Cursor.lockState = CursorLockMode.None;
							targetPaperNote.GetComponent<HDK_UIFade> ().TextIn = true;
							targetPaperNote.GetComponent<HDK_UIFade> ().TextOut = false;
							Player.GetComponent<FirstPersonController> ().enabled = false;
							this.GetComponent<AudioSource> ().clip = PaperPickup [Random.Range (0, PaperPickup.Length)];
							this.GetComponent<AudioSource> ().volume = pickupVolume;
							this.GetComponent<AudioSource> ().Play ();
						} else {
							if (!Player.GetComponent<HDK_DigitalCamera>().UsingCamera) 
							{
								Player.GetComponent<HDK_MouseZoom> ().enabled = true;
								Player.GetComponent<HDK_MouseZoom> ().canZoom = true;
							}
							if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera && !Player.GetComponent<HDK_DigitalCamera>().broken) 
							{
								Player.GetComponentInChildren<HDK_DigitalCameraZoom> ().canZoom = true;
							}
                            if (hasHBob)
                            {
                                GetComponentInParent<HeadBobController>().enabled = true;
                            }
                            if (hasPeak)
                            {
                                GetComponentInParent<HDK_Peak>().enabled = true;
                            }
                            if (Player.GetComponent<HDK_Stamina>() != null)
                            {
                                Player.GetComponent<HDK_Stamina>().Busy(false);
                            }
                            ReadingPaper = false;
                            Player.GetComponent<HDK_DigitalCamera>().CameraUI.GetComponent<CanvasGroup>().alpha = 1;
                            if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
                            {

                                if (Player.GetComponent<HDK_DigitalCamera>().broken)
                                {
                                    Player.GetComponent<HDK_DigitalCamera>().camera_effect.enabled = true;
                                    Player.GetComponent<HDK_DigitalCamera>().brokenGUI.GetComponent<CanvasGroup>().alpha = 1;
                                }
                            }
							canusecamera = true;
							targetPaperNote.GetComponent<HDK_UIFade> ().TextIn = false;
							targetPaperNote.GetComponent<HDK_UIFade> ().TextOut = true;
							Player.GetComponent<FirstPersonController> ().enabled = true;
							this.GetComponent<AudioSource> ().clip = PaperPickup [Random.Range (0, PaperPickup.Length)];
							this.GetComponent<AudioSource> ().volume = pickupVolume;
							this.GetComponent<AudioSource> ().Play ();
						}
					}
				}
		}

		if (OnTagDoor) {

				if (!ExaminingObject) {
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						if (doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().Jammed) {
							doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().SendMessageUpwards ("doorJammed");
						}
						if (doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().Free) {
							doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().SendMessageUpwards ("doorOpenClose");
						}
						if (doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().Locked) {
							doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().SendMessageUpwards ("doorLocked");
						}
						if (doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().wasLocked) {
							doorRaycasted.GetComponentInParent<HDK_DynamicObject> ().SendMessageUpwards ("removeWasLocked");
						}
					}
				}
		}

		if (OnTagFlashlight) {

				if (!ExaminingObject) {

					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						Player.GetComponent<HDK_Flashlight> ().SendMessage ("HasFlashlight");
						Destroy (hit.transform.gameObject);
						this.GetComponent<AudioSource> ().clip = GeneralPickup [Random.Range (0, GeneralPickup.Length)];
						this.GetComponent<AudioSource> ().volume = pickupVolume;
						this.GetComponent<AudioSource> ().Play ();
					}
				}
		}

		if (OnTagLamp)
        {				
			if (!ExaminingObject)
            {
				if (Input.GetKeyDown (KeyCode.Mouse0))
                {
					if (RaycastedLamp.GetComponent<HDK_SwitchableLamp> ().isOn)
                    {
						RaycastedLamp.SendMessage ("SwitchOff");
					}
                    else
                    {
						RaycastedLamp.SendMessage ("SwitchOn");
					}
				}
			}
		}

        if (OnTagPlayAudio)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!raycasted_obj.GetComponent<HDK_PlayableAudio>().isPlaying)
                {
                    raycasted_obj.SendMessage("PlaySound");
                }
            }            
        }

        if (OnTagSecurityCam)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                    if (!UsingSecurityCam)
                    {
                        if (GetComponentInParent<HDK_SimpleInventory>() != null)
                        {
                            GetComponentInParent<HDK_SimpleInventory>().close();
                        }
                        if (Player.GetComponent<HDK_DigitalCamera>().nv_Enabled)
                        {
                            Player.GetComponent<HDK_DigitalCamera>().nv_light.enabled = false;                            
                        }
                        if (raycasted_obj.GetComponent<HDK_HidingZone>())
                        {
                            if (!raycasted_obj.GetComponent<HDK_HidingZone>().HaveTrigger)
                            {
                                Player.GetComponent<HDK_Hiding>().OnHide(raycasted_obj.GetComponent<HDK_HidingZone>().ShowGUI);
                            }
                        }
                        UsingSecurityCam = true;
                        Player.GetComponent<FirstPersonController>().enabled = false;
                        raycasted_obj.GetComponent<HDK_SecurityMonitor>().StartCam();
                        if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
                        {
                            Player.GetComponent<HDK_DigitalCamera>().CameraUI.GetComponent<CanvasGroup>().alpha = 0;
                            if (Player.GetComponent<HDK_DigitalCamera>().broken)
                            {
                                Player.GetComponent<HDK_DigitalCamera>().brokenGUI.GetComponent<CanvasGroup>().alpha = 0;
                                Player.GetComponent<HDK_DigitalCamera>().camera_effect.enabled = false;
                            }
                        }
                        Player.GetComponent<HDK_MouseZoom>().SendMessage("ZoomOut");
                        Player.GetComponentInChildren<HDK_DigitalCameraZoom>().canZoom = false;

                    }
                    else if (UsingSecurityCam)
                    {
                        UsingSecurityCam = false;
                        Player.GetComponent<FirstPersonController>().enabled = true;
                        raycasted_obj.GetComponent<HDK_SecurityMonitor>().EndCam();
                        if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
                        {
                            Player.GetComponent<HDK_DigitalCamera>().CameraUI.GetComponent<CanvasGroup>().alpha = 1;
                            if (Player.GetComponent<HDK_DigitalCamera>().broken)
                            {
                                Player.GetComponent<HDK_DigitalCamera>().camera_effect.enabled = true;
                                Player.GetComponent<HDK_DigitalCamera>().brokenGUI.GetComponent<CanvasGroup>().alpha = 1;
                            }
                        }
                        if (Player.GetComponent<HDK_DigitalCamera>().nv_Enabled)
                        {
                            Player.GetComponent<HDK_DigitalCamera>().nv_light.enabled = true;
                        }
                        if (!Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
                        {
                            Player.GetComponent<HDK_MouseZoom>().enabled = true;
                            Player.GetComponent<HDK_MouseZoom>().canZoom = true;
                        }
                        if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera && !Player.GetComponent<HDK_DigitalCamera>().broken)
                        {
                            Player.GetComponentInChildren<HDK_DigitalCameraZoom>().canZoom = true;
                        }
                        if (raycasted_obj.GetComponent<HDK_HidingZone>())
                        {
                            if (!raycasted_obj.GetComponent<HDK_HidingZone>().HaveTrigger)
                            {
                                Player.GetComponent<HDK_Hiding>().OffHide(raycasted_obj.GetComponent<HDK_HidingZone>().ShowGUI);
                            }
                        }
                    }
             }
        }

        if (OnTagFood)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(!raycasted_obj.GetComponent<HDK_Food>().Harmful)
                {
                    if (Player.GetComponent<HDK_PlayerHealth>().Health < 100f)
                    {
                        Player.GetComponent<HDK_PlayerHealth>().Health += raycasted_obj.GetComponent<HDK_Food>().value;
                        Destroy(hit.transform.gameObject);
                        this.GetComponent<AudioSource>().clip = raycasted_obj.GetComponent<HDK_Food>().Sounds[Random.Range(0, raycasted_obj.GetComponent<HDK_Food>().Sounds.Length)];
                        this.GetComponent<AudioSource>().volume = raycasted_obj.GetComponent<HDK_Food>().SoundVolume;
                        this.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    Player.GetComponent<HDK_PlayerHealth>().Damage(raycasted_obj.GetComponent<HDK_Food>().value);
                    Destroy(hit.transform.gameObject);
                }
            }            
        }

        if (OnTagFlashlightBattery) {

				if (!ExaminingObject) {
				
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						Player.GetComponent<HDK_Flashlight> ().SendMessage ("AddBattery");

						bool has_five_batteries = HDK_Flashlight.five_battery;

						if (!has_five_batteries) {						
							Destroy (hit.transform.gameObject);
							this.GetComponent<AudioSource> ().clip = GeneralPickup [Random.Range (0, GeneralPickup.Length)];
							this.GetComponent<AudioSource> ().volume = pickupVolume;
							this.GetComponent<AudioSource> ().Play ();
						}
					}
				}
		}			
		
		if (OnTagTelecamera) {

				if (!ExaminingObject) {

					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						Player.GetComponent<HDK_DigitalCamera>().HasCamera = true;
						Player.GetComponent<HDK_DigitalCamera> ().SendMessage ("ShowPickedText");
						Destroy (hit.transform.gameObject);
						this.GetComponent<AudioSource> ().clip = GeneralPickup [Random.Range (0, GeneralPickup.Length)];
						this.GetComponent<AudioSource> ().volume = pickupVolume;
						this.GetComponent<AudioSource> ().Play ();
					}
				}
		}				

		if (OnTagKey) {

				if (!ExaminingObject) {

					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						Player.GetComponent<HDK_KeyManager> ().SendMessage ("AddKey");
						Destroy (hit.transform.gameObject);
						this.GetComponent<AudioSource> ().clip = KeyPickup [Random.Range (0, KeyPickup.Length)];
						this.GetComponent<AudioSource> ().volume = pickupVolume;
						this.GetComponent<AudioSource> ().Play ();
						if (!targetDoor.GetComponentInParent<HDK_DynamicObject> ().Jammed) {
							targetDoor.GetComponentInParent<HDK_DynamicObject> ().SendMessageUpwards ("OpenDoor");
						}
					}
				}
			}
	 }
        public void CloseCam()
        {
            UsingSecurityCam = false;
            Player.GetComponent<FirstPersonController>().enabled = true;
            raycasted_obj.GetComponent<HDK_SecurityMonitor>().EndCam();
            if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
            {
                Player.GetComponent<HDK_DigitalCamera>().CameraUI.GetComponent<CanvasGroup>().alpha = 1;
                if (Player.GetComponent<HDK_DigitalCamera>().broken)
                {
                    Player.GetComponent<HDK_DigitalCamera>().camera_effect.enabled = true;
                    Player.GetComponent<HDK_DigitalCamera>().brokenGUI.GetComponent<CanvasGroup>().alpha = 1;
                }
            }
            if (!Player.GetComponent<HDK_DigitalCamera>().UsingCamera)
            {
                Player.GetComponent<HDK_MouseZoom>().enabled = true;
                Player.GetComponent<HDK_MouseZoom>().canZoom = true;
            }
            if (Player.GetComponent<HDK_DigitalCamera>().UsingCamera && !Player.GetComponent<HDK_DigitalCamera>().broken)
            {
                Player.GetComponentInChildren<HDK_DigitalCameraZoom>().canZoom = true;
            }
        }
    }
 }