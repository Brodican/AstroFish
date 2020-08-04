using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to 'Tail' child GameObject of 'Player', ensures the orca sprite moves according to player input and flips are added as necessary.
public class TailController : MonoBehaviour
{
    // Variable to store player gameobject
    GameObject player;
    // Playercontroller variable used to access playerController script
    PlayerController playerController;

    // Variable to access ScoreDetails gameobject
    GameObject scoreDetails;
    // Access TrickTrackController script
    TrickTrackController trickTrackController;

    // Variables for current rotation degrees and the change in degrees from the point where the player
    // exited the water
    public float degrees;
    public int degreeChange;
    public int degInc;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        // Retrieve and store the player gameobject
        player = GameObject.Find("Player");
        // Retrieve script attached to player GameObject
        playerController = player.GetComponent<PlayerController>();
        // Retrieve and store the ScoreDetails gameobject
        scoreDetails = GameObject.Find("ScoreDetails");
        // Retrieve script attached to ScoreDetails
        trickTrackController = scoreDetails.GetComponent<TrickTrackController>();
        degInc = 6;
    }

    // Update is called once per frame
    void Update()
    {
        // Set degrees to current degrees of player (from PlayerController) - ensures the sprite matches the orca's rotation
        degrees = playerController.degrees;
        // Get rotation suitable for transform passing degrees 
        transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
    }

    // Called when left turn occurs, tracks degree change and ensures flips are added to trick list.
    public void LeftTurn()
    {
        if (playerController.isAbove)
        {
            degreeChange -= degInc;
        }
        else
        {
            degreeChange = 0;
        }

        if (degreeChange == -360)
        {
            degreeChange = 0;
            trickTrackController.AddElement("Backflip!", 5000);
        }

    }

    // Called when right turn occurs, tracks degree change and ensures flips are added to trick list.
    public void RightTurn()
    {
        if (playerController.isAbove)
        {
            degreeChange += degInc;
        }
        else
        {
            degreeChange = 0;
        }

        if (degreeChange == 360)
        {
            degreeChange = 0;
            trickTrackController.AddElement("Frontflip!", 5000);
        }

    }

}
