using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls 'Outside_Ambience' GameObject, ensures it plays in menu.
public class AudioSourceController : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;

    Scene activeScene;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        source.clip = clip;

        // Get scene for scene check
        activeScene = SceneManager.GetActiveScene();

        // Start playing if in menu scene
        if (activeScene.name == "Menu")
            source.Play();
    }
}
