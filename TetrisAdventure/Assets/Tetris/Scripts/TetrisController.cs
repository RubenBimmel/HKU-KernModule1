using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisController : SynchronisedBehaviour {

    public GameManager manager;

    public enum TetrisState { start, newBlock, blockIsMoving, blockHasLanded, gameOver}
    private TetrisState state;

    public GameGrid gameGrid;
    public Grid previewGrid;
    private Tetromino activeTetromino;
    private Tetromino nextTetromino;

    private int rowCounter;
    private const int ROWS_FOR_SPEED_UP = 5;
    private readonly int[] POINTS_FOR_ROWS = new int[] {0, 50, 150, 250, 350 };

    private int tetrominoCounter;
    private const int TETROMINOS_FOR_NEXT_COIN = 7;
    private bool addSlowDown;

    private bool rotate;
    private int horizontalMovement;

    // Use this for initialization
    void Start()
    {
        if (!gameGrid || !previewGrid)
        {
            Debug.LogError("Grid is missing");
        }

        if (!manager)
        {
            Debug.LogError("Game manager is missing");
        }

        state = TetrisState.start;
        rowCounter = 0;
    }

    // Update is only used to check if player has given input inbetween beats
    void Update()
    {
        if (Input.GetButtonDown("TetrisUp"))     rotate = true;
        if (Input.GetButtonDown("TetrisLeft"))   horizontalMovement--;
        if (Input.GetButtonDown("TetrisRight"))  horizontalMovement++;
    }

    // Gets called every beat
    public override void OnBeat (int beat) {

        switch (state)
        {
            case TetrisState.start:
                previewGrid.ClearGrid();
                gameGrid.ClearGrid();

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

                    manager.OnNewTetromino(activeTetromino, tetrominoCounter, gameGrid);
                    tetrominoCounter++;

                    if (GameManager.type == GameManager.GameType.Adventure)
                    {
                        if (tetrominoCounter % TETROMINOS_FOR_NEXT_COIN == 0)
                        {
                            addCoinOnRandomCell();
                        }

                        if (addSlowDown)
                        {
                            SlowDown newSD = Instantiate<SlowDown>(manager.slowDownPrefab, new Vector3(100, 100, 1), Quaternion.identity);
                            newSD.SetManager(manager);
                            nextTetromino.AddCollectable(newSD);
                        }
                    }
                }
                else
                {
                    manager.GameIsOver();
                }

                break;

            case TetrisState.blockIsMoving:
                if ((Input.GetButton("TetrisLeft") || horizontalMovement < 0) && beat % 2 == 0)
                {
                    horizontalMovement = 0;
                    activeTetromino.Move(-1, 0);
                }

                if ((Input.GetButton("TetrisRight") || horizontalMovement > 0) && beat % 2 == 0)
                {
                    horizontalMovement = 0;
                    activeTetromino.Move(1, 0);
                }

                if (rotate)
                {
                    activeTetromino.Rotate();
                    rotate = false;
                }

                if (Input.GetButton("TetrisDown") || beat % 4 == 0)
                {
                    if (!activeTetromino.Move(0, -1))
                    {
                        state = TetrisState.blockHasLanded;
                    }
                }

                break;

            case TetrisState.blockHasLanded:
                activeTetromino = null;

                int rowsDeleted = gameGrid.CheckForFullRow();
                manager.addScore(POINTS_FOR_ROWS[rowsDeleted]);

                addSlowDown = rowsDeleted >= 3;

                rowCounter += rowsDeleted;
                if (rowCounter > ROWS_FOR_SPEED_UP)
                {
                    rowCounter -= ROWS_FOR_SPEED_UP;
                    manager.addSpeed(1);
                }
                state = TetrisState.newBlock;
                break;

            case TetrisState.gameOver:
                break;
        }
    }

    private void addCoinOnRandomCell()
    {
        int column = Random.Range(0, gameGrid.width);
        int row = gameGrid.getHeighestAvailableCellInColumn(column, activeTetromino.blockPositions);
        if (row < gameGrid.height)
        {
            // Coin hovers on a random cell, 2 to 4 blocks above the block
            row = Mathf.Min(row + Random.Range(2, 4), gameGrid.height - 1);

            Vector2 pos = gameGrid.GetWorldPosition(column, row);
            manager.AddCoin(pos.x, pos.y);
        }
    }
}
