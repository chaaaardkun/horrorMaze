//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HDK_MouseZoom : MonoBehaviour {

	public float normal;
	public float zoom;
	public float smooth;
	public bool isZoomed;
	public bool canZoom;
	Camera main_camera;
	GameObject zoomIcon;

	public void ZoomOut()
	{
		isZoomed = false;
		canZoom = false;
	}

	void Start () {
		main_camera = GameObject.Find ("Camera").GetComponent<Camera>();
		zoomIcon = GameObject.Find ("icon_zooming");
	}

	void Update () {

		var d = Input.GetAxis("Mouse ScrollWheel");
	
		if (!GameObject.Find("Player").GetComponent<HDK_DigitalCamera>().UsingCamera) {
			if (canZoom) {
				if (d > 0f)
				{
					isZoomed = true;
				}
				else if (d < 0f)
				{
					isZoomed = false;
				}
			}

			if (isZoomed) {
				main_camera.fieldOfView = Mathf.Lerp (main_camera.fieldOfView, (float)this.zoom, Time.deltaTime * this.smooth);
				main_camera.GetComponent<TiltShift> ().enabled = true;
				zoomIcon.GetComponent<HDK_UIFade> ().TextIn = true;
				zoomIcon.GetComponent<HDK_UIFade> ().TextOut = false;
			} else {
				main_camera.fieldOfView = Mathf.Lerp (main_camera.fieldOfView, (float)this.normal, Time.deltaTime * this.smooth);
				main_camera.GetComponent<TiltShift> ().enabled = false;
				zoomIcon.GetComponent<HDK_UIFade> ().TextOut = true;
				zoomIcon.GetComponent<HDK_UIFade> ().TextIn = false;

			}
		}
		}
}