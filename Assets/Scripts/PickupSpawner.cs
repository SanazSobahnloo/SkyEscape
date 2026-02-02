using System.Collections;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bombPrefab;

    public GameObject coinGoldPrefab;   // 10
    public GameObject coinSilverPrefab; // 8
    public GameObject coinBronzePrefab; // 6

    [Header("Spawn Timing")]
    public float spawnEveryMin = 2f;
    public float spawnEveryMax = 4f;

    [Header("Chances")]
    [Range(0f, 1f)] public float coinChance = 0.7f;  // احتمال اینکه اصلاً coin بیاد
    [Range(0f, 1f)] public float goldChance = 0.2f;  // بین سکه‌ها
    [Range(0f, 1f)] public float silverChance = 0.3f; // بین سکه‌ها
    // برنز = باقی‌مونده

    [Header("Spawn Area")]
    public Camera cam;
    public float padding = 0.5f;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(spawnEveryMin, spawnEveryMax);
            yield return new WaitForSeconds(wait);

            SpawnOne();
        }
    }

    void SpawnOne()
    {
        Vector3 pos = GetRandomPositionInView();

        bool spawnCoin = Random.value < coinChance;

        GameObject prefabToSpawn;

        if (spawnCoin)
        {
            prefabToSpawn = PickCoinPrefab();
        }
        else
        {
            prefabToSpawn = bombPrefab;
        }

        if (prefabToSpawn != null)
            Instantiate(prefabToSpawn, pos, Quaternion.identity);
    }

    GameObject PickCoinPrefab()
    {
        float r = Random.value;

        // نرمال‌سازی ساده در صورت اینکه جمع احتمال‌ها 1 نشه
        float g = Mathf.Clamp01(goldChance);
        float s = Mathf.Clamp01(silverChance);
        float b = Mathf.Clamp01(1f - (g + s));

        float total = g + s + b;
        if (total <= 0.0001f) return coinBronzePrefab;

        g /= total;
        s /= total;
        // b هم implicitly

        if (r < g) return coinGoldPrefab;
        if (r < g + s) return coinSilverPrefab;
        return coinBronzePrefab;
    }

    Vector3 GetRandomPositionInView()
    {
        // محدوده‌ی دوربین
        float z = Mathf.Abs(cam.transform.position.z);
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, z));

        float x = Random.Range(bottomLeft.x + padding, topRight.x - padding);
        float y = Random.Range(bottomLeft.y + padding, topRight.y - padding);

        return new Vector3(x, y, 0f);
    }
}
