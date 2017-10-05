using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SynchronisedBehaviour {

    public GameGrid grid;
    private Tetrimino activeTetrimino;

    // Use this for initialization
    void Start()
    {
        if (!grid)
        {
            Debug.LogError("Grid is missing");
        }
    }

    // Gets called every beat
    public override void OnBeat (int beat) {
        if (activeTetrimino == null)
        {
            activeTetrimino = new Tetrimino(grid);
        }
        else
        {
            if (Input.GetKey("escape"))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKey("left"))
            {
                activeTetrimino.Move(-1, 0);
            }

            if (Input.GetKey("right"))
            {
                activeTetrimino.Move(1, 0);
            }

            if (Input.GetKey("up"))
            {
                activeTetrimino.Rotate();
            }

            if (Input.GetKey("down") || beat % 4 == 0)
            {
                if (!activeTetrimino.Move(0, -1))
                {
                    activeTetrimino = null;
                    grid.CheckForFullRow();
                }
            }
        }
	}
}
