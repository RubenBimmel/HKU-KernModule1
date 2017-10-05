using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public const int width = 10;
    public const int height = 20;
    public readonly int[] spawnPosition = new int[] { (int)(.5 * Grid.width), Grid.height - 1 };
    private const float cellSize = 1f;
    protected Block [,] cells;

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

    // Check if cell exists and is empty
    public bool CellIsAvailable(int blockXPos, int blockYPos)
    {
        if (blockXPos >= 0 && blockXPos < width && blockYPos >= 0 && blockYPos < height)
        {
            return !cells[blockXPos, blockYPos];
        }
        return false;
    }
}
