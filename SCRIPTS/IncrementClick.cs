using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class IncrementClick :NetworkBehaviour
{
    public NewWay PlayerManager;
    // the syncvar for syncining variables
    [SyncVar]
    public int NumberOfClicks = 0;

    public void IncrementClicks()
    {
        // we get the network ID
        NetworkIdentity networkID = NetworkClient.connection.identity;
        PlayerManager = networkID.GetComponent<NewWay>(); // Initialize Player Manager
        // Invoke the command 
        PlayerManager.CmdIncrementClick(gameObject);

    }
}
