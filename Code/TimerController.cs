using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the `Timer' GameObject, ensuring remaining time is counted correctly.
public class TimerController : MonoBehaviour
{
    // Variables to get reference to playerController
    GameObject player;
    PlayerController playerController;

    bool countDown;
    int timerEndTimer;
    public float remainingTime;
    public int roundedTime;
    TextMeshProUGUI timer;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        countDown = false;
        timerEndTimer = 130;
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        remainingTime = 60;
        roundedTime = 60;
        timer = GetComponent<TextMeshProUGUI>();
        timer.text = roundedTime + " seconds";
    }

    // Update timer each physics frame - this is independent of framerate, so timer will track time correctly even if framerate drops.
    void FixedUpdate()
    {
        // countDown true once timer ended
        if (!countDown)
        {
            TimeReduction();
        }
        // Once timer ended, end game after set time
        else
        {
            timerEndTimer--;
            if (timerEndTimer == 0)
            {
                SceneManager.LoadScene("End");
            }
        }
    }

    // Method to reduce timer according to time passed each physics frame.
    void TimeReduction()
    {
        // If there is time remaining reduce remaining time according to time passed from deltaTime constant
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        // Round time so whole number shown in timer text
        roundedTime = Mathf.RoundToInt(remainingTime);
        // If rounded below zero, set to zero as negative time would be incorrect
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        // Add timer to object text
        timer.text = roundedTime + " seconds";
        // Start countdown to stop game if time done
        if ((remainingTime == 0) && (!playerController.isAbove))
        {
            countDown = true;
            playerController.StopCounting();
        }
    }
}
