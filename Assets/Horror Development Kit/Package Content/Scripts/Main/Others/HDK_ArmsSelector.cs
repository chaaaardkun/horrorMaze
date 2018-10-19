//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;

public class HDK_ArmsSelector : MonoBehaviour {

	[Header ("Arms Texture Selector")]
	public SkinnedMeshRenderer[] ArmsMesh;		//The mesh of the arms model
	public bool Normal;							//Do you want normal texture?
	public bool Bloody;							//Do you want bloody texture?
	public Material normalMaterial;				//The material of the normalTexture
	public Material bloodyMaterial;				//The material of the bloodyTexture

	//THE SCRIPT DOESN'T WORK RUNTIME, SO USE IT BEFORE PLAY THE SCENE		

	void Start () 
	{
		if (!Normal && !Bloody) 
		{
			Debug.Log ("You must select at least one of the bool! Normal or Bloody!");
			Normal = true;
		}

		if (Normal && !Bloody) 
		{
			foreach (SkinnedMeshRenderer mesh in ArmsMesh) 
			{
				mesh.material = normalMaterial;
			}
		}else if(!Normal && Bloody)
		{
			foreach (SkinnedMeshRenderer mesh in ArmsMesh) 
			{
				mesh.material = bloodyMaterial;
			}
		}

		if (Normal && Bloody) 
		{
			Debug.Log ("You must select only one of the bool! Normal or Bloody, not both!");
			Bloody = false;
		}
	}
}