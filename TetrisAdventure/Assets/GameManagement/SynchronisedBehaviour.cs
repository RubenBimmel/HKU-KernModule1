using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronisedBehaviour : MonoBehaviour {

    public void Awake () {
        GamePulse.sendBeat += OnBeat;
    }

    public void OnDestroy ()
    {
        GamePulse.sendBeat -= OnBeat;
    }

    // Update is called on every beat
    public virtual void OnBeat (int beat) {
		
	}

}
