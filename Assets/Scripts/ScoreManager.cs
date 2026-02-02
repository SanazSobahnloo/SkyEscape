using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
   public static ScoreManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    float aliveTime;
    bool isRunning;

    const string HighScoreKey = "HIGH_SCORE_SECONDS";

    public int score;
    private float timer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        aliveTime = 0f;
        isRunning = true;

        float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);
        UpdateUI(0f, highScore);
    }

    void Update()
{
    timer += Time.deltaTime;
    score = Mathf.FloorToInt(timer);
    scoreText.text = score.ToString();
}

    public void StopAndSaveIfHighScore()
    {
        if (!isRunning) return;
        isRunning = false;

        float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);

        if (aliveTime > highScore)
        {
            PlayerPrefs.SetFloat(HighScoreKey, aliveTime);
            PlayerPrefs.Save();
            highScore = aliveTime;
        }

        UpdateUI(aliveTime, highScore);
    }

    void UpdateUI(float score, float highScore)
    {
        if (scoreText)
            scoreText.text = $"Score: {score:0.0}s";

        if (highScoreText)
            highScoreText.text = $"High Score: {highScore:0.0}s";
    }

}
