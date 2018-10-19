//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Characters.FirstPerson;

public class HDK_DigitalCameraZoom : MonoBehaviour {

	[Header ("Camera Zoom")]
	public bool zoomed;
	public float volume;
	public AudioClip ZoomIn;
	public AudioClip ZoomOut;
	public AudioClip ZoomBroken;
	bool zooming;
	public float lenght;
	public Slider lineIndicator;
	HDK_DigitalCamera CameraScript;
	public bool canZoom;

    void Start()
	{
		CameraScript = GameObject.Find ("Player").GetComponent<HDK_DigitalCamera>();
		canZoom = true;
	}

	IEnumerator zoomming()
	{
		zooming = true;
		yield return new WaitForSeconds (lenght);
		zooming = false;
	}

	void Update () {

		if (zooming) 
		{
			if (!zoomed) {
				lineIndicator.value -= Time.deltaTime * lenght;
			} else 
			{
				lineIndicator.value += Time.deltaTime * lenght;
			}
		}

        if (Input.GetKeyDown (KeyCode.Mouse2) && !zooming && !CameraScript.broken && canZoom) {
			if (!zoomed) {
				StartCoroutine (zoomming ());
				zoomed = true;
				GetComponent<Animation> ().Play ("ZoomIn"); 
				GetComponent<AudioSource> ().PlayOneShot (ZoomIn, volume);
			} else {
				StartCoroutine (zoomming ());
				zoomed = false;
				GetComponent<Animation> ().Play ("ZoomOut"); 
				GetComponent<AudioSource> ().PlayOneShot (ZoomOut, volume);
			}
		} 
		else if (Input.GetKeyDown (KeyCode.Mouse2) && !zooming && CameraScript.broken) 
		{
			this.GetComponent<AudioSource> ().PlayOneShot (ZoomBroken, volume);
		}
	}

	public void Broken()
	{
		StartCoroutine (zoomming ());
		canZoom = false;
		zoomed = false;
		this.GetComponent<Animation> ().Play ("ZoomOut"); 
		this.GetComponent<AudioSource> ().PlayOneShot (ZoomOut, volume);
	}

	public void CloseCamera()
	{
		StartCoroutine (zoomming ());
		zoomed = false;
		this.GetComponent<Animation> ().Play ("ZoomOut"); 
		this.GetComponent<AudioSource> ().PlayOneShot (ZoomOut, volume);
	}

}