using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;  

public class NewWay : NetworkBehaviour
{
    // We declared these objects to use them in Unity (from the inspector window)
    public GameObject card1; // These will be initialized via Unity
    public GameObject card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    List<GameObject> cards = new List<GameObject>(); // Hold the cards

   public override void OnStartClient() // we override the base method
    {
        base.OnStartClient(); // The original method
        PlayerArea = GameObject.Find("PlayerArea"); // Linking the variable with the actual game object named: "PlayerArea"
        EnemyArea = GameObject.Find("EnemyArea"); // Doing the same for enemy area

        DropZone = GameObject.Find("DropZone"); //  Doing the same for drop zone
    }

    // This part will be run exclusively by the server, therefore we have the header "Server"
    [Server]
    public override void OnStartServer () // we override the base method
    {
        base.OnStartServer(); // The base method
        cards.Add(card1);
        cards.Add(card2);
        Debug.Log(cards); 
    }

    // Since the client is requesting something it is a command for the Server
    [Command]
    public void CmdDealCards()
    {
        // Use this for loop for creating the cards (populating) on our game field
        for (int i = 0; i < 5; i++)
        {
            // Creating the cards one after the other
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient); // Making sure deal cards happens for each client (spawning it for all clients)
            RpcShowCard(card, "Dealt"); // passing in the current card as "dealt", since it was just dealt  
           
        }
    }
    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);

    }
    // This methdod is used to invoke the RpcShowCard, thereby creating a waterfall effect.
    // This works as so: firstly the clint plays the card and then tells the server, via a command, which then tels all clients via a remote process call 
    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
        if(isServer)
        {
            UpdateTurnsPlayed();
        }
    }

    [Server]
     void UpdateTurnsPlayed()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>(); // placing the game manager script attached to Game Manager Object in to the variable gm
        gm.UpdateTurnsPlayed();
        RpcLogToClients("Turns Played: " + gm.TurnsPlayed);
    }
    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message); // Clients recieve the message of how many turns have been played
    }
    // A client remote procedure call type function for the server to tell the client what to do
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {   
        // If the card is dealt
        if(type == "Dealt") 
        {
            // If the first client has authority over the cards, i.e if they are their cards
            if (hasAuthority)
            {
                // We again make the player area be the parrent of the card
                card.transform.SetParent(PlayerArea.transform , false); // set to false since we want the card to inherit its transform from the player area 
            }
            // If this client does not have authority over these cards
            else
            {
                // then make the enemy area the parent of the card, since it is the enemy player's cards
                card.transform.SetParent(EnemyArea.transform , false);
                card.GetComponent<CardFilliper>().flip(); // We flip the card so that the opposing player does not see the player cards
            }
        }
        // Else if the card has been played 
        else if(type == "Played")
        {
            // make the card's tranform inherit that of the DropZone
            card.transform.SetParent(DropZone.transform , false);
            // If we have recied a remote process call from the server about a card spawning but we do not have authority over it
            if(!hasAuthority)
            {
                // then flip the cad so we do not see it 
                card.GetComponent<CardFilliper>().flip();
            }
        }
    }
    // We will once again do a waterfall structure, but this time the erver wlll target specific clients, with target RPCs
    [Command]
    public void CmdTargetSelfCard()
    {
        TargetSelfCard();
    }
    [Command]
    public void CmdTargetOtherCard (GameObject target)
    {
        // We take the identity of the apponent card
        NetworkIdentity enemyID = target.GetComponent<NetworkIdentity>();
        TargetOtherCard(enemyID.connectionToClient); // and pass it in the "TargetOtherCard" target RPCs function, it targets a specific enemy card
    }
    [TargetRpc]
    void TargetSelfCard()
    {
        Debug.Log("Tareted by self!");
    }
    [TargetRpc]
    void TargetOtherCard(NetworkConnection target)
    {
        Debug.Log("Targeted by other!");
    }
    // Same as before we shall create a waterfall method
    [Command]
    public void CmdIncrementClick(GameObject card)
    {
        RpcIncrementClick(card);
    }
    [ClientRpc]
    void RpcIncrementClick(GameObject card)
    {
        // Find the IncrementClick script attached to the card object and from there 
        // find the public variable NumberOfCliks and increment it
        card.GetComponent<IncrementClick>().NumberOfClicks++;
        Debug.Log("This card has been clicked " + card.GetComponent<IncrementClick>().NumberOfClicks + " times"); // Log it in the debug
    }
}
