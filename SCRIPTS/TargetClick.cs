using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetClick : NetworkBehaviour
{
    public NewWay PlayerManager;
    public void OnTargetClick ()
    {
        // We declare and initialize our network ID
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<NewWay>();
        // If we have authority over this object (card)
        if(hasAuthority)
        {
            PlayerManager.CmdTargetSelfCard();
        }
        else
        {
            // else pass in the current game object 
            PlayerManager.CmdTargetOtherCard(gameObject);
        }
    } 
}
