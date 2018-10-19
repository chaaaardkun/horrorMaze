//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;

public class HDK_Peak : MonoBehaviour {

	[Header ("Peek Settings")]
	public float leanAngle = 25.0f;
	public float leanSpeed = 5.0f;
	public float leanBackSpeed = 6.0f;
    private bool leanLeft;
	private bool leanRight;
	private bool noLean;
		
	void Update()
	{

		if (Input.GetKey("z") && noLean && !leanRight)
		{
			LeanLeft();
		}
		
		else if (Input.GetKey("x") && noLean && !leanLeft)
		{
			LeanRight();
		}
		
		else
		{
			LeanBack();
		}
	}
	public void LeanLeft()
	{
		leanRight = false;
        leanLeft = true;
		float currAngle = transform.rotation.eulerAngles.z;
		float targetAngle = leanAngle;
		if (currAngle > 180.0f)
		{
			currAngle = 360f - currAngle;
		}
		float angle = Mathf.Lerp(currAngle, targetAngle, leanSpeed * Time.deltaTime);
		Quaternion rotAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
		transform.rotation = rotAngle;
	}
	
	public void LeanRight()
	{
		leanLeft = false;
		leanRight = true;
		float currAngle = transform.rotation.eulerAngles.z;
		float targetAngle = -leanAngle;
		if (currAngle > 180.0f)
		{
			targetAngle = 360f - leanAngle;
		}
		float angle = Mathf.Lerp(currAngle, targetAngle, leanSpeed * Time.deltaTime);
		Quaternion rotAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
		transform.rotation = rotAngle;
	}
	
	public void LeanBack()
	{
		leanLeft = false;
		leanRight = false;
		noLean = true;
		float currAngle = transform.rotation.eulerAngles.z;
		float targetAngle = 0.0f;
		if (currAngle > 180.0f)
		{
			targetAngle = 360f;
		}
		float angle = Mathf.Lerp(currAngle, targetAngle, leanSpeed * Time.deltaTime);
		Quaternion rotAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
		transform.rotation = rotAngle;
	}
} 