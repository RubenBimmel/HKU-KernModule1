﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    private Text levelText;
    private Text speedText;
    private Text scoreText;
    private int[] stats;
    private Text[] statsText;

    // Use this for initialisation
    void Awake () {
        levelText = transform.Find("Text_level").GetComponent<Text>();
        if (!levelText) {
            Debug.LogError("Text_level not found");
        }

        speedText = transform.Find("Text_speed").GetComponent<Text>();
        if (!speedText) {
            Debug.LogError("Text_level not found");
        }

        scoreText = transform.Find("Text_score").GetComponent<Text>();
        if (!scoreText) {
            Debug.LogError("Text_score not found");
        }

        //Text fields are not linked to shapes so if anything changes in the definition of the enum for Tetromino Shapes this needs to be changed to.
        stats = new int[7];
        statsText = new Text[7];
        statsText[0] = transform.Find("Text_stats_I").GetComponent<Text>();
        statsText[1] = transform.Find("Text_stats_J").GetComponent<Text>();
        statsText[2] = transform.Find("Text_stats_L").GetComponent<Text>();
        statsText[3] = transform.Find("Text_stats_O").GetComponent<Text>();
        statsText[4] = transform.Find("Text_stats_S").GetComponent<Text>();
        statsText[5] = transform.Find("Text_stats_T").GetComponent<Text>();
        statsText[6] = transform.Find("Text_stats_Z").GetComponent<Text>();
        for(int i = 0; i < 7; i++) {
            if (!statsText[i]) {
                Debug.LogError("Text_stats_# not found");
            }
        }
    }

    // Set UI text
    public void SetLevelText (int level) {
        levelText.text = level.ToString();
    }

    // Set UI text
    public void SetSpeedText(int speed) {
        speedText.text = speed.ToString();
    }

    // Set UI text
    public void SetScoreText(int score) {
        scoreText.text = score.ToString();
    }

    // Set UI text for given shape.
    public void UpdateStats (Tetromino.TetrominoShape shape) {
        stats[(int)shape]++;
        statsText[(int)shape].text = string.Concat("x ", stats[(int)shape].ToString());
    }

}
