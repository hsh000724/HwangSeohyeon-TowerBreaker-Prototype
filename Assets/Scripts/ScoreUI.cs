using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    int currentScore = 0;

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "SCORE : " + currentScore;
    }
}