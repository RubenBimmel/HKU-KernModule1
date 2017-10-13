using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public void setColour (Color colour)
    {
        transform.GetComponent<SpriteRenderer>().color = colour;
    }

    public void Destroy ()
    {
        Destroy(gameObject);
    }
}
