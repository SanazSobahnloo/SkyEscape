using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Coin, Bomb }
    public ItemType type;

    public int value = 5;
    public float lifeTime = 6f;

    [Header("UI Feedback")]
    public GameObject scorePopupPrefab;   // Prefab متن +10 / -10

    private ScoreModifier scoreModifier;
    private float dieAt;

    private void Awake()
    {
        dieAt = Time.time + lifeTime;

        // از صحنه پیدا می‌کنه
        scoreModifier = FindFirstObjectByType<ScoreModifier>();
    }

    private void Update()
    {
        if (Time.time >= dieAt)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("plane")) return;

        int delta = (type == ItemType.Coin) ? value : -value;

        if (scoreModifier != null)
            scoreModifier.AddBonus(delta);

        ShowScorePopup(delta);

        Destroy(gameObject);
    }

    private void ShowScorePopup(int delta)
{
    if (scorePopupPrefab == null) return;

    Canvas canvas = FindFirstObjectByType<Canvas>();
    if (canvas == null) return;

    GameObject obj = Instantiate(scorePopupPrefab, canvas.transform);

    ScorePopup popup = obj.GetComponent<ScorePopup>();
    if (popup != null)
        popup.SetDelta(delta);
}


    private Transform FindAnyCanvasTransform()
    {
        // بهترین حالت: Canvas فعال داخل صحنه
        Canvas c = FindFirstObjectByType<Canvas>();
        if (c != null) return c.transform;

        // اگر Canvas پیدا نشد
        return null;
    }
}
