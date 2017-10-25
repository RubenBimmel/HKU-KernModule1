using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class UIValue : MonoBehaviour {

	public enum Value { Level, Speed };
    public Value value;

    private Text text;

    void Awake ()
    {
        text = transform.GetComponent<Text>();
    }

    void Update ()
    {
        switch (value)
        {
            case Value.Level:
                text.text = GameManager.levelAtStart.ToString();
                break;
            case Value.Speed:
                text.text = GameManager.gameSpeedAtStart.ToString();
                break;
        }
    }
}
