using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Shows end score, and adds hi-score if attained.
public class EndScoreShower : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        // Set text to score retrieved from player preferences.
        int score = PlayerPrefs.GetInt("Score");
        scoreText.text = "You Scored: " + score;
        // If some score has been achieved
        if (score > 0)
        {
            ScoreCheck(score);
        }
    }

    // Method to compare score achieved in current game to each hi-score, replacing the hi-score and lower hi-scores if necessary
    private void ScoreCheck(int score)
    {
        if (!PlayerPrefs.HasKey("HiScores"))
        {
            PlayerPrefs.SetInt("HiScores", 1);
            PlayerPrefs.SetInt("HiScore1", score);
            PlayerPrefs.SetInt("HiScore2", 0);
            PlayerPrefs.SetInt("HiScore3", 0);
            PlayerPrefs.SetInt("HiScore4", 0);
            PlayerPrefs.SetInt("HiScore5", 0);
            return;
        }

        if (score > PlayerPrefs.GetInt("HiScore1"))
        {
            PlayerPrefs.SetInt("HiScore5", PlayerPrefs.GetInt("HiScore4"));
            PlayerPrefs.SetInt("HiScore4", PlayerPrefs.GetInt("HiScore3"));
            PlayerPrefs.SetInt("HiScore3", PlayerPrefs.GetInt("HiScore2"));
            PlayerPrefs.SetInt("HiScore2", PlayerPrefs.GetInt("HiScore1"));

            PlayerPrefs.SetInt("HiScore1", score);
        }
        else if (score > PlayerPrefs.GetInt("HiScore2"))
        {
            PlayerPrefs.SetInt("HiScore5", PlayerPrefs.GetInt("HiScore4"));
            PlayerPrefs.SetInt("HiScore4", PlayerPrefs.GetInt("HiScore3"));
            PlayerPrefs.SetInt("HiScore3", PlayerPrefs.GetInt("HiScore2"));

            PlayerPrefs.SetInt("HiScore2", score);
        }
        else if (score > PlayerPrefs.GetInt("HiScore3"))
        {
            PlayerPrefs.SetInt("HiScore5", PlayerPrefs.GetInt("HiScore4"));
            PlayerPrefs.SetInt("HiScore4", PlayerPrefs.GetInt("HiScore3"));

            PlayerPrefs.SetInt("HiScore3", score);
        }
        else if (score > PlayerPrefs.GetInt("HiScore4"))
        {
            PlayerPrefs.SetInt("HiScore5", PlayerPrefs.GetInt("HiScore4"));

            PlayerPrefs.SetInt("HiScore4", score);
        }
        else if (score > PlayerPrefs.GetInt("HiScore5"))
        {
            PlayerPrefs.SetInt("HiScore5", score);
        }
        else
        {
            return;
        }
    }
    
}
