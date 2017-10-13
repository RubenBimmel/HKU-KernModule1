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
    public bool AddBlock (int blockXPos, int blockYPos)
    {
        return AddBlock(blockXPos, blockYPos, Color.white);
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

    // Gets called to create a block that can be added to the grid
    private Block ConstructBlock (int blockXPos, int blockYPos, Color colour)
    {
        Block newBlock = (Block)Instantiate(blockPrefab, transform.position, transform.rotation);
        newBlock.transform.parent = transform;
        newBlock.transform.localPosition = new Vector3(blockXPos, blockYPos);
        newBlock.setColour(colour);
        return newBlock;
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
