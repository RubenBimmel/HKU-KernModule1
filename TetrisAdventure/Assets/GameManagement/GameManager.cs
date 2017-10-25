using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GamePulse))]

public class GameManager : SynchronisedBehaviour {

    private GamePulse gamePulse;
    private int gameSpeed;
    private const int MINIMUM_GAME_SPEED = 10;

    public CanvasManager canvas;
    private int score;
    public synth.Synthesizer synthesizer;

    public Coin coinPrefab;
    public SlowDown slowDownPrefab;

    // Use this for initialization
    void Start()
    {

        if (!canvas)
        {
            Debug.LogError("Canvas is missing");
        }

        if (!coinPrefab)
        {
            Debug.LogError("Coin prefab is missing");
        }

        if (!slowDownPrefab)
        {
            Debug.LogError("SlowDown prefab is missing");
        }

        gamePulse = transform.GetComponent<GamePulse>();
        gameSpeed = 0;
    }

    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public int getSpeed ()
    {
        return gameSpeed;
    }

    public void setSpeed (int speed) {
        gameSpeed = speed;
        gamePulse.setBeatSpeedOverTime(MINIMUM_GAME_SPEED + gameSpeed, 8 - gameSpeed);
        canvas.setSpeedText(gameSpeed);
    }

    public void setSpeedOverTime(int speed, float t)
    {
        gameSpeed = speed;
        gamePulse.setBeatSpeedOverTime(MINIMUM_GAME_SPEED + gameSpeed, t);
        canvas.setSpeedText(gameSpeed);
    }

    public void addSpeed(int speed)
    {
        setSpeed(gameSpeed + speed);
    }

    public void addScore(int points)
    {
        score += points;
        canvas.setScoreText(score);
    }

    public void AddCoin(float x, float y)
    {
        Coin newCoin = Instantiate<Coin>(coinPrefab, new Vector3(x, y, 1), Quaternion.identity);
        newCoin.transform.parent = transform;
        newCoin.SetManager(this);
        newCoin.SetActive(true);
    }

    public void GameIsOver()
    {
        Time.timeScale = 0;
    }
}
