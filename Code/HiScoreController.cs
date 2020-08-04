using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controls the `HiScores' GameObject, ensuring hi-scores are shown.
public class HiScoreController : MonoBehaviour
{
    TextMeshProUGUI hiscoreText;

    // Start contains code initializing the script variables, and is called before the first frame.
    void Start()
    {
        hiscoreText = GetComponent<TextMeshProUGUI>();
        WriteScore();
    }

    // Retrieves HiScores from preferences and writes them to object text, or writes zeroes if no scores yet added.
    private void WriteScore()
    {
        hiscoreText.text = "Hi Scores: ";
        if (PlayerPrefs.HasKey("HiScores"))
        {
            hiscoreText.text += PlayerPrefs.GetInt("HiScore1") + "\n";
            hiscoreText.text += PlayerPrefs.GetInt("HiScore2") + "\n";
            hiscoreText.text += PlayerPrefs.GetInt("HiScore3") + "\n";
            hiscoreText.text += PlayerPrefs.GetInt("HiScore4") + "\n";
            hiscoreText.text += PlayerPrefs.GetInt("HiScore5");
        }
        else
        {
            hiscoreText.text += "\n 0 \n 0 \n 0 \n 0 \n 0";
        }
    }

}
