using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino {

    private Grid grid;
    private enum TetrominoShape { I, J, L, O, S, T, Z };
    private TetrominoShape shape;
    private Color colour;
    private int[,] blockPositions;
    private int rotation;

    // Constructor
    public Tetromino ()
    {
        shape = (TetrominoShape)Random.Range(0, 6);
        
        switch (shape)
        {
            case TetrominoShape.I:
                colour = Color.red;
                break;
            case TetrominoShape.J:
                colour = Color.blue;
                break;
            case TetrominoShape.L:
                colour = new Color(1, .25f, 0);
                break;
            case TetrominoShape.O:
                colour = Color.yellow;
                break;
            case TetrominoShape.S:
                colour = Color.magenta;
                break;
            case TetrominoShape.T:
                colour = Color.cyan;
                break;
            case TetrominoShape.Z:
                colour = Color.green;
                break;
        }

        rotation = 0;
    }

    public bool AddToGrid (Grid _grid)
    {
        grid = _grid;
        blockPositions = GetBlockPositions();

        for (int i = 0; i < 4; i++)
        {
            bool added = grid.AddBlock(blockPositions[i, 0], blockPositions[i, 1], colour);
            if (!added)
            {
                return false;
            }
        }

        return true;
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

        bool tetrominoCanMove = ((GameGrid) grid).TransformTetromino(blockPositions, targetPositions);

        if (tetrominoCanMove)
        {
            blockPositions = targetPositions;
        }

        return tetrominoCanMove;
    }

    // Public rotate function
    public bool Rotate ()
    {
        //O can not rotate
        if (shape == TetrominoShape.O)
        {
            return true;
        }

        //calculate new positions for rotated tetromino
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

        //I gets repositioned on rotation
        if (shape == TetrominoShape.I)
        {
            if (rotation % 2 == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    targetPositions[i, 0]--;
                }
            }
        }

        bool tetrominoCanMove = ((GameGrid)grid).TransformTetromino(blockPositions, targetPositions);

        if (tetrominoCanMove)
        {
            rotation = ++rotation % 4;
            blockPositions = targetPositions;
        }

        return tetrominoCanMove;

    }

    // Gets called on initialisation.
    // First block is anchor for block rotations (I and O use other rotations)
    private int[,] GetBlockPositions()
    {
        int[,] positions = new int[,] { };

        switch (shape)
        {
            case TetrominoShape.I:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } };
                break;
            case TetrominoShape.J:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetrominoShape.L:
                positions = new int[,] { { -1, -1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                break;
            case TetrominoShape.O:
                positions = new int[,] { { 0, 0 }, { 0, -1 }, { 1, 0 }, { 1, -1 } };
                break;
            case TetrominoShape.S:
                positions = new int[,] { { -1, -1 }, { 0, -1 }, { 0, 0 }, { 1, 0 } };
                break;
            case TetrominoShape.T:
                positions = new int[,] { { -1, 0 }, { 0, 0 }, { 0, -1 }, { 1, 0 } };
                break;
            case TetrominoShape.Z:
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

    //Get anchor for tetromino rotation
    private int GetRotationBlock()
    {
        switch (shape)
        {
            case TetrominoShape.I:
                if (rotation < 2) return 2;
                return 1;
            case TetrominoShape.J:
                return 1;
            case TetrominoShape.L:
                return 2;
            case TetrominoShape.S:
                if (rotation < 2) return 1;
                return 2;
            case TetrominoShape.T:
                return 1;
            case TetrominoShape.Z:
                if (rotation < 2) return 1;
                return 2;
        }

        return 0;
    }
}
