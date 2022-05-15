using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawingCards : NetworkBehaviour
{
    public NewWay PlayerManager;

    public void WhenClicked()
    {
         // We assign the network ID of a client goverened by the network client into our variable
        NetworkIdentity networkID = NetworkClient.connection.identity; // When the user clicks on it
        PlayerManager = networkID.GetComponent<NewWay>();
        PlayerManager.CmdDealCards();
    }
  
}
