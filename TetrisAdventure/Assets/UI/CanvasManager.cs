using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    private Text levelText;
    private Text speedText;

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
    }

    public void setLevelText (int level)
    {
        levelText.text = level.ToString();
    }

    public void setSpeedText(int speed)
    {
        speedText.text = speed.ToString();
    }

}
