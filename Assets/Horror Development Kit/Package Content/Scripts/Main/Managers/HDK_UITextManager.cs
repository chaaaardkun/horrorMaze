//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDK_UITextManager : MonoBehaviour
{
    [Header("Flashlight")]
    public string f_Pickup = "";           //When you pickup the flashlight
    public string f_NegativeUse = "";      //When you press <<f>> but you don't have the flashlight

    [Header("Batteries")]
    public string b_Pickup = "";           //When you pickup a battery
    public string b_NegativePickup = "";   //When you want to pickup a battery but you reached the limit of batteries
    public string b_NegativeUse = "";      //When you press <<q>> to charge the flashlight but you don't have batteries 

    [Header("Keys")]
    public string k_Pickup = "";           //When you pickup a key

    [Header("Digital Camera")]
    public string c_Pickup = "";           //When you pickup the digital camera
    public string c_NegativeUse = "";      //When you press <<c>> but you don't have the digital camera

    [Header("Doors & Furnitures")]
    public string d_NegativeUse = "";      //When you try to open a locked door
    public string d_ImpossibleUse = "";    //When you try to open a jammed door
}