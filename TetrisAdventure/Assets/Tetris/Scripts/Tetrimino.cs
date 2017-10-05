using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino {

    private GameGrid grid;
    private enum TetriminoShape { I, J, L, O, S, T, Z };
    private TetriminoShape shape;
    private int[,] blockPositions;
    private int rotation;

    // Constructor
    public Tetrimino (GameGrid _grid)
    {
        grid = _grid;

        shape = (TetriminoShape)Random.Range(0, 6);
        blockPositions = GetBlockPositions();
        rotation = 0;

        for (int i = 0; i < 4; i++)
        {
            grid.AddBlock(blockPositions[i, 0], blockPositions[i, 1]);
        }
    }

    // Public move function
    public bool Move (int xOffset, int yOffset)
    {
        int[,] targetPositions = (int[,])blockPositions.Clone();
        for (int i = 0; i < 4; i++)
        {
            targetPositions[i, 0] += xOffset;
            targetPositions[i, 1] += yOffset;
        }

        bool tetriminoCanMove = grid.TransformTetrimino(blockPositions, targetPositions);

        if (tetriminoCanMove)
        {
            blockPositions = targetPositions;
        }

        return tetriminoCanMove;
    }

    // Public rotate function
    public bool Rotate ()
    {
        if (shape == TetriminoShape.O)
        {
            return true;
        }

        int[,] targetPositions = new int[4, 2];
        int[,] relativePosition = new int[4, 2];

        int rotationBlock = GetRotationBlock();

        for (int i = 0; i < 4; i++)
        {
            relativePosition[i, 0] = blockPositions[i, 0] - blockPositions[rotationBlock, 0];
            relativePosition[i, 1] = blockPositions[i, 1] - blockPositions[rotationBlock, 1];

            targetPositions[i, 0] = blockPositions[rotationBlock, 0] + relativePosition[i, 1];
            targetPositions[i, 1] = blockPositions[rotationBlock, 1] - relativePosition[i, 0];
        }

        bool tetriminoCanMove = grid.TransformTetrimino(blockPositions, targetPositions);

        if (tetriminoCanMove)
        {
            rotation = ++rotation % 4;
            blockPositions = targetPositions;
        }

        return tetriminoCanMove;

    }

    // Gets called on initialisation.
    // First block is anchor for block rotations (I and O use other rotations)
    private int[,] GetBlockPositions()
    {
        int[,] positions = new int[,] { };

        switch (shape)
        {
            case TetriminoShape.I:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } };
                break;
            case TetriminoShape.J:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetriminoShape.L:
                positions = new int[,] { { -1, -1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                break;
            case TetriminoShape.O:
                positions = new int[,] { { 0, 0 }, { 0, -1 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetriminoShape.S:
                positions = new int[,] { { -1, -1 }, { 0, -1 }, { 0, 0 }, { 1, 0 } };
                break;
            case TetriminoShape.T:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 0, -1 }, { 1, 0 } };
                break;
            case TetriminoShape.Z:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 0, -1 }, { 1, -1 } };
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            positions[i, 0] += grid.spawnPosition[0];
            positions[i, 1] += grid.spawnPosition[1];
        }

        return positions;
    }

    private int GetRotationBlock()
    {
        switch (shape)
        {
            case TetriminoShape.I:
                return 1;
            case TetriminoShape.J:
                return 1;
            case TetriminoShape.L:
                return 2;
            case TetriminoShape.S:
                return 2;
            case TetriminoShape.T:
                return 1;
            case TetriminoShape.Z:
                return 1;
        }

        return 0;
    }
}
