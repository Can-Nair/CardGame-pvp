using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; // We use mirror in order to zoom in only on the cards that we have authority over 
using UnityEngine.UI; // Since we are dealng with sprites

public class CardZoom : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomCard; // From Unity, as a prefab
    private GameObject zoomCard;
    private Sprite zoomSprite;

    public void Awake ()
    {
        Canvas = GameObject.Find("Main Canvas"); // initialize the variable via Unity
        zoomSprite = gameObject.GetComponent<Image>().sprite; //  initialize the variable with the sprite field of the image component of the current card (object)

    }
    // We will need to react when a mouse hovers over the card object, we will do this with event triggers
    public void OnHoverEnter()
    {
        // Preventing unnecessary code from executing  
        if (!hasAuthority) return;
        // Instantiate a new ZoomCard object with its vectors being the mouse position, but we add 250 to the vertical axis in order for the zoomed image to appear a little bit higher than the card that it is the zoomed position of
        zoomCard = Instantiate(ZoomCard, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity); // the quaternion refers to no rotation 
        zoomCard.GetComponent<Image>().sprite = zoomSprite; // We determine the image / sprite of our zoomed card image
        zoomCard.transform.SetParent(Canvas.transform, true); // We make it a child of the Canvas s that it materiallizes in front of everthing else, true because we do not want it to inherit its transform field from he canvas
        // Otherwise the card mirage wouldhave been placed right in the middle of the screen (Canvas)
        RectTransform rect = zoomCard.GetComponent<RectTransform>(); // Store the dimensions of our card mirage
        rect.sizeDelta = new Vector2(240, 344); // Then make it double 
    }

    public void OnHoverExit() 
    {
        Destroy(zoomCard); // The mirage disappears
    }
    
}
