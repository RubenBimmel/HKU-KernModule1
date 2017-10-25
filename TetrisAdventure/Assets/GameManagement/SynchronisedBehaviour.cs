using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronisedBehaviour : MonoBehaviour {

    // Adds this to sendBeat event
    public void Awake () {
        GamePulse.sendBeat += OnBeat;
    }

    // Removes this to sendBeat event
    public void OnDestroy () {
        GamePulse.sendBeat -= OnBeat;
    }

    // OnBeat is called by GamePulse on every beat
    public virtual void OnBeat (int beat) {
		
	}

}
