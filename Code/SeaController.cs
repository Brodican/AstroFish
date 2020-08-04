using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Position changing script attached to Sea GameObjects
public class SeaController : MonoBehaviour
{
    // Variables used to track the position of the sea objects, and the amount they need to be shifted by.
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
        yPos = 0;
        removalConst = 39.3f;
        replaceConst = 78.6f;
    }

    // Update position of sea blocks each physics frame.
    // Ensures empty background is not seen - sea blocks are moved off-camera before that occurs.
    void FixedUpdate()
    {
        // Directly get camera position every frame
        cameraPos = Camera.main.transform.position.x;
        // If object too far behind camera position, set it ahead of all other objects
        if ((cameraPos - removalConst) >= positionTrack)
        {
            positionTrack += replaceConst;
            transform.position = new Vector2(positionTrack, transform.position.y);
        }

        // If object too far ahead camera position, set it behind of all other objects
        if ((cameraPos + removalConst) <= positionTrack)
        {
            positionTrack -= replaceConst;
            transform.position = new Vector2(positionTrack, transform.position.y);
        }
    }
}
