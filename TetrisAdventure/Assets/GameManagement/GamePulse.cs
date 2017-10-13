using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePulse : MonoBehaviour {

    public delegate void OnBeat(int beat);
    public static event OnBeat sendBeat;

    public static float beatSpeed = 9000;
    private float timer = 0f;
    private int beat = -1;
	
    // Update is called once per frame
	void Update () {
        float beatTime = 1 / beatSpeed;
        timer += Time.deltaTime;

        if (timer >= beatTime) {
            beat++;
            sendBeat(beat);

            timer -= beatTime;
        }
	}


}
