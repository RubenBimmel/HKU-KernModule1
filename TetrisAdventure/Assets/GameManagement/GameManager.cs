using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SynchronisedBehaviour {

    private enum TetrisState { start, newBlock, blockIsMoving, blockHasLanded, gameOver}
    private TetrisState state;

    public GameGrid gameGrid;
    public Grid previewGrid;
    private Tetromino activeTetromino;
    private Tetromino nextTetromino;

    public int tetrisSpeed;
    private const int tetrisStartSpeed = 10;
    private int rowCounter;
    private const int ROWS_FOR_SPEED_UP = 8;

    public CanvasManager canvas;

    private bool rotate;

    // Use this for initialization
    void Start()
    {
        if (!gameGrid || !previewGrid)
        {
            Debug.LogError("Grid is missing");
        }

        if (!canvas)
        {
            Debug.LogError("Canvas is missing");
        }

        state = TetrisState.start;
        tetrisSpeed = 0;
        rowCounter = 0;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("up"))
        {
            rotate = true;
        }
    }

    // Gets called every beat
    public override void OnBeat (int beat) {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene(0);
        }

        switch (state)
        {
            case TetrisState.start:
                previewGrid.ClearGrid();
                gameGrid.ClearGrid();

                nextTetromino = new Tetromino();
                nextTetromino.AddToGrid(previewGrid);

                GamePulse.beatSpeed = tetrisStartSpeed;
                tetrisSpeed = 0;
                canvas.setSpeedText(tetrisSpeed);

                state = TetrisState.newBlock;
                break;

            case TetrisState.newBlock:
                if (nextTetromino == null)
                {
                    activeTetromino = new Tetromino();
                }
                else
                {
                    activeTetromino = nextTetromino;
                }

                previewGrid.ClearGrid();
                if (activeTetromino.AddToGrid(gameGrid))
                {
                    nextTetromino = new Tetromino();
                    nextTetromino.AddToGrid(previewGrid);
                    state = TetrisState.blockIsMoving;
                }
                else
                {
                    state = TetrisState.gameOver;
                }
                break;

            case TetrisState.blockIsMoving:
                if (Input.GetKey("left"))
                {
                    activeTetromino.Move(-1, 0);
                }

                if (Input.GetKey("right"))
                {
                    activeTetromino.Move(1, 0);
                }

                if (rotate)
                {
                    activeTetromino.Rotate();
                    rotate = false;
                }

                if (Input.GetKey("down") || beat % 4 == 0)
                {
                    if (!activeTetromino.Move(0, -1))
                    {
                        state = TetrisState.blockHasLanded;
                    }
                }
                break;

            case TetrisState.blockHasLanded:
                activeTetromino = null;
                rowCounter += gameGrid.CheckForFullRow();
                if (rowCounter > ROWS_FOR_SPEED_UP)
                {
                    rowCounter -= ROWS_FOR_SPEED_UP;
                    tetrisSpeed++;
                    GamePulse.beatSpeed = tetrisStartSpeed + tetrisSpeed;
                    canvas.setSpeedText(tetrisSpeed);
                }
                state = TetrisState.newBlock;
                break;

            case TetrisState.gameOver:
                break;
        }
	}
}
