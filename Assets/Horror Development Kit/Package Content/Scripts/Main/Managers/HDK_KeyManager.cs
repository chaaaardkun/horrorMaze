//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HDK_KeyManager : MonoBehaviour {

	[Header ("Key Management")]
	public bool HasKey;
	bool HasKeyText;
	public int Keys;
	GameObject infoText;
    HDK_UITextManager TextManager;

	void Start ()
	{
		infoText = GameObject.Find ("key_picked");
        TextManager = GameObject.Find("Player").GetComponent<HDK_UITextManager>();
	}

	void Update ()
	{	
		if (Keys > 0) {
			HasKey = true;
		} else 
		{
			HasKey = false;
		}

		if (HasKeyText)
        {
            infoText.GetComponent<HDK_UIFade>().FadeIn();
            infoText.GetComponent<Text>().text = TextManager.k_Pickup;
            StartCoroutine (ShowOffKeyText ());
		} else
		{
            infoText.GetComponent<HDK_UIFade>().FadeOut();
		}
	}

	public void AddKey()
	{
		HasKeyText = true;
		Keys += 1;
	}

	public void RemoveKey()
	{
		Keys -= 1;
	}

	IEnumerator ShowOffKeyText()
	{
		yield return new WaitForSeconds (2);
		HasKeyText = false;
	}
}