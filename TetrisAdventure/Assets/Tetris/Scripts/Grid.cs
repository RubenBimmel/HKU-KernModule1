using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public const int width = 10;
    public const int height = 20;
    public readonly int[] spawnPosition = new int[] { (int)(.5 * Grid.width), Grid.height - 1 };
    private const float cellSize = 1f;
    private Block [,] cells;

    public Block blockPrefab;

    // Use this for initialization
    void Start ()
    {
        cells = new Block [width, height];

        if (!blockPrefab)
        {
            Debug.LogError("Block Prefab is missing");
        }
	}

    // Use this function to prevent overriding a block
    public bool AddBlock (int blockXPos, int blockYPos)
    {
        if (CellIsAvailable(blockXPos, blockYPos))
        {
            cells[blockXPos, blockYPos] = ConstructBlock(blockXPos, blockYPos);
            return true;
        }
        return false;
    }

    // Gets called to create a block that can be added to the grid
    private Block ConstructBlock (int blockXPos, int blockYPos)
    {
        Block newBlock = (Block)Instantiate(blockPrefab, transform.position, transform.rotation);
        newBlock.transform.parent = transform;
        newBlock.transform.localPosition = new Vector3(blockXPos, blockYPos);
        return newBlock;
    }

    // Use this function to keep the grid clean and matching with transform positions
    public void MoveBlock (int blockXPos, int blockYPos, int targetXPos, int targetYPos)
    {
        if (CellIsAvailable(targetXPos, targetYPos))
        {
            //Update transform position
            cells[blockXPos, blockYPos].transform.localPosition = new Vector3(targetXPos, targetYPos);

            //Update grid
            cells[targetXPos, targetYPos] = cells[blockXPos, blockYPos];
            cells[blockXPos, blockYPos] = null;
        }
        else
        {
            Debug.LogError("Trying to move a block to a ocupied cell");
        }
    }

    // Gets called to move, rotate or change a tetrimino. Returns if transformation was possible.
    public bool TransformTetrimino(int[,] blockPositions, int[,] targetPositions)
    {
        if (TetriminoCanMove(blockPositions, targetPositions))
        {
            Block[] tetriminoBlocks = new Block[4];
            for (int i = 0; i < 4; i++)
            {
                tetriminoBlocks[i] = cells[blockPositions[i, 0], blockPositions[i, 1]];

                if (!tetriminoBlocks[i])
                {
                    Debug.LogError(string.Concat("Tetrimino block missing in cell [ ",blockPositions[i,0],", ", blockPositions[i, 1]," ]"));
                    return false;
                }

                cells[blockPositions[i, 0], blockPositions[i, 1]] = null;
            }

            for (int i = 0; i < 4; i++)
            {
                tetriminoBlocks[i].transform.localPosition = new Vector3(targetPositions[i, 0], targetPositions[i, 1]);
                cells[targetPositions[i, 0], targetPositions[i, 1]] = tetriminoBlocks[i];
            }

            return true;
        }
        return false;
    }

    // Gets called when a Tetrinimo's lifetime has ended
    public void CheckForFullRow()
    {
        // Rows need to be checked in reverse order. Else it will skip the one that has been lowered after removing a row.
        for (int row = height -1; row >= 0; row--)
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
                for (int i = 0; i < width; i++)
                {
                    cells[i, row].Destroy();
                    MoveBlocksDownRecursively(i, row);
                }
            }
        }
    }

    // Gets called when a row gets destroyed
    private void MoveBlocksDownRecursively (int blockXPos, int blockYPos)
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

    // Check if cell exists and is empty
    public bool CellIsAvailable(int blockXPos, int blockYPos)
    {
        if (blockXPos >= 0 && blockXPos < width && blockYPos >= 0 && blockYPos < height)
        {
            return !cells[blockXPos, blockYPos];
        }
        return false;
    }

    // Check if multiple blocks can move
    private bool TetriminoCanMove(int[,] blockPositions, int[,] targetPositions)
    {
        bool tetriminoCanMove = true;

        for (int i = 0; i < 4; i++)
        {
            // Check if target is already occupied by tetrimino
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
                tetriminoCanMove = false;
            }
        }

        return tetriminoCanMove;
    }
}
