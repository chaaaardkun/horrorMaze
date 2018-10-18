using UnityEngine;
using System.Collections;

public class LightsOff : MonoBehaviour
{
	public Transform firstLightParent;
 	public Transform secLightParent;
	public Transform thirdLightParent;
	public Transform fourthLightParent;
	public GameObject whiteLady;
	public AudioSource scareSound;
	private bool entered = false;

	void Start() {
		whiteLady.SetActive(false);
	}

	private void OnTriggerEnter(Collider trig) {
		if(entered == false){
			if (trig.tag == "Player"){
				entered = true;
				StartCoroutine(TurnOffLights());
			}
			
		}
	}

    IEnumerator TurnOffLights()
    {
		yield return new WaitForSeconds(0.5f);
        firstLight();
		scareSound.Play();
		whiteLady.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		secLight();
		whiteLady.transform.position = new Vector3(-23.263f, -0.003f, -14.926f);
		yield return new WaitForSeconds(1.2f);	
		thirdLight();
		whiteLady.transform.position = new Vector3(-23.263f, -0.003f, -10.926f);
		yield return new WaitForSeconds(1.2f);
		fourthLight();
		whiteLady.SetActive(false);
    }

	private void firstLight(){
		for (int j = 0; j < firstLightParent.childCount; j++)
         {
             firstLightParent.GetChild(j).gameObject.SetActive(false);
         }
	}
	private void secLight(){
		for (int j = 0; j < secLightParent.childCount; j++)
         {
             secLightParent.GetChild(j).gameObject.SetActive(false);
         }
	}
	private void thirdLight(){
		for (int j = 0; j < thirdLightParent.childCount; j++)
         {
             thirdLightParent.GetChild(j).gameObject.SetActive(false);
         }
	}
	private void fourthLight(){
		for (int j = 0; j < fourthLightParent.childCount; j++)
         {
             fourthLightParent.GetChild(j).gameObject.SetActive(false);
         }
	}
}