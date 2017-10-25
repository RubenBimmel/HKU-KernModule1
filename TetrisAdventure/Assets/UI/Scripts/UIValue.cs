using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class UIValue : MonoBehaviour {

	public enum Value { Level, Speed };
    public Value value;

    private Text text;

    // Use this for initialisation
    void Awake () {
        text = transform.GetComponent<Text>();
    }

    // Update is called each frame. Gets value to be showed on screen
    void Update () {
        switch (value) {
            case Value.Level:
                text.text = GameManager.levelAtStart.ToString();
                break;
            case Value.Speed:
                text.text = GameManager.gameSpeedAtStart.ToString();
                break;
        }
    }
}
