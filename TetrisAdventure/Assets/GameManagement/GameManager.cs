﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GamePulse))]

public class GameManager : MonoBehaviour {

    private GamePulse gamePulse;

    public enum GameType {Adventure, Classic }
    public static GameType type;

    public static int gameSpeedAtStart;
    private int gameSpeed;
    private const int MINIMUM_GAME_SPEED = 10;

    public static int levelAtStart;
    private int level;
    private const int POINTS_FOR_NEXT_LEVEL = 2500;
    private const int LEVEL_CAP = 10;

    private CanvasManager canvas;
    private int score;
    public synth.Synthesizer synthesizer;

    public Coin coinPrefab;
    public SlowDown slowDownPrefab;
    public EnemyController[] enemyPrefabs;

    // Use this for initialization
    void Awake() {

        if (!coinPrefab) {
            Debug.LogError("Coin prefab is missing");
        }

        if (!slowDownPrefab) {
            Debug.LogError("SlowDown prefab is missing");
        }

        gamePulse = transform.GetComponent<GamePulse>();
    }

    // Initialization after all objects are awake
    void Start() {
        GameObject activeCanvas = GameObject.FindGameObjectWithTag("Canvas");

        if (!activeCanvas) {
            Debug.LogError("Canvas is missing");
        }

        canvas = activeCanvas.GetComponent<CanvasManager>();

        SetSpeed(gameSpeedAtStart);
        SetLevel(levelAtStart);

        Time.timeScale = 1;
    }

    // Get input for gameflow
    void Update() {
        if (Input.GetButton("Cancel")) {
            SceneManager.LoadScene(0);
        }
    }

    public int getSpeed () {
        return gameSpeed;
    }

    public void ResetSpeed () {
        SetSpeed(gameSpeedAtStart);
    }

    // SetSpeed using default lerp
    public void SetSpeed (int speed) {
        gameSpeed = speed;
        gamePulse.SetBeatSpeedOverTime(MINIMUM_GAME_SPEED + gameSpeed, 8 - gameSpeed);
        canvas.SetSpeedText(gameSpeed);
    }

    // SetSpeed using custom lerp
    public void setSpeedOverTime(int speed, float t) {
        gameSpeed = speed;
        gamePulse.SetBeatSpeedOverTime(MINIMUM_GAME_SPEED + gameSpeed, t);
        canvas.SetSpeedText(gameSpeed);
    }

    // Add points to the speed and update UI
    public void addSpeed(int speed) {
        SetSpeed(gameSpeed + speed);
    }

    // Add points to the level and update UI
    public void SetLevel(int newLevel) {
        level = newLevel;
        canvas.SetLevelText(level);
    }

    // Add points to the score, calculate new level and update UI
    public void addScore(int points) {
        score += points;
        level = Mathf.Min(levelAtStart + Mathf.FloorToInt(score / POINTS_FOR_NEXT_LEVEL), LEVEL_CAP);
        canvas.SetScoreText(score);
        canvas.SetLevelText(level);
    }

    // Spawns a Coin
    public void AddCoin(float x, float y) {
        Coin newCoin = Instantiate<Coin>(coinPrefab, new Vector3(x, y, 1), Quaternion.identity);
        newCoin.transform.parent = transform;
        newCoin.SetManager(this);
        newCoin.SetActive(true);
    }

    // Spawns an enemy on a random position on the grid
    public void SpawnEnemy(EnemyController enemyPrefab, GameGrid grid, int[,] mask) {
        int spawnXPos = Random.Range(0, grid.width);
        int spawnYPos = grid.getHeighestAvailableCellInColumn(spawnXPos, mask);
        if (spawnYPos < grid.height) {
            spawnYPos = Mathf.Min(grid.height, spawnYPos + 2);
            Vector2 spawnPos = grid.GetWorldPosition(spawnXPos, spawnYPos);

            EnemyController newEnemy = Instantiate<EnemyController>(enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.SetManager(this);
        }
    }


    // On each new Tetromino the stats are updated. For adventure mode this also spawns enemies depending on the level
    public void OnNewTetromino (Tetromino activeTetromino, int counter, GameGrid grid) {
        canvas.UpdateStats(activeTetromino.shape);

        if (type == GameType.Adventure) {
            switch (level) {
                case 1:
                    if (counter % 4 == 0) {
                        SpawnEnemy(enemyPrefabs[0], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 2:
                    if (counter % 4 == 0) {
                        SpawnEnemy(enemyPrefabs[1], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 3:
                    if (counter % 4 == 0) {
                        SpawnEnemy(enemyPrefabs[2], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 4:
                    if (counter % 2 == 0) {
                        SpawnEnemy(enemyPrefabs[Random.Range(1, 2)], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 5:
                    if (counter % 3 == 0) {
                        SpawnEnemy(enemyPrefabs[Random.Range(1, 2)], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 6:
                    if (counter % 3 == 0) {
                        SpawnEnemy(enemyPrefabs[3], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 7:
                    if (counter % 3 == 0) {
                        SpawnEnemy(enemyPrefabs[Random.Range(0, 1)], grid, activeTetromino.blockPositions);
                        SpawnEnemy(enemyPrefabs[Random.Range(0, 1)], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 8:
                    if (counter % 3 == 0) {
                        SpawnEnemy(enemyPrefabs[Random.Range(2, 3)], grid, activeTetromino.blockPositions);
                        SpawnEnemy(enemyPrefabs[Random.Range(2, 3)], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 9:
                    if (counter % 2 == 0) {
                        SpawnEnemy(enemyPrefabs[4], grid, activeTetromino.blockPositions);
                    }
                    break;
                case 10:
                    SpawnEnemy(enemyPrefabs[Random.Range(0, 4)], grid, activeTetromino.blockPositions);
                    if (counter % 4 == 0) {
                        SpawnEnemy(enemyPrefabs[Random.Range(0, 4)], grid, activeTetromino.blockPositions);
                    }
                    break;
            }
        }
    }

    // Gets called when the player dies or Tetris grid is full
    public void GameIsOver() {
        Time.timeScale = 0;
    }
}
