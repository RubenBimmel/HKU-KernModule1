using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterView : MonoBehaviour {

    private Animator animator;
    private float direction;

	// Use this for initialization
	void Awake () {
        animator = transform.GetComponent<Animator>();
        direction = 1;
	}
	
    // Used to update the animations
	public void UpdateAnimatior (float horizontalVelocity, bool grounded) {
        if (grounded) {
            if (horizontalVelocity < 0) direction = -1;
            if (horizontalVelocity > 0) direction = 1;

            transform.localScale = new Vector3(direction, 1, 1);

            if (horizontalVelocity == 0) {
                animator.CrossFade("Idle", 0);
            }
            else {
                animator.CrossFade("Walk", 0);
            }
        }
        else {
            animator.CrossFade("Jump", 0);
        }
    }

    // Used when a character needs to be invisible for a while
    public void SetActive (bool active) {
        transform.GetComponent<SpriteRenderer>().enabled = active;
    }
}
