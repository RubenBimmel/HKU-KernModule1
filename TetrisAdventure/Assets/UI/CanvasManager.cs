using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    private Text levelText;
    private Text speedText;
    private Text scoreText;

    void Awake ()
    {
        levelText = transform.Find("Text_level").GetComponent<Text>();
        if (!levelText)
        {
            Debug.LogError("Text_level not found");
        }

        speedText = transform.Find("Text_speed").GetComponent<Text>();
        if (!speedText)
        {
            Debug.LogError("Text_level not found");
        }

        scoreText = transform.Find("Text_score").GetComponent<Text>();
        if (!scoreText)
        {
            Debug.LogError("Text_score not found");
        }
    }

    public void setLevelText (int level)
    {
        levelText.text = level.ToString();
    }

    public void setSpeedText(int speed)
    {
        speedText.text = speed.ToString();
    }

    public void setScoreText(int score)
    {
        scoreText.text = string.Concat(" ",score.ToString());
    }

}
