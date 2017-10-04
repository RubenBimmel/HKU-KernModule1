using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour {

    public UnityEvent buttonEvent;

    void OnMouseDown ()
    {
        buttonEvent.Invoke();
    }
}
