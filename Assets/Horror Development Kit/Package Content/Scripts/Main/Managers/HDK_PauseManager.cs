//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class HDK_PauseManager : MonoBehaviour {

    public bool GamePaused;
    public bool CanPause;

    [Header ("Main Variables")]
	public GameObject PauseMenu;
	public GameObject OptionsPanel;
    GameObject Player;

    [Header ("Load Fade Settings")]
    string Scene;
    public float secBeforeFade = 3.0f;
    public float fadeTime = 5.0f;
    public Texture fadeTexture;
    private bool fadeIn = false;
    private float tempTime;
    private float time = 0.0f;

    [Header ("Mouse SFX")]
	public AudioClip mouseHover;
	public AudioClip mouseClick;
	public float mouseVolume;

    void Start()
	{
        GamePaused = false;
        Player = GameObject.Find("Player");
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(secBeforeFade);
        fadeIn = true;
    }

    void OnGUI()
    {
        if (fadeIn)
        {
            Color colorT = GUI.color;
            colorT.a = tempTime;
            GUI.color = colorT;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }

    public void PlayHover()
	{
		GetComponent<AudioSource>().PlayOneShot(mouseHover, mouseVolume);
	}

	public void PlayClick()
	{
		GetComponent<AudioSource>().PlayOneShot(mouseClick, mouseVolume);
	}

	public void LoadScene (string sceneToLoad)
	{
        StartCoroutine(Fade());
        Scene = sceneToLoad;
	}

    public void UnPause()
    {
        GamePaused = false;
        PauseMenu.SetActive(false);
        Player.SetActive(true);
        gameObject.GetComponent<Camera>().enabled = false;
        gameObject.GetComponent<AudioListener>().enabled = false;
    }

    public void DoPause()
    {
        GamePaused = true;
        PauseMenu.SetActive(true);
        Player.SetActive(false);
        gameObject.GetComponent<Camera>().enabled = true;
        gameObject.GetComponent<AudioListener>().enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update () {
        
        if (fadeIn)
        {
            if (time < fadeTime) time += Time.deltaTime;
            tempTime = Mathf.InverseLerp(0.0f, fadeTime, time);
        }

        if (tempTime >= 1.0f)
            SceneManager.LoadScene(Scene);

        bool security = HDK_RaycastManager.UsingSecurityCam;

        //GAME PAUSE INPUT CHECK
        if (!GamePaused && CanPause && !security) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DoPause();
            }
        }
        else if(security)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Player.GetComponentInChildren<HDK_RaycastManager>().CloseCam();
                DoPause();
            }
        }

		//GAME OPTIONS SETTINGS
		if (OptionsPanel.activeInHierarchy == true)
		{
			//Texture
			if (QualitySettings.masterTextureLimit == 0)
			{
				GameObject.Find("TextureQuality").GetComponent<Dropdown>().value = 2;
			}

			if (QualitySettings.masterTextureLimit == 1)
			{
				GameObject.Find("TextureQuality").GetComponent<Dropdown>().value = 1;
			}

			if (QualitySettings.masterTextureLimit == 2)
			{
				GameObject.Find("TextureQuality").GetComponent<Dropdown>().value = 0;
			}
			//Anti aliasing
			if (QualitySettings.antiAliasing == 0)
			{
				GameObject.Find("AA").GetComponent<Dropdown>().value = 0;
			}

			if (QualitySettings.antiAliasing == 2)
			{
				GameObject.Find("AA").GetComponent<Dropdown>().value = 1;
			}

			if (QualitySettings.antiAliasing == 4)
			{
				GameObject.Find("AA").GetComponent<Dropdown>().value = 2;
			}

			if (QualitySettings.antiAliasing == 8)
			{
				GameObject.Find("AA").GetComponent<Dropdown>().value = 3;
			}
			//Anisotropic
			if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
			{
				GameObject.Find("AS").GetComponent<Dropdown>().value = 0;
			}

			if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
			{
				GameObject.Find("AS").GetComponent<Dropdown>().value = 1;
			}

			if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
			{
				GameObject.Find("AS").GetComponent<Dropdown>().value = 2;
			}
			//Geometry blend weights
			if (QualitySettings.blendWeights == BlendWeights.OneBone)
			{
				GameObject.Find("GeometryLevel").GetComponent<Dropdown>().value = 0;
			}
			if (QualitySettings.blendWeights == BlendWeights.TwoBones)
			{
				GameObject.Find("GeometryLevel").GetComponent<Dropdown>().value = 1;
			}
			if (QualitySettings.blendWeights == BlendWeights.FourBones)
			{
				GameObject.Find("GeometryLevel").GetComponent<Dropdown>().value = 2;
			}
			//Shadow cascades
			if (QualitySettings.shadowCascades == 0)
			{
				GameObject.Find("ShadowsCascades").GetComponent<Dropdown>().value = 0;
			}
			if (QualitySettings.shadowCascades == 2)
			{
				GameObject.Find("ShadowsCascades").GetComponent<Dropdown>().value = 1;
			}
			if (QualitySettings.shadowCascades == 4)
			{
				GameObject.Find("ShadowsCascades").GetComponent<Dropdown>().value = 2;
			}
			//Vsync option
			if (QualitySettings.vSyncCount == 0)
			{
				GameObject.Find("VSyncToogle").GetComponent<Toggle>().isOn = false;
			}
			if (QualitySettings.vSyncCount == 1)
			{
				GameObject.Find("VSyncToogle").GetComponent<Toggle>().isOn = true;
			}
			//
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}

    public void UpdateVolume(float v)
    {
        //Apply volume
        AudioListener.volume = v;
    }

    public void MSAALevel(int a)
	{
		a = GameObject.Find("AA").GetComponent<Dropdown>().value;
		if (a == 0)
		{
			QualitySettings.antiAliasing = 0;
		}
		if (a == 1)
		{
			QualitySettings.antiAliasing = 2;
		}
		if (a == 2)
		{
			QualitySettings.antiAliasing = 4;
		}
		if (a == 3)
		{
			QualitySettings.antiAliasing = 8;
		}
	}

	public void TextureQuality(int te)
	{
		te = GameObject.Find("TextureQuality").GetComponent<Dropdown>().value;
		if (te == 0)
		{
			QualitySettings.masterTextureLimit = 2;
		}
		if (te == 1)
		{
			QualitySettings.masterTextureLimit = 1;
		}
		if (te == 2)
		{
			QualitySettings.masterTextureLimit = 0;
		}
	}

	public void UpdateAnisotropic(int a)
	{
		a = GameObject.Find("AS").GetComponent<Dropdown>().value;
		if (a == 0)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		}
		if (a == 1)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
		}
		if (a == 2)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
		}
	}

	public void BlendWeight(int bw)
	{
		bw = GameObject.Find("GeometryLevel").GetComponent<Dropdown>().value;
		if (bw == 0)
		{
			QualitySettings.blendWeights = BlendWeights.OneBone;
		}
		if (bw == 1)
		{
			QualitySettings.blendWeights = BlendWeights.TwoBones;
		}
		if (bw == 2)
		{
			QualitySettings.blendWeights = BlendWeights.FourBones;
		}
	}

	public void VSync(bool vs)
	{
		vs = GameObject.Find("VSyncToogle").GetComponent<Toggle>().isOn;
		if(vs == true)
		{
			QualitySettings.vSyncCount = 1;
		}
		if (vs == false)
		{
			QualitySettings.vSyncCount = 0;
		}
	}

	public void ShadowsCascades(int s)
	{
		s = GameObject.Find("ShadowsCascades").GetComponent<Dropdown>().value;
		if (s == 0)
		{
			QualitySettings.shadowCascades = 0;
		}
		if (s == 1)
		{
			QualitySettings.shadowCascades = 2;
		}
		if (s == 2)
		{
			QualitySettings.shadowCascades = 4;
		}
	}
}