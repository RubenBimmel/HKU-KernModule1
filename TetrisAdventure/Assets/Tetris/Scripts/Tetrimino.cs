using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino {

    private Grid grid;
    private enum TetriminoShape { I, J, L, O, S, T, Z };
    private TetriminoShape shape;
    private int[,] blockPositions;

    // Constructor
    public Tetrimino (Grid _grid)
    {
        grid = _grid;

        shape = (TetriminoShape)Random.Range(0, 6);
        blockPositions = GetBlockPositions();

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

        if (shape == TetriminoShape.I)
        {
            //QQQ
        }

        int[,] targetPositions = (int[,])blockPositions.Clone();
        int[,] relativePosition = new int[4, 2];

        for (int i = 1; i < 4; i++)
        {
            relativePosition[i, 0] = targetPositions[i, 0] - targetPositions[0, 0];
            relativePosition[i, 1] = targetPositions[i, 1] - targetPositions[0, 1];

            targetPositions[i, 0] = targetPositions[0, 0] + relativePosition[i, 1];
            targetPositions[i, 1] = targetPositions[0, 1] - relativePosition[i, 0];
        }

        bool tetriminoCanMove = grid.TransformTetrimino(blockPositions, targetPositions);

        if (tetriminoCanMove)
        {
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
                positions = new int[,] { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetriminoShape.L:
                positions = new int[,] { { 0, 0 }, { -1, -1 }, { -1, 0 }, { 1, 0 } };
                break;
            case TetriminoShape.O:
                positions = new int[,] { { 0, 0 }, { 0, -1 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetriminoShape.S:
                positions = new int[,] { { 0, 0 }, { -1, -1 }, { 0, -1 }, { 1, 0 } };
                break;
            case TetriminoShape.T:
                positions = new int[,] { { 0, 0 }, { -1, 0 }, { 0, -1 }, { 1, 0 } };
                break;
            case TetriminoShape.Z:
                positions = new int[,] { { 0, 0 }, { -1, 0 }, { 0, -1 }, { 1, -1 } };
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            positions[i, 0] += grid.spawnPosition[0];
            positions[i, 1] += grid.spawnPosition[1];
        }

        return positions;
    }
}
