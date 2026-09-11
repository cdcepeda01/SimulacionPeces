using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishTankSimulation : MonoBehaviour
{
    [Header("Simulacion")]
    public int initialFish = 50;
    public int actualFish;

    public float reproductionRate = 0.10f;
    public int fishCaughtPerDay = 2;


    [Header("Tiempo")]
    public float secondsPerDay = 1f;

    private int day = 0;
    private float timer = 0f;


    [Header("Visual")]
    public GameObject fishPrefab;
    public SpriteRenderer tankArea;
    public TMP_Text statusText;

    private List<GameObject> fishObjects = new List<GameObject>();


    private int fishBornToday = 0;
    private int fishCaughtToday = 0;


    void Start()
    {
        actualFish = initialFish;
        day = 0;
        timer = 0f;

        DrawView();

        Debug.Log("Dia " + day + ": " + actualFish + " peces");
    }


    void Update()
    {
        if (actualFish <= 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= secondsPerDay)
        {
            timer = 0f;
            Simulate();
        }
    }


    void Simulate()
    {
        day++;

        fishBornToday = Mathf.RoundToInt(actualFish * reproductionRate);
        actualFish += fishBornToday;

        fishCaughtToday = Mathf.Min(fishCaughtPerDay, actualFish);
        actualFish -= fishCaughtToday;

        if (actualFish < 0)
        {
            actualFish = 0;
        }

        DrawView();

        Debug.Log(
            "Dia " + day +
            " | Nacen: " + fishBornToday +
            " | Se pescan: " + fishCaughtToday +
            " | Peces restantes: " + actualFish
        );

        if (actualFish <= 0)
        {
            Debug.Log("No quedan peces en el tanque.");
        }
    }


    void DrawView()
    {
        ClearView();

        Bounds bounds = tankArea.bounds;

        for (int i = 0; i < actualFish; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            Vector3 randomPosition = new Vector3(x, y, 0f);

            GameObject fish = Instantiate(
                fishPrefab,
                randomPosition,
                Quaternion.identity
            );

            SpriteRenderer sr = fish.GetComponent<SpriteRenderer>();

            if (sr != null && Random.value > 0.5f)
            {
                sr.flipX = true;
            }

            fishObjects.Add(fish);
        }

        if (statusText != null)
        {
            statusText.text =
                "Dia: " + day +
                "\nPeces: " + actualFish +
                "\nCrecimiento: 10%" +
                "\nPesca diaria: 2";
        }
    }


    void ClearView()
    {
        foreach (GameObject fish in fishObjects)
        {
            Destroy(fish);
        }

        fishObjects.Clear();
    }
}