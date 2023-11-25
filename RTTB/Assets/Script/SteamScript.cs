using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    public string pseudo;
    void Start()
    {
        if (SteamManager.Initialized)
        {
            pseudo = SteamFriends.GetPersonaName();
            Debug.Log(pseudo);
        }
    }
}
