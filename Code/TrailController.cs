using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script limiting length of trail component of `Trail' GameObject
public class TrailController : MonoBehaviour
{
    // Variables to hold references to GameObjects and components
    GameObject player;
    PlayerController playerController;
    TrailRenderer trailRenderer;

    // Hold time the trail should show for
    public float currentTime;
    public float timeConst;
    public float maxTime;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        // Used to adjust time trail stays rendered for
        timeConst = 10;
    }

    // Makes trail shorter, so it doesn't appear at the default speed
    void Update()
    {
        currentTime = (playerController.speed - timeConst) / 20;
        trailRenderer.time = currentTime;
    }

}
