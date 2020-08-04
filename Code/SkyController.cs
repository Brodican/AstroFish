using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Position changing script attached to Sky GameObjects.
public class SkyController : MonoBehaviour
{
    // Variables for position tracking and replacement of sky blocks.
    public float positionTrack;
    public float cameraPos;
    public float yPos;
    private float removalConst;
    private float replaceConst;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        // Initialize previously declared variables.
        positionTrack = transform.position.x;
        cameraPos = Camera.main.transform.position.x;
        yPos = 395;
        removalConst = 52.5f;
        replaceConst = 2 * removalConst;
    }

    // Update position of sky blocks each physics frame.
    // Ensures empty background is not seen - sky blocks are moved off-camera before that occurs.
    void FixedUpdate()
    {
        // Directly get camera position every frame
        cameraPos = Camera.main.transform.position.x;
        // If object too far behind camera position, set it ahead of all other objects
        if ((cameraPos - removalConst) >= positionTrack) 
        {
            positionTrack += replaceConst;
            transform.position = new Vector2(positionTrack, yPos);
        }

        // If object too far ahead camera position, set it behind of all other objects
        if ((cameraPos + removalConst) <= positionTrack)
        {
            positionTrack -= replaceConst;
            transform.position = new Vector2(positionTrack, yPos);
        }
    }
}
