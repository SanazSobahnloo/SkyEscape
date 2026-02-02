using UnityEngine;
using TMPro;

public class ScoreModifier : MonoBehaviour
{
    [Header("References")]
    public ScoreManager scoreManager;   // اگر خالی باشد خودش از Instance می‌گیرد

    [Header("UI Override (optional)")]
    public TMP_Text scoreText;          // اگر خالی باشد از ScoreManager می‌گیرد
    public TMP_Text highScoreText;      // اگر خالی باشد از ScoreManager می‌گیرد

    [Header("Bonus/Penalty")]
    public int bonusScore = 0;          // سکه/بمب اینو تغییر میدن

    // همون کلید پروژه تو
    private const string HighScoreKey = "HIGH_SCORE_SECONDS";

    private int highScoreInt;

    private void Awake()
    {
        if (scoreManager == null)
            scoreManager = ScoreManager.Instance;

        if (scoreManager != null)
        {
            if (scoreText == null) scoreText = scoreManager.scoreText;
            if (highScoreText == null) highScoreText = scoreManager.highScoreText;
        }

        // های‌اسکور قبلی (که float بوده) رو می‌گیریم و int می‌کنیم
        float hsFloat = PlayerPrefs.GetFloat(HighScoreKey, 0f);
        highScoreInt = Mathf.FloorToInt(hsFloat);
    }

    // LateUpdate تا بعد از ScoreManager.Update متن رو overwrite کنیم
    private void LateUpdate()
    {
        if (scoreManager == null) return;

        int finalScore = scoreManager.score + bonusScore;
        if (finalScore < 0) finalScore = 0;

        // نمایش بدون اعشار
        if (scoreText != null)
            scoreText.text = $"Score: {finalScore}";

        // های‌اسکور بدون اعشار (ولی تو همون PlayerPrefs float ذخیره می‌کنیم که با سیستم قبلی سازگار بمونه)
        if (finalScore > highScoreInt)
        {
            highScoreInt = finalScore;
            PlayerPrefs.SetFloat(HighScoreKey, highScoreInt);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScoreInt}";
    }

    public void AddBonus(int amount)
    {
        bonusScore += amount;
    }
}
