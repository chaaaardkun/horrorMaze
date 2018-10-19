//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace UnityStandardAssets.ImageEffects{
public class HDK_DigitalCamera : MonoBehaviour {

	[Header ("Digital Camera")]
	public AudioClip CameraOn;
	public AudioClip[] FoleySound;
	HDK_DigitalCameraZoom zoomScript;
	AudioSource audio_s;
	public GameObject CameraUI;
	GameObject mainCam;
	GameObject controlsText;
	GameObject anim;
	GameObject Player;
	GameObject infoText;
    GameObject interactText;
	public float sounds_volume;
	Animation NoItemsHands;
	bool HasPickedText;
    bool foleying;
    public static bool canUse;
	public bool HasCamera;
	public bool UsingCamera;
	bool TakeCamera;
	bool PutOutCamera;
    HDK_UITextManager TextManager;

    [Header("Broken Camera")]
    public AudioClip broke_sound;
    public AudioSource BrokenNoise;
    public GameObject brokenGUI;
    public GlitchEffect camera_effect;
    public bool broken;
    public bool BrokeIt;

    [Header("Night Vision")]
    public bool nv_canUse;
    public bool nv_Enabled = false;
    public float nv_Life;
    public float nv_MaxLife;
    public float nv_SpeedIncrease;
    public float nv_SpeedDecrease;
    public AudioClip nv_on;
    public AudioClip nv_off;
    public AudioClip nv_batterydead;
    public AudioSource nv_audiosource;
    private bool nv_soundplayed = true;
    private bool nv_batterysoundplayed = true;
    public float nv_soundvolume;
    private bool nv_turnOn = false;
    public Image nv_LifeGUI;
    public Light nv_light;

    void Start()
	{
		anim = GameObject.Find ("CamHolder");
		Player = GameObject.Find ("Player");
		canUse = true;
		audio_s = GameObject.Find("audio_Camera").GetComponent<AudioSource> ();
		mainCam = GameObject.Find ("Camera");
		infoText = GameObject.Find ("camera_picked");
		controlsText = GameObject.Find ("c_camera");
        interactText = GameObject.Find("t_camera");
        zoomScript = mainCam.GetComponent<HDK_DigitalCameraZoom> ();
        TextManager = Player.GetComponent<HDK_UITextManager>();
        infoText.GetComponent<Text>().text = TextManager.c_Pickup;
    }

    IEnumerator TakeCam()
	{
		Player.GetComponent<HDK_MouseZoom> ().SendMessage ("ZoomOut");
		anim.GetComponent<Animation> ().Play ("CamFoley", PlayMode.StopAll);
        foleying = true;
		audio_s.clip = FoleySound [Random.Range (0, FoleySound.Length)];
		audio_s.volume = sounds_volume;
		audio_s.Play ();
		yield return new WaitForSeconds (2.5f);
		audio_s.PlayOneShot (CameraOn, sounds_volume);
		zoomScript.enabled = true;
		CameraUI.SetActive (true);
        CameraUI.GetComponent<CanvasGroup>().alpha = 1;
        UsingCamera = true;
        foleying = false;
        mainCam.GetComponent<NoiseAndGrain> ().enabled = true;
		mainCam.GetComponent<VignetteAndChromaticAberration> ().intensity = 0.3f;
		mainCam.GetComponent<VignetteAndChromaticAberration> ().blur = 0.5f;
		mainCam.GetComponent<Fisheye> ().enabled = true;
		zoomScript.canZoom = true;
	    if (broken)	{
            brokenGUI.GetComponent<CanvasGroup>().alpha = 1;
            BrokenNoise.gameObject.SetActive(true);
            brokenGUI.SetActive (true);
		    mainCam.GetComponent<GlitchEffect> ().enabled = true;
		    zoomScript.canZoom = false;
	    }
        if (nv_Enabled)
        {
            mainCam.GetComponent<ColorCorrectionCurves>().enabled = true;
            nv_light.enabled = true;
            nv_Enabled = true;
        }
    }

	IEnumerator PutDownCam()
	{			
		if (zoomScript.zoomed) {
			zoomScript.SendMessage ("CloseCamera");
		}
		zoomScript.canZoom = false;
		Player.GetComponent<HDK_MouseZoom> ().enabled = true;
		Player.GetComponent<HDK_MouseZoom> ().canZoom = true;
		mainCam.GetComponent<Camera> ().fieldOfView = 60;
		anim.GetComponent<Animation> ().Play ("CamFoley", PlayMode.StopAll);
        foleying = true;
        audio_s.clip = FoleySound [Random.Range (0, FoleySound.Length)];
		audio_s.volume = sounds_volume;
		audio_s.Play ();
		yield return new WaitForSeconds (2.5f);
		UsingCamera = false;
        foleying = false;
        CameraUI.SetActive (false);
		mainCam.GetComponent<NoiseAndGrain> ().enabled = false;
		mainCam.GetComponent<VignetteAndChromaticAberration> ().intensity = 0.1f;
		mainCam.GetComponent<VignetteAndChromaticAberration> ().blur = 0.3f;
		mainCam.GetComponent<Fisheye> ().enabled = false;
		GetComponent<HDK_MouseZoom> ().canZoom = true;
		if (broken) {
			brokenGUI.SetActive (false);
			mainCam.GetComponent<GlitchEffect> ().enabled = false;
			zoomScript.enabled = false;
            BrokenNoise.gameObject.SetActive(false);
        }
        if (nv_Enabled)
        {
            mainCam.GetComponent<ColorCorrectionCurves>().enabled = false;
            nv_light.enabled = false;
            nv_Enabled = false;
        }
	}

    IEnumerator ShowOffKeyText()
	{
		yield return new WaitForSeconds (2);
		HasPickedText = false;
	}

	public void ShowPickedText()
	{
		HasPickedText = true;
	}

    IEnumerator FadeOutText(bool quickly)
    {
        if (!quickly)
        {
            yield return new WaitForSeconds(2);
            interactText.GetComponent<HDK_UIFade>().FadeOut();
        }
        else
        {
            interactText.GetComponent<HDK_UIFade>().FadeOut();
        }
    }

    void Update ()
    {
		if (HasPickedText) {
            infoText.GetComponent<HDK_UIFade>().FadeIn();
            controlsText.GetComponent<HDK_UIFade>().FadeIn();
			StartCoroutine (ShowOffKeyText ());
            StartCoroutine(FadeOutText(true));
		} else
		{
            infoText.GetComponent<HDK_UIFade>().FadeOut();
            controlsText.GetComponent<HDK_UIFade>().FadeOut();
		}

		if (BrokeIt) {
			broken = true;
			BrokeIt = false;
			audio_s.PlayOneShot (broke_sound, sounds_volume);
			brokenGUI.SetActive (true);
			camera_effect.enabled = true;
            BrokenNoise.gameObject.SetActive(true);
            if (zoomScript.zoomed) {
				zoomScript.SendMessage ("Broken");	
			}
		}

		if (TakeCamera) 
		{
			TakeCamera = false;
			StartCoroutine(TakeCam ());
		}

		if (PutOutCamera) 
		{
			PutOutCamera = false;
			StartCoroutine(PutDownCam());
		}

        bool examining = HDK_RaycastManager.ExaminingObject;
        bool security = HDK_RaycastManager.UsingSecurityCam;
        bool reading = HDK_RaycastManager.ReadingPaper;

        if (HasCamera) 
		{
            if (canUse)
            {
                if (!examining && !security && !reading && !foleying)
                {
                    if (Input.GetKeyUp(KeyCode.C))
                    {
                        if (!UsingCamera)
                        {
                            TakeCamera = true;
                        }
                        if (UsingCamera)
                        {
                            PutOutCamera = true;
                        }
                    }
                }
            }
		}
        else
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                interactText.GetComponent<HDK_UIFade>().FadeIn();
                interactText.GetComponent<Text>().text = TextManager.c_NegativeUse;
                StartCoroutine(FadeOutText(false));
            }
        }
        
        if (nv_turnOn)
        {
            if (UsingCamera && !examining && !security)
            {
                mainCam.GetComponent<ColorCorrectionCurves>().enabled = true;
                nv_light.enabled = true;
                nv_Enabled = true;
            }

            if (!nv_soundplayed)
            {
                nv_audiosource.PlayOneShot(nv_on);
                nv_audiosource.volume = nv_soundvolume;
                nv_soundplayed = true;
            }
        }

        if (!nv_turnOn)
        {
            mainCam.GetComponent<ColorCorrectionCurves>().enabled = false;
            nv_light.enabled = false;
            nv_Enabled = false;
            if (!nv_soundplayed || !nv_batterysoundplayed)
            {
                nv_audiosource.volume = nv_soundvolume;
                if (nv_batterysoundplayed)
                {
                    nv_audiosource.PlayOneShot(nv_off);
                    nv_soundplayed = true;
                }
                else
                {
                    nv_audiosource.PlayOneShot(nv_batterydead);
                    nv_batterysoundplayed = true;
                }
            }
        }

        if (UsingCamera && nv_canUse && !examining && !security && !reading)
        {
            if (Input.GetKeyUp(KeyCode.N))
            {
                if (nv_Enabled)
                {
                    nv_turnOn = false;
                }
                else
                {
                    nv_turnOn = true;
                }
                nv_soundplayed = false;
            }
        }

        if (nv_Life <= 0)
        {
            nv_Life = 0;
            nv_turnOn = false;
            nv_batterysoundplayed = false;
        }
        else if (nv_Life >= nv_MaxLife)
        {
            nv_Life = nv_MaxLife;
        }

        float nvhealth = nv_Life / 100f;
        nv_LifeGUI.fillAmount = nvhealth;

        if (HasCamera)
        {
            if (!nv_Enabled)
            {                    
                nv_Life += Time.deltaTime * nv_SpeedIncrease;
            }
            else
            {
                nv_Life -= Time.deltaTime * nv_SpeedDecrease;
            }
        }
    }
}
}