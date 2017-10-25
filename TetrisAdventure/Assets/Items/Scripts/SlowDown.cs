using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : Collectable {

    // Gets called when item is picked up
    protected override void ApplyItem() {
        if (manager) {
            int speed = manager.getSpeed();
            if (speed > 0) {
                // speed is lowered by 1 and 1 for each 5 levels
                speed--;
                speed -= Mathf.FloorToInt(speed / 5);
            }
            manager.setSpeedOverTime(speed, 2);
        }
        else {
            Debug.LogWarning("SlowDown has no manager");
        }
    }
}
