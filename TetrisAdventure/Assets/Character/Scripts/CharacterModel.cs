using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterModel : MonoBehaviour {

    private new Rigidbody2D rigidbody;
    private CharacterView view;

    public float characterSpeed = 2;
    private const float CHARACTER_ACCELERATION = 10;
    private const float CHARACTER_JUMP_FORCE = 3.4f;
    public float distanceToFeet = .16f;

    // Initialisation
    void Awake() {
        if (GameManager.type == GameManager.GameType.Classic) {
            gameObject.SetActive(false);
        }

        rigidbody = transform.GetComponent<Rigidbody2D>();

        view = GetComponentInChildren<CharacterView>();
    }

    // Used to set horizontal movement
    public void SetHorizontalMovement (float movement) {
        float horizontalVelocity = Mathf.MoveTowards(rigidbody.velocity.x, movement * characterSpeed, Time.deltaTime * CHARACTER_ACCELERATION);
        rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        if (view) {
            view.UpdateAnimatior(horizontalVelocity, IsGrounded());
        }
    }

    // Used when a character needs to be respawned without velocity
    public void ResetVelocity () {
        rigidbody.velocity = Vector2.zero;
    }

    // Gets called to jump
    public void Jump () {
        if (IsGrounded()) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, CHARACTER_JUMP_FORCE);
        }
    }

    // Gets called when a player bounces up in air
    public void Bounce () {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, CHARACTER_JUMP_FORCE);
    }

    // Check if player is on ground
    private bool IsGrounded() {
        return Physics2D.OverlapCircle(transform.TransformPoint(Vector3.down * distanceToFeet), 0.05f, 1);
    }
}
