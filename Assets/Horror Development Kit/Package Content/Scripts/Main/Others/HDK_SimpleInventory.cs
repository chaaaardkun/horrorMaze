//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Characters.FirstPerson;

public class HDK_SimpleInventory : MonoBehaviour {

	[Header ("Inventory")]
	GameObject InventoryObject;
	public bool open;
	bool fadeIn;
	bool fadeOut;
    public bool AutoClose;
    public float CloseWait;

	[Header ("Items")]
	bool hasFlashlight;
	bool hasBattery;
	bool hasDigitalCamera;
	bool hasKey;
	float n_Batteries;
	int n_Keys;

	[Header ("UI")]
	Text q_keys;
	Text q_batteries;
    GameObject icon_flashlight;
    GameObject icon_battery;
    GameObject icon_digitalcamera;
    GameObject icon_key;

    GameObject Player;
    HDK_PauseManager PauseScript;

	void Start()
	{
		open = false;
		InventoryObject = GameObject.Find ("Inventory");
		q_batteries = GameObject.Find ("text_battery").GetComponent<Text>();
		q_keys = GameObject.Find ("text_key").GetComponent<Text>();
		Player = GameObject.Find("Player");
        PauseScript = GameObject.Find("Canvas").GetComponent<HDK_PauseManager>();
        icon_flashlight = GameObject.Find("icon_flashlight");
        icon_battery = GameObject.Find("icon_battery");
        icon_digitalcamera = GameObject.Find("icon_digitalcamera");
        icon_key = GameObject.Find("icon_key");
    }

	public void close()
	{
		fadeOut = true;
		fadeIn = false;
		open = false;
        StopAllCoroutines();
	}

	void Update()
	{
		if (fadeIn) 
		{
			InventoryObject.GetComponent<CanvasGroup> ().alpha += Time.deltaTime*2;
		}

		if (fadeOut) 
		{
			InventoryObject.GetComponent<CanvasGroup> ().alpha -= Time.deltaTime*2;
		}

		bool examining = HDK_RaycastManager.ExaminingObject;
		bool reading = HDK_RaycastManager.ReadingPaper;
        hasDigitalCamera = GetComponent<HDK_DigitalCamera>().HasCamera;

        if (!examining && !reading && !PauseScript.GamePaused) {
			if (Input.GetKeyUp (KeyCode.I))
            {
                if (AutoClose)
                {
                    if (!open)
                    {
                        StartCoroutine(CloseInventory());
                    }                
                }
                else
                {
                    if (open)
                    {
                        fadeOut = true;
                        fadeIn = false;
                        open = false;
                    }
                    else if (!open)
                    {
                        fadeOut = false;
                        fadeIn = true;
                        open = true;
                    }
                }
			}
		}

		n_Batteries = Player.GetComponent<HDK_Flashlight> ().battery_quantity;
		n_Keys = Player.GetComponent<HDK_KeyManager> ().Keys;

		if (n_Batteries > 0) {
			hasBattery = true;
			icon_battery.GetComponent<HDK_UIFade> ().TextIn = true;
            icon_battery.GetComponent<HDK_UIFade> ().TextOut = false;
			q_batteries.text = n_Batteries + " / 5";
		} else 
		{
            hasBattery = false;
			q_batteries.text = "N / A";
            icon_battery.GetComponent<HDK_UIFade> ().TextIn = false;
            icon_battery.GetComponent<HDK_UIFade> ().TextOut = true;
		}

		if (Player.GetComponent<HDK_Flashlight> ().hasFlashlight) {
			hasFlashlight = true;
			icon_flashlight.GetComponent<HDK_UIFade> ().TextIn = true;
            icon_flashlight.GetComponent<HDK_UIFade> ().TextOut = false;
		} else 
		{
			hasFlashlight = false;
            icon_flashlight.GetComponent<HDK_UIFade> ().TextIn = false;
            icon_flashlight.GetComponent<HDK_UIFade> ().TextOut = true;
		}

		if (hasDigitalCamera) {
			icon_digitalcamera.GetComponent<HDK_UIFade> ().TextIn = true;
            icon_digitalcamera.GetComponent<HDK_UIFade> ().TextOut = false;
		} else 
		{
            icon_digitalcamera.GetComponent<HDK_UIFade> ().TextIn = false;
            icon_digitalcamera.GetComponent<HDK_UIFade> ().TextOut = true;
		}

		if (n_Keys > 0) {
			hasKey = true;
			icon_key.GetComponent<HDK_UIFade> ().TextIn = true;
            icon_key.GetComponent<HDK_UIFade> ().TextOut = false;
			q_keys.text = n_Keys.ToString("F0");
		} else 
		{
			hasKey = false;
            icon_key.GetComponent<HDK_UIFade> ().TextIn = false;
            icon_key.GetComponent<HDK_UIFade> ().TextOut = true;
		}
	}

	IEnumerator CloseInventory()
	{
        fadeOut = false;
        fadeIn = true;
        open = true;
        yield return new WaitForSeconds (CloseWait);
        fadeOut = true;
        fadeIn = false;
        open = false;
    }
}