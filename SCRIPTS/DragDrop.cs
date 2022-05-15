using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    // Assume the card is not getting draged in the bagnning
    private bool isdrag = false;
    public NewWay PlayerManager; // The player manager for networking
    private bool isDraggable = true; // check if the client has authority over an object
    public GameObject canvas;
    private GameObject ParentStart;
    private Vector2 PositionStart;
    private GameObject dropZone;
    private bool isOverDropZone;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Main Canvas");
        if(!hasAuthority)
        {
            isDraggable = false;
        }

    }
    private void OnCollisionEnter2D (Collision2D collision)
    {
        Debug.Log("Colliding!");
        isOverDropZone = true;
        dropZone = collision.gameObject; // Whatever it is colliding with becomes the drop zone 
    }
    private void OnCollisionExit2D (Collision2D collision)
    {
        Debug.Log("Uncolliding!");
        isOverDropZone=false;
        dropZone = null; // If there is no collision then we do not have a drop zone
    }

    //  Method for the starting of the dragging action 
    public void StartOfDrag() {

        if (!isDraggable) return; // This part ensures that the code for dragging a card does not execute for an undraggable card
        isdrag = true; // Sice the card is getting dragged
        ParentStart = transform.parent.gameObject; // This is used to assign the player area to ParentStart 
        //  We store the position of this transform before it is dragged
        PositionStart = transform.position; 

    }

    // Method for the endin of the dragging action
    public void EndOfDrag() {

        if (!isDraggable) return;
        isdrag = false; // Sice we have completed the dragging process
        if (isOverDropZone)
        {
            // If the player places the card in an appropriate place (the drop zone) then we will make the drop zone the parent of the card 
            transform.SetParent(dropZone.transform, false); // Therefore it will place the card right into the center of the drop zone
            isDraggable = false; //  after you have placed the card the card becomes unmoveable
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<NewWay>(); //                                                                              !!!
            PlayerManager.PlayCard(gameObject);
        }
        else
        {
            // If the player does not do a permited move, i.e. placing the card onto an inapproprate place
            transform.position = PositionStart; // Update the position as the original start position
            // And subordinate its traqnsform (place on the play field) to the trnsform of the parent at the start
            transform.SetParent(ParentStart.transform, false); // and since the original parent was the player area (we say false since we want the world postion to change)
            // These two lines will allow us to place the card back into the player area (with its initial start coordinates)
            // We set parent once again since the parent of our card, when we first began to drag it, was the canvas (we had done this in order for it to render properly) 
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isdrag)
        {
            //making sure the card follows the mouse
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(canvas.transform, true); //We want it to keep its position therefore we say: true
        }
    }
}
