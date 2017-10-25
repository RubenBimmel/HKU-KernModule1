using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Block blockPrefab;
    public int width;
    public int height;
    private const float cellSize = 1f;
    protected Block [,] cells;

    [HideInInspector]
    public int[] spawnPosition;

    // Use this for initialization
    void Start ()
    {
        cells = new Block [width, height];
        spawnPosition = new int[] { (int)(.5 * width), height - 1 };

        if (!blockPrefab)
        {
            Debug.LogError("Block Prefab is missing");
        }
	}

    // Use this function to prevent overriding a block
    public bool AddBlock(int blockXPos, int blockYPos, Color colour)
    {
        if (CellIsAvailable(blockXPos, blockYPos))
        {
            cells[blockXPos, blockYPos] = ConstructBlock(blockXPos, blockYPos, colour);
            return true;
        }
        return false;
    }

    // Use this function to prevent overriding a block
    public bool AddBlock(int blockXPos, int blockYPos)
    {
        return AddBlock(blockXPos, blockYPos, Color.white);
    }

    // Get referance to block
    public Block GetBlock (int blockXPos, int blockYPos)
    {
        return cells[blockXPos, blockYPos];
    }

    // Gets called to create a block that can be added to the grid
    private Block ConstructBlock (int blockXPos, int blockYPos, Color colour)
    {
        Block newBlock = Instantiate<Block>(blockPrefab, transform.position, transform.rotation);
        newBlock.transform.parent = transform;
        newBlock.transform.localPosition = new Vector3(blockXPos, blockYPos);
        newBlock.SetColour(colour);
        newBlock.SetGrid(this);
        return newBlock;
    }

    // Used by non grid objects to get the world space position of a block
    public Vector2 GetWorldPosition(int blockXPos, int blockYPos)
    {
        return transform.TransformPoint(new Vector2(blockXPos, blockYPos));
    }

    public int[] GetLocalPosition(Vector2 WorldPos)
    {
        Vector2 localPos = transform.InverseTransformPoint(WorldPos);
        int x = Mathf.RoundToInt(localPos.x);
        int y = Mathf.RoundToInt(localPos.y);
        return new int[] { x, y };
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

    // Get heighest block in a row (used to calculate spawn positions for coins etc.)
    public int getHeighestAvailableCellInColumn(int column, int[,] maskPositions)
    {
        for (int row = height - 1; row >= 0; row--)
        {
            if (!CellIsAvailable(column, row))
            {
                bool masked = false;
                for (int i = 0; i < 4; i++ )
                {
                    if (column == maskPositions[i,0] && row == maskPositions[i,1])
                    {
                        masked = true;
                        Debug.Log("InMask");
                    }
                }
                if (!masked)
                {
                    return row + 1;
                }
            }
        }
        return 0;
    }

    // Empty mask version
    public int getHeighestAvailableCellInColumn(int column)
    {
        return getHeighestAvailableCellInColumn(column, new int[,] { { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 } });
    }

    // Clear a Cell
    public void ClearCell(int blockXPos, int blockYPos)
    {
        if (!CellIsAvailable(blockXPos, blockYPos))
        {
            cells[blockXPos, blockYPos].Destroy();
            cells[blockXPos, blockYPos] = null;
        }
        else
        {
            Debug.LogWarning("Trying to clear a cell that is already empty");
        }
    }

    // Clears the Grid
    public void ClearGrid()
    {
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                if (cells[i, j])
                {
                    cells[i, j].Destroy();
                    cells[i, j] = null;
                }
            }
        }
    }
}
