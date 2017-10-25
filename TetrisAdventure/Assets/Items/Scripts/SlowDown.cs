using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : Collectable {

    protected override void ApplyItem()
    {
        if (manager)
        {
            int speed = manager.getSpeed();
            if (speed > 0)
            {
                speed--;
                speed -= Mathf.FloorToInt(speed / 5);
            }
            manager.setSpeedOverTime(speed, 2);
        }
        else
        {
            Debug.LogWarning("SlowDown has no manager");
        }
    }
}
