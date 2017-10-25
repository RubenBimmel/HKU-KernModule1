using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterModel : MonoBehaviour {

    private new Rigidbody2D rigidbody;

    public float characterSpeed = 2;
    private const float CHARACTER_ACCELERATION = 10;
    private const float CHARACTER_JUMP_FORCE = 3.4f;
    public float distanceToFeet = .16f;

    void Awake()
    {
        if (GameManager.type == GameManager.GameType.Classic)
        {
            gameObject.SetActive(false);
        }

        rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    public void SetHorizontalMovement (float movement)
    {
        float horizontalVelocity = Mathf.MoveTowards(rigidbody.velocity.x, movement * characterSpeed, Time.deltaTime * CHARACTER_ACCELERATION);
        rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
    }

    public void ResetVelocity ()
    {
        rigidbody.velocity = Vector2.zero;
    }

    public void Jump ()
    {
        if (IsGrounded())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, CHARACTER_JUMP_FORCE);
        }
    }

    public void Bounce ()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, CHARACTER_JUMP_FORCE);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.TransformPoint(Vector3.down * distanceToFeet), 0.05f, 1);
    }
}
