using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public TMP_Text timerText;
    private float time;
    private bool isRunning = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Avoid duplicates
    }

    void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            timerText.text = "Stopwatch: " + time.ToString("F2") + "s";
        }
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            time = 0f;
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Timer stopped at: " + time.ToString("F5") + " seconds");
    }
}