using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterModel))]

public class EnemyController : MonoBehaviour, ICharacterController {

    private GameManager manager;

    private CharacterModel model;
    private float horizontalMovement;

    private const float KILL_AREA = .22f;

    public float spawnTime;

    public bool killableByPlayer;
    public bool killableByBlock;
    public bool canJump;

    private void Awake () {
        spawnTime = Time.time;
    }

    // Use this for initialization
    private void Start () {
        model = transform.GetComponent<CharacterModel>();
        horizontalMovement = model.characterSpeed;
    }

    // Gets called in sync with physics
    private void LateUpdate () {
        model.SetHorizontalMovement(horizontalMovement);
    }
	
	// Enemy behaviour always happen on collision with a wall or other character
    protected virtual void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            if (killableByPlayer && Mathf.Abs(transform.position.x - other.transform.position.x) <= KILL_AREA && other.transform.position.y - transform.position.y > KILL_AREA) {
                other.transform.GetComponent<PlayerController>().KilledEnemy();
                DestroyEnemy();
            }
            else if (Time.time - spawnTime > 2f) {
                other.transform.GetComponent<PlayerController>().HitByEnemy();
            }
        }

        else if (transform.position.y - other.transform.position.y < model.distanceToFeet) {
            if (canJump && !Physics2D.OverlapPoint(transform.position + new Vector3(.32f * horizontalMovement, .64f)) && !Physics2D.OverlapPoint(transform.position + new Vector3(0, .32f))) {
                model.Jump();
            }
            else {
                horizontalMovement = -horizontalMovement;
            }
        }
    }

    // Gets called by the manager that creates it
    public void SetManager (GameManager newManager) {
        manager = newManager;
    }

    // Gets called when a tetromino collides with a character after moving
    public void HitByBlock () {
        if (killableByBlock) {
            DestroyEnemy();
        }
    }

    // Used to destroy this enemy
    public void DestroyEnemy () {
        if (manager) {
            manager.AddCoin(transform.position.x, transform.position.y);
        }
        Destroy(gameObject);
    }
}
