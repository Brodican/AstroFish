using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script with methods to call when buttons pressed.
public class ButtonController : MonoBehaviour
{
    // Loads the game scene.
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    // Opens Youtube game tutorial
    public void YouTube_Tutorial()
    {
        Application.OpenURL("https://youtu.be/DPkt2BWnOIM");
    }

    // Loads the menu scene.
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Ends game.
    public void Quit()
    {
        Application.Quit();
    }

    // Sets difficulty.
    public void SetDifficulty(int dif)
    {
        // Set difficulty to key in PlayerPrefs, ensuring it is stored between game sessions
        PlayerPrefs.SetInt("Difficulty", dif);
        float mult = 0;
        // Set final score multiplier higher for higher difficulties
        switch (dif)
        {
            case 1: mult = 0.5f;
                break;
            case 2: mult = 1;
                break;
            case 3: mult = 1.5f;
                break;
            case 4: mult = 2;
                break;
        }
        // Set multiplier to key in PlayerPrefs, ensuring it is stored between game sessions
        PlayerPrefs.SetFloat("end_multiplier", mult);
    }
}
