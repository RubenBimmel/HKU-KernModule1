using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    private bool active;
    private const float LIFETIME = 8;
    private float endOfLife;
    protected GameManager manager;

    public void SetActive(bool newState)
    {
        active = newState;
        if (active)
        {
            endOfLife = Time.time + LIFETIME;
        }
    }

    void Update()
    {
        if (active && Time.time > endOfLife)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ApplyItem();
        Destroy(gameObject);
    }

    protected virtual void ApplyItem() { }

    public void SetManager(GameManager newManager)
    {
        manager = newManager;
    }
}
