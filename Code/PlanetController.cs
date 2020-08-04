using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls planet position, attached to each planet object
public class PlanetController : MonoBehaviour
{
    // Hold reference to player GameObject
    GameObject player;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        // Initializes player variable with reference to player GameObject
        player = GameObject.Find("Player");
    }

    // Update x position to player x position with each physics update
    private void FixedUpdate()
    {
        transform.position = new Vector2(player.transform.position.x - 5, transform.position.y);
    }
}
