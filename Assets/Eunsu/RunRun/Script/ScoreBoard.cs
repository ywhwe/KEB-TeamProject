using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static ScoreBoard scoreInstance;
    public int score;

    private void Awake()
    {
        scoreInstance = this;
        scoreText.text = 0.ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public void SetScore(int number)
    {
        score += number;
    }
}
