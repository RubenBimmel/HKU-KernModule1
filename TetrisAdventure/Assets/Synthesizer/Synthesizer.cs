using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Synthesizer : SynchronisedBehaviour
{

    // un-optimized version
    public double gain = 0.05;

    private int voices = 2;
    private double[] frequency;
    private double increment;
    private double[] phase;

    void Start()
    {
        increment = 2 * Math.PI / AudioSettings.outputSampleRate;
        phase = new double[voices];
        frequency = new double[voices];
    }

    public override void OnBeat(int beat)
    {
        if (beat % 2 == 0)
        {
            int note = (beat / 2) % Music.notes.GetLength(0);
            for (int i = 0; i < voices; i++)
            {
                frequency[i] = Music.notes[note, i];
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int iterator = 0; iterator < data.Length; iterator += channels)
        {

            float output = 0;

            // update increment in case frequency has changed

            for (int i = 0; i < voices; i++)
            {
                phase[i] += frequency[i] * increment;
                if (phase[i] > 2 * Math.PI)
                {
                    phase[i] -= 2 * Math.PI;
                }

                float saw = (float)(gain * (phase[i] / Mathf.PI - 1));
                float sine = (float)(gain * Math.Sin(phase[i]));
                output += Mathf.Lerp(saw, sine, .75f);
            }

            // if we have stereo, we copy the mono data to each channel
            data[iterator] = output;
            if (channels == 2) data[iterator + 1] = data[iterator];
        }
    }
}
