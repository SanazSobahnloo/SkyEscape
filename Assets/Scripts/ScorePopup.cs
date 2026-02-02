using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("Animation")]
    public float lifeTime = 1.0f;
    public float moveUpSpeed = 40f;

    private TMP_Text tmp;
    private Color startColor;
    private float t;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        startColor = tmp != null ? tmp.color : Color.white;
    }

    private void Update()
    {
        t += Time.deltaTime;

        // حرکت به بالا
        transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);

        // Fade out
        if (tmp != null)
        {
            float a = Mathf.Lerp(1f, 0f, t / lifeTime);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, a);
        }

        if (t >= lifeTime)
            Destroy(gameObject);
    }

    public void SetDelta(int delta)
    {
        if (tmp == null) return;

        if (delta >= 0)
        {
            tmp.text = "+" + delta;
            tmp.color = Color.green;
        }
        else
        {
            tmp.text = delta.ToString(); // خودش - دارد
            tmp.color = Color.red;
        }

        startColor = tmp.color;
    }
}
