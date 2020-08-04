using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

// Controls the `ScoreDetails' GameObject
public class TrickTrackController : MonoBehaviour
{
    // List which will store each trick and the details of its count/score
    private List<ScoreElement> scoreElements;
    // Holds link to GameObject (canvas component) this script is attached to
    TextMeshProUGUI scoreDetails;

    GameObject score;
    ScoreController scoreController;

    public static float multiplier;

    // Start contains code initializing the script variables, and is called before the first frame
    void Start()
    {
        scoreDetails = GetComponent<TextMeshProUGUI>();
        scoreElements = new List<ScoreElement>();

        score = GameObject.Find("Score");
        scoreController = score.GetComponent<ScoreController>();

        scoreDetails.text = "";
        multiplier = 1;
    }

    // When a score is added, ensure the necessary information is added to the text of `ScoreDetails'
    public void AddElement(string element, int inScoreConst)
    {
        scoreDetails.text = "";

        // If trick already added, increment its count - otherwise add new trick
        if (scoreElements.Exists(e => e.GetElement() == element))
        {
            (scoreElements.Find(x => x.GetElement() == element)).Increment();
        } 
        else
        {
            ScoreElement newElement = new ScoreElement(element, inScoreConst);
            scoreElements.Add(newElement);
        }

        // Add information regarding each trick to text
        for (int i = 0; i < scoreElements.Count; i++)
        {
            if (i == 0) 
            {
                scoreDetails.text = scoreDetails.text + " (" + scoreElements[i].GetElement() + "*" + scoreElements[i].GetCount() + " (" + scoreElements[i].GetScore() + ")";
            } else
                scoreDetails.text = scoreDetails.text + " + " + scoreElements[i].GetElement() + "*" + scoreElements[i].GetCount() + " (" + scoreElements[i].GetScore() + ")";

            if (i == scoreElements.Count - 1)
            {
                scoreDetails.text += ") * " + multiplier;
            }
        }
        // Increase multiplier - ensures trick combos rewarded
        multiplier += .02f;
        // This rounding needs to occur as error can occur with float addition
        multiplier = (float) Math.Round(multiplier, 2);
    }

    // Check if score element has been added
    public bool Exists ()
    {
        return scoreElements.Count > 0;
    }

    // Resets the multiplier.
    public void ResetMult()
    {
        multiplier = 1;
    }

    // Adds all list element score to the total score, then resets the list
    public void ResetListSmooth()
    {
        float scoreAdded = 0;
        for (int i = 0; i < scoreElements.Count; i++)
        {
            scoreAdded += scoreElements[i].GetScore();
        }
        scoreController.IncreaseScore(scoreAdded, multiplier);
        scoreElements = new List<ScoreElement>();
        scoreDetails.text = "";
        multiplier = 1;
    }

    // Resets list without adding list elements to score
    public void ResetListSplash()
    {
        scoreElements = new List<ScoreElement>();
        scoreDetails.text = "";
    }

    // Class to represent 1 trick: includes information on count of this trick, score for 1 trick, and total score
    private class ScoreElement
    {
        // Variables storing information for one trick
        private readonly string element;
        private int count;
        private int score;
        private readonly int scoreConst;

        // Constructor to initialize variables
        public ScoreElement(string inElement, int inScoreConst)
        {
            element = inElement;
            count = 1;
            scoreConst = inScoreConst;
            score = scoreConst;
        }

        // Increment count of this trick, increasing total score
        public void Increment()
        {
            count++;
            score = count * scoreConst;
        }

        // Other methods are getters
        public string GetElement()
        {
            return element;
        }

        public int GetCount()
        {
            return count;
        }

        public int GetScore()
        {
            return score;
        }
    }
}
