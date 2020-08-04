using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures camera tracks orca - controls 'Main Camera'
public class CameraController : MonoBehaviour
{
    // Holds player position.
    private Transform playerTransform;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        // Retrieve initial reference to player object transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Updates camera every frame, ensuring it is always at the player's position
    void Update()
    {
        transform.position = playerTransform.position;
    }
}
