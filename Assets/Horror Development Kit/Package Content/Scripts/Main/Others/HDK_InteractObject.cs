//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;

public class HDK_InteractObject : MonoBehaviour {

	[Header ("Interactable Item")]
	public string ItemName;					//The name of the item
	public bool Examined;					//Did you examine this item?
	public bool Interactable;				//Can you interact / take / use this item?
	public GameObject ExaminableObject;		//The object you will examine with mouse. Are located into the "ExamineObjectCamera"

}