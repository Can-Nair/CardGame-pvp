using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // we do not need to use mirror since this script shall execute after the networking has been completed

public class CardFilliper : MonoBehaviour
{
    public Sprite CardFront; // The front of our card 
    public Sprite CardBack; // The back of our card 

    public void flip()
    {
        //  we shall store the current sprite in this variable
        Sprite currentSprite = gameObject.GetComponent<Image>().sprite; // We get the sprite field of the image compoent of the current game object
        // (lower case g indicates the current game object)
        // If the current sprite that we have is that of the card front
        if(currentSprite == CardFront) 
        {
            gameObject.GetComponent<Image>().sprite = CardBack; // We flip the card by turning the front to the back (literally!)
        }
        // If it is not (meaning we have the back of the card)
        else
        {
            gameObject.GetComponent<Image>().sprite = CardFront; // We flip it the other way around
        }
    }
}
