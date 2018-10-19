//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HDK_DynamicObject : MonoBehaviour {

    [Header("Main Settings")]
    public bool isDoor;
    public GameObject animPrefab; 	//The door body child of the door prefab.
	public bool doorOpen = false; 	//Bool used to check the state of the door, if it's open or not.
	public string OpenAnimation;
	public string CloseAnimation;
	public string LockedAnimation;
	GameObject infoText;
	GameObject Player;
	public bool hasLockAnim;
    HDK_UITextManager TextManager;

    [Header ("SFX")]
	public GameObject audioSource; 	//The prefab's audio source GameObject, from which the sounds are played.	
	public AudioClip openSound; 	//The door opening sound effect
	public AudioClip closeSound; 	//The door closing sound effect	
	public AudioClip lockedSound;
	public float volumeSounds = 1;	//Volume of open - close sounds

	[Header ("Security")]
	public bool Jammed;
	public bool Locked;
	public bool Free;
	bool clicked;
	public bool wasLocked;

	void Start()
	{
		infoText = GameObject.Find ("t_door");
		Player = GameObject.Find ("Player");
        TextManager = GameObject.Find("Player").GetComponent<HDK_UITextManager>();
    }

    void Update()
    {
        if (isDoor)
        {
            if (animPrefab.GetComponent<BoxCollider>())
            {
                if (animPrefab.GetComponent<Animation>().isPlaying)
                {
                    animPrefab.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    animPrefab.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
    }

    public void OpenDoor()
	{
		Free = true;
		Locked = false;
		wasLocked = true;
	}
	
	public void removeWasLocked()
	{
		wasLocked = false;
		Player.GetComponent<HDK_KeyManager> ().RemoveKey ();
	}

	public void doorOpenClose() {

	if (animPrefab.GetComponent<Animation>().isPlaying == false) 
		{
			if (!doorOpen) 
				{
				animPrefab.GetComponent<Animation>().Play(OpenAnimation);
				audioSource.GetComponent<AudioSource>().clip = openSound;
				audioSource.GetComponent<AudioSource>().volume = volumeSounds;
				audioSource.GetComponent<AudioSource>().Play();
				doorOpen = true;
				}
				else
				{
				animPrefab.GetComponent<Animation>().Play(CloseAnimation);
				audioSource.GetComponent<AudioSource>().clip = closeSound;
				audioSource.GetComponent<AudioSource>().volume = volumeSounds;
				audioSource.GetComponent<AudioSource>().Play();
				doorOpen = false;
			}
		}
	}

	IEnumerator FadeOutInfo()
	{
        yield return new WaitForSeconds (2);
		infoText.GetComponent<HDK_UIFade> ().FadeOut();
	}

	public void doorLocked()
	{
		if (hasLockAnim) 
		{
			animPrefab.GetComponent<Animation>().Play(LockedAnimation);
		}
		audioSource.GetComponent<AudioSource>().clip = lockedSound;
		audioSource.GetComponent<AudioSource>().volume = volumeSounds;
		audioSource.GetComponent<AudioSource>().Play();
        infoText.GetComponent<Text>().text = TextManager.d_NegativeUse;
        infoText.GetComponent<HDK_UIFade>().FadeIn();
        StartCoroutine (FadeOutInfo ());
	}

	public void doorJammed()
	{
		if (hasLockAnim) 
		{
			animPrefab.GetComponent<Animation>().Play(LockedAnimation);
		}
		audioSource.GetComponent<AudioSource>().clip = lockedSound;
		audioSource.GetComponent<AudioSource>().volume = volumeSounds;
		audioSource.GetComponent<AudioSource>().Play();
        infoText.GetComponent<Text>().text = TextManager.d_ImpossibleUse;
        infoText.GetComponent<HDK_UIFade>().FadeIn();
        StartCoroutine (FadeOutInfo ());
	}
}