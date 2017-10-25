using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable {

    private const int POINTS = 500;

    protected override void ApplyItem()
    {
        if(manager)
        {
            manager.addScore(POINTS);
        }
        else
        {
            Debug.LogWarning("Coin has no manager");
        }
    }
}
