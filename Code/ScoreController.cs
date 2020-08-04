using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controls `Score' GameObject, ensures score is shown
public class ScoreController : MonoBehaviour {

    // Variables to store score and final multiplier
    public static int score;
    public static float end_multiplier;
    // Stores text component this is attached to
    TextMeshProUGUI scoreText;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start () {
        score = 0;
        // Set to reference component the script is attached to
        scoreText = GetComponent<TextMeshProUGUI>();
        // Initially make score display "Score: 0"
        scoreText.text = "Score: " + score;

        end_multiplier = PlayerPrefs.GetFloat("end_multiplier");
    }

    // Method called from TrickTrackController when score is to be increased
    public void IncreaseScore(float increase, float multiplier) {
        // Increase mutiplied by amount based on difficulty
        increase *= end_multiplier;
        //Score updated by necessary amount multiplied by multiplier
        score += (int) Math.Round(increase * multiplier);
        // Score stored in playerprefs for highscore tracking
        PlayerPrefs.SetInt("Score", score);
        // Text of scoreText updated to reflect updated score
        scoreText.text = "Score: " + score;
    }
}
