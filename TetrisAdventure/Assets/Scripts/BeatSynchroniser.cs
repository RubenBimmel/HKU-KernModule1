using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSynchroniser : MonoBehaviour {

    public delegate void OnBeat(int beat);
    public static event OnBeat sendBeat;

    public float beatSpeed = 10f;
    private float timer = 0f;
    private int beat = 0;
    private const int beatCount = 4;
	
    // Update is called once per frame
	void Update () {
        float beatTime = 1 / beatSpeed;
        timer += Time.deltaTime;

        if (timer >= beatTime) {
            beat = ++beat % beatCount;
            sendBeat(beat);

            timer -= beatTime;
        }
	}


}
