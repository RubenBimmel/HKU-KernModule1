using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterModel))]

public class EnemyController : MonoBehaviour, ICharacterController {

    private CharacterModel model;
    private float horizontalMovement;

	// Use this for initialization
	void Start () {
        model = transform.GetComponent<CharacterModel>();
        horizontalMovement = model.characterSpeed;
    }

    void LateUpdate ()
    {
        model.SetHorizontalMovement(horizontalMovement);
    }
	
	// Enemy behaviour always happen on collision with a wall or other character
    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (transform.position.y - other.transform.position.y < model.distanceToFeet)
        {
            if (Physics2D.OverlapPoint(transform.position + new Vector3(.32f * horizontalMovement, .64f)))
            {
                horizontalMovement = -horizontalMovement;
            }
            else
            {
                model.Jump();
            }

        }
    }

    // Gets called when a tetromino collides with a character after moving
    public void HitByBlock ()
    {
        Destroy(gameObject);
    }
}
