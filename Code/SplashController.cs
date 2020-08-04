using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to `Splash' GameObjects, with methods called from `PlayerController' for control of those objects
public class SplashController : MonoBehaviour
{
    // Variables for current timer and maximum
    public int timer;
    public int maxTimer;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        timer = 0;
        maxTimer = 30;
    }

    // Ensures Splash GameObject is deactivated (and no longer visisble) a certain number of frames after activation - uses timer to track frames
    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer == 0)
                this.gameObject.SetActive(false);
        }
    }
    // Setter for position of splash object - set from PlayerController based on player position
    public void SetPos(float inX)
    {
        transform.position = new Vector2(inX, 20.5f);
    }

    // Activates GameObject and set timer to maximum
    public void Active()
    {
        this.gameObject.SetActive(true);
        timer = maxTimer;
    }
}
