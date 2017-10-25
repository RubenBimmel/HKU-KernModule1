using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace synth {
    public class Synthesizer : SynchronisedBehaviour {
        private const float amplification = 0.2f;

        private int voices = 3;     // Number of notes that can be played at the same time
        private double increment;   // Time for each audio calculation
        private double[] frequency; // Frequencies for all voices
        private double[] phase;     // Phases for all voices
        private float[] time;       // Time since last note of each voice started
        private float noiseTime;    // Time since last noise note started
        private bool leadSustain;   // Bool to trigger sustain (changes envelop)

        public enum Sound { triangle, square, noise}
        private Sound[] voiceChannels = {Sound.triangle, Sound.triangle, Sound.square };

        private System.Random rand = new System.Random();

        // Values for envelopes.
        private const float a = 0f;
        private const float d = .5f;
        private const float d2 = .1f;
        private const float sV = .1f;
        private const float s = 5f;

        // Use this for initialisation
        void Start() {
            increment = (double) 1 / AudioSettings.outputSampleRate;
            phase = new double[voices];
            frequency = new double[voices];
            time = new float[voices];
            noiseTime = 1;
        }

        // Gets called every beat
        public override void OnBeat(int beat) {
            int note = beat % Music.notes.GetLength(0);
            for (int i = 0; i < voices; i++) {
                if (Music.notes[note, i] >= 0) {
                    frequency[i] = Music.notes[note, i];
                    time[i] = 0;
                }
            }
            // Last part of music needs sustain. This is hacked in here (When done properly I should make a new array with sustain for all notes)
            leadSustain = (note >= 256);

            if (Music.noise[note % 32] == 1) {
                noiseTime = 0;
            }
        }

        // Calculate audio
        void OnAudioFilterRead(float[] data, int channels) {
            for (int iterator = 0; iterator < data.Length; iterator += channels) {
                float output = 0;

                for (int i = 0; i < voices; i++) {
                    // Sustain is hacked in. When active and only for lead voice the time since last note will remain 0
                    bool sustain = (i == 0) && leadSustain;
                    if (!sustain) {
                        time[i] += (float)increment;
                    }

                    phase[i] += frequency[i] * increment;
                    if (phase[i] > 1) {
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

        // Calculate soundWave
        private float getAudio(double phase, float time, Sound sound) {
            float value = 0;
            float gain  = 0;

            switch (sound) {
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

        // Attack Decay Sustain Release envelope. Release is instant so R is not needed
        private float adsrEnvelope(float time) {
            float value = 0;
            if (time < a) {
                value = time / a;
            }
            else if (time < (a + d)) {
                value = 1 - (time - a) / d * (1 - sV);
            }
            else if (time < (a + d + s)) {
                value = sV - (time - a - d) / s * sV;
            }
            return value;
        }

        // Simple envelope with unique values for noise
        private float adEnvelope(float time) {
            float value = 0;
            if (time < a) {
                value = time / a;
            }
            else if (time < (a + d2)) {
                value = 1 - (time - a) / d2;
            }
            return value;
        }
    }

}
