using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : Grid {

    // Adds a block using world space coordinates, used by the character.
    public int[] AddBlock(Vector2 WorldPos)
    {
        int[] localPos = GetLocalPosition(WorldPos);

        if (AddBlock(localPos[0], localPos[1]))
        {
            return localPos;
        }
        return null;
    }

    // Use this function to keep the grid clean and matching with transform positions
    private void MoveBlock(int blockXPos, int blockYPos, int targetXPos, int targetYPos)
    {
        if (CellIsAvailable(targetXPos, targetYPos))
        {
            //Update transform position
            cells[blockXPos, blockYPos].transform.localPosition = new Vector3(targetXPos, targetYPos);

            //Update grid
            cells[targetXPos, targetYPos] = cells[blockXPos, blockYPos];
            cells[blockXPos, blockYPos] = null;

            //Update character collision
            cells[targetXPos, targetYPos].CheckCharacterCollision();
        }
        else
        {
            Debug.LogError("Trying to move a block to an ocupied cell");
        }
    }

    // Public move function used by non tetris mechanics that can adjust the grid
    // This function returns the blocks positions to make sure it wont lose track of the object.
    public int[] MoveBlock(int[] blockPos, int xOffset, int yOffset)
    {
        if (CellIsAvailable(blockPos[0], blockPos[1]))
        {
            Debug.LogWarning("Trying to move an empty cell");
            return blockPos;
        }

        int[] targetPos = new int[] { blockPos[0] + xOffset, blockPos[1] + yOffset };
        if (CellIsAvailable(targetPos[0], targetPos[1]))
        {
            MoveBlock(blockPos[0], blockPos[1], targetPos[0], targetPos[1]);
            return targetPos;
        }
        return blockPos;
    }

    // Gets called to move, rotate or change a tetromino. Returns if transformation was possible.
    public bool TransformTetromino(int[,] blockPositions, int[,] targetPositions)
    {
        if (TetrominoCanMove(blockPositions, targetPositions))
        {
            Block[] tetrominoBlocks = new Block[4];
            for (int i = 0; i < 4; i++)
            {
                tetrominoBlocks[i] = cells[blockPositions[i, 0], blockPositions[i, 1]];

                if (!tetrominoBlocks[i])
                {
                    Debug.LogError(string.Concat("Tetromino block missing in cell [ ", blockPositions[i, 0], ", ", blockPositions[i, 1], " ]"));
                    return false;
                }

                cells[blockPositions[i, 0], blockPositions[i, 1]] = null;
            }

            for (int i = 0; i < 4; i++)
            {
                // Update transform positions
                tetrominoBlocks[i].transform.localPosition = new Vector3(targetPositions[i, 0], targetPositions[i, 1]);

                // Update grid
                cells[targetPositions[i, 0], targetPositions[i, 1]] = tetrominoBlocks[i];

                //Update character collision
                cells[targetPositions[i, 0], targetPositions[i, 1]].CheckCharacterCollision();
            }

            return true;
        }
        return false;
    }

    // Gets called only when a Tetrinimo's lifetime has ended. Else rows will get destroyed while a tetromino is still falling.
    // Returns the amount of rows that are deleted
    public int CheckForFullRow()
    {
        int fullRows = 0;
        // Rows need to be checked in reverse order. Else it will skip the one that has been lowered after removing a row.
        for (int row = height - 1; row >= 0; row--)
        {
            bool rowIsFull = true;
            for (int i = 0; i < width; i++)
            {
                if (!cells[i, row])
                {
                    rowIsFull = false;
                }
            }

            if (rowIsFull)
            {
                fullRows++;

                for (int i = 0; i < width; i++)
                {
                    cells[i, row].Destroy();
                    MoveBlocksDownRecursively(i, row);
                }
            }
        }
        return fullRows;
    }

    // Gets called when a row gets destroyed
    private void MoveBlocksDownRecursively(int blockXPos, int blockYPos)
    {
        cells[blockXPos, blockYPos] = null;

        if (blockYPos < height - 1)
        {
            if (cells[blockXPos, blockYPos + 1])
            {
                MoveBlock(blockXPos, blockYPos + 1, blockXPos, blockYPos);
            }
            MoveBlocksDownRecursively(blockXPos, blockYPos + 1);
        }
    }

    // Check if multiple blocks can move
    private bool TetrominoCanMove(int[,] blockPositions, int[,] targetPositions)
    {
        bool tetrominoCanMove = true;

        for (int i = 0; i < 4; i++)
        {
            // Check if target is already occupied by tetromino
            bool moveToSelf = false;
            for (int j = 0; j < 4; j++)
            {
                if (targetPositions[i, 0] == blockPositions[j, 0] && targetPositions[i, 1] == blockPositions[j, 1])
                {
                    moveToSelf = true;
                }
            }

            // Else check if cell is available
            if (!moveToSelf && !CellIsAvailable(targetPositions[i, 0], targetPositions[i, 1]))
            {
                tetrominoCanMove = false;
            }
        }

        return tetrominoCanMove;
    }
}
