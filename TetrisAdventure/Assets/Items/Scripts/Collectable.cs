using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    private bool active;
    private const float LIFETIME = 8;
    private float endOfLife;
    protected GameManager manager;

    // Gets called once each frame
    private void Update() {
        if (active && Time.time > endOfLife) {
            Destroy(gameObject);
        }
    }

    // OnTrigger gets called when a rigidbody collides with item. Anything with physics interacts with items
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            ApplyItem();
        }
        Destroy(gameObject);
    }

    // Activate item. Once active the timer starts running
    public void SetActive(bool newState) {
        active = newState;
        if (active) {
            endOfLife = Time.time + LIFETIME;
        }
    }

    // Set manager for interaction with gameplay
    public void SetManager(GameManager newManager) {
        manager = newManager;
    }

    // Gets called when an item is picked up
    protected virtual void ApplyItem() { }
    
}
