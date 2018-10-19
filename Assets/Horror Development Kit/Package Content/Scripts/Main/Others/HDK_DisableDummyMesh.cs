//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;

public class HDK_DisableDummyMesh : MonoBehaviour {

	public GameObject Anims;

	void Update () {

		if (!Anims.GetComponent<Animation> ().isPlaying) {
			foreach (SkinnedMeshRenderer meshes in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				meshes.enabled = false;
			}
		} else 
		{
			foreach (SkinnedMeshRenderer meshes in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				meshes.enabled = true;
			}
		}	
	}
}