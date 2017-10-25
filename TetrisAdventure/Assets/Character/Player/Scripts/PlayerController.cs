using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterModel))]

public class PlayerController : SynchronisedBehaviour, ICharacterController {

    private new Collider2D collider;
    private CharacterModel model;

    public GameObject characterSprites;
    public Block blockPrefab;
    public GameManager manager;
    public GameGrid grid;
    private int[] gridPosition;

    // Use this for initialization
    void Start () {
        collider = transform.GetComponent<Collider2D>();
        model = transform.GetComponent<CharacterModel>();

        if (!grid) {
            Debug.LogError("Grid is missing");
        }

        if (!manager) {
            Debug.LogError("Game manager is missing");
        }

        if (!characterSprites) {
            Debug.LogError("Character sprite is missing");
        }

        if (!blockPrefab) {
            Debug.LogError("Block Prefab is missing");
        }
    }

    void LateUpdate() {
        // Check if character needs to be placed on the grid
        if (Input.GetButton("CharacterCrouch")) {
            if (gridPosition == null) {
                gridPosition = grid.GetLocalPosition(transform.position);
                grid.AddBlock(gridPosition[0], gridPosition[1], Color.white, blockPrefab);
                CheckIfCharacterFillsRow();

                collider.enabled = false;
                characterSprites.SetActive(false);
            }
        }
        else {
        // When character is moving normally
            if (gridPosition == null) {
                model.SetHorizontalMovement(Input.GetAxisRaw("CharacterHorizontal"));

                if (Input.GetButtonDown("CharacterJump")) {
                    model.Jump();
                }
            }
            else {
            //Character is still on grid and needs to be placed in the world 
                grid.ClearCell(gridPosition[0], gridPosition[1]);

                collider.enabled = true;
                characterSprites.SetActive(true);

                transform.position = grid.GetWorldPosition(gridPosition[0], gridPosition[1]);
                model.ResetVelocity();

                gridPosition = null;
            }
        }
    }

    // OnBeat is used for movement when the player is on the grid (crouching)
    public override void OnBeat(int beat) {
        if (gridPosition != null) {
            if (beat % 2 == 0) {
                gridPosition = grid.MoveBlock(gridPosition, (int)Input.GetAxisRaw("CharacterHorizontal"), 0);
            }

            if (beat % 4 == 0) {
                int[] newPositions = grid.MoveBlock(gridPosition, 0, -1);
                if (gridPosition == newPositions) {
                    CheckIfCharacterFillsRow();
                }
                else {
                    gridPosition = newPositions;
                }
            }
        }
    }

    // Gets called when the player can't go lower on the grid (same as Tetromino behaviour)
    private void CheckIfCharacterFillsRow () {
        int rowsDeleted = grid.CheckForFullRow();
        if (rowsDeleted > 0) {
            manager.GameIsOver();
        }
    }

    // Gets called when a tetromino collides with a character after moving
    public void HitByBlock() {
        manager.GameIsOver();
    }

    // Gets called when attacked by enemy
    public void HitByEnemy() {
        manager.GameIsOver();
    }

    // Gets called when enemy is killed by player
    public void KilledEnemy() {
        model.Bounce();
    }
    
}
