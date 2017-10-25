using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePulse : MonoBehaviour {

    public delegate void OnBeat(int beat);
    public static event OnBeat sendBeat;

    public static float beatSpeed = 9000;
    private float timer = 0f;
    private int beat = -1;

    private float lerpTimer;
    private float targetBeatSpeed;
	
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

    public void setBeatSpeedOverTime(float targetSpeed, float t)
    {
        lerpTimer = Time.time + t;
        targetBeatSpeed = targetSpeed;
        StartCoroutine("lerpBeatSpeed");
    }

    private IEnumerator lerpBeatSpeed()
    {
        while (Time.time < lerpTimer)
        {
            beatSpeed = Mathf.Lerp(beatSpeed, targetBeatSpeed, lerpTimer - Time.time);
            yield return null;
        }
        beatSpeed = targetBeatSpeed;
    }
}
