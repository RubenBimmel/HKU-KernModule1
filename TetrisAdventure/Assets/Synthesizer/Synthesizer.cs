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
        private float noiseTime;
        private bool leadSustain;

        public enum Sound { triangle, square, noise}
        private Sound[] voiceChannels = {Sound.triangle, Sound.triangle, Sound.square };

        private System.Random rand = new System.Random();

        private const float a = 0f;
        private const float d = .5f;
        private const float d2 = .1f;
        private const float sV = .1f;
        private const float s = 5f;

        void Start()
        {
            increment = (double) 1 / AudioSettings.outputSampleRate;
            phase = new double[voices];
            frequency = new double[voices];
            time = new float[voices];
            noiseTime = 1;
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
            leadSustain = (note >= 256);

            if (Music.noise[note % 32] == 1)
            {
                noiseTime = 0;
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
                    bool sustain = (i == 0) && leadSustain;

                    if (!sustain)
                    {
                        time[i] += (float)increment;
                    }

                    phase[i] += frequency[i] * increment;
                    if (phase[i] > 1)
                    {
                        phase[i] -= 1;
                    }

                    output += amplification * getAudio(phase[i], time[i], voiceChannels[i]);
                }

                noiseTime += (float)increment;
                output += amplification * getAudio(0, noiseTime, Sound.noise);

                // if we have stereo, we copy the mono data to each channel
                data[iterator] = output;
                if (channels == 2) data[iterator + 1] = data[iterator];
            }
        }

        private float getAudio(double phase, float time, Sound sound)
        {
            float value = 0;
            float gain  = 0;

            switch (sound)
            {
                case Sound.triangle:
                    gain = adsrEnvelope(time);
                    double triangle = phase * 4 - 1;
                    if (triangle > 1) triangle = 1 - triangle;
                    value = Mathf.Floor((float)triangle * 16) / 16;
                    break;
                case Sound.square:
                    gain = adsrEnvelope(time);
                    if (phase > .5) value = 1;
                    break;
                case Sound.noise:
                    gain = adEnvelope(time);
                    value = (float)(rand.NextDouble() * 2 - 1);
                    break;
            }

            return value * gain;
        }

        private float adsrEnvelope(float time)
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

        private float adEnvelope(float time)
        {
            float value = 0;
            if (time < a)
            {
                value = time / a;
            }
            else if (time < (a + d2))
            {
                value = 1 - (time - a) / d2;
            }
            return value;
        }
    }

}
