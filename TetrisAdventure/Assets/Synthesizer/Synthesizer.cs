using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace synth
{

    public class Synthesizer : SynchronisedBehaviour
    {

        // un-optimized version
        private const float amplification = 0.2f;

        private int voices = 3;
        private double increment;
        private double[] frequency;
        private double[] phase;
        private float[] time;

        public enum Sound { triangle, square, noise}

        private System.Random rand = new System.Random();

        private const float a = 0f;
        private const float d = .5f;
        private const float sV = .1f;
        private const float s = 5f;

        void Start()
        {
            increment = (double) 1 / AudioSettings.outputSampleRate;
            phase = new double[voices];
            frequency = new double[voices];
            time = new float[voices];
        }

        public override void OnBeat(int beat)
        {
            int note = beat % Music.notes.GetLength(0);
            for (int i = 0; i < voices; i++)
            {
                if (Music.notes[note, i] >= 0)
                {
                    frequency[i] = Music.notes[note, i];
                    time[i] = 0;
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
                    if (i < 2)
                    {
                        time[i] += (float)increment;

                        phase[i] += frequency[i] * increment;
                        if (phase[i] > 1)
                        {
                            phase[i] -= 1;
                        }

                        output += amplification * getAudio(phase[i], time[i], Sound.triangle);
                    }
                    else
                    {
                        if(frequency[i]>0)
                            output += amplification * getAudio(0, time[i], Sound.noise);
                    }
                }

                // if we have stereo, we copy the mono data to each channel
                data[iterator] = output;
                if (channels == 2) data[iterator + 1] = data[iterator];
            }
        }

        private float getAudio(double phase, float time, Sound sound)
        {
            float value = 0;
            float gain = envelope(time);

            switch (sound)
            {
                case Sound.triangle:
                    double triangle = phase * 4 - 1;
                    if (triangle > 1) triangle = 1 - triangle;
                    value = Mathf.Floor((float)triangle * 16) / 16;
                    break;
                case Sound.square:
                    if (phase > .5) value = 1;
                    break;
                case Sound.noise:
                    value = (float)(rand.NextDouble() * 2 - 1);
                    gain = (gain - .995f) * 100;
                    break;
            }

            return value * gain;
        }

        private float envelope(float time)
        {
            float value = 0;
            if (time < a)
            {
                value = time / a;
            }
            else if (time < (a + d))
            {
                value = 1 - (time - a) / d * (1 - sV);
            }
            else if (time < (a + d + s))
            {
                value = sV - (time - a - d) / s * sV;
            }
            return value;
        }
    }

}
