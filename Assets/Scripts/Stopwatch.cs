using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TMP_Text display;
    public TMP_Text modeText;
    public Button startButton;
    public Button stopButton;
    public Button resetButton;
    public Button toggleModeButton;
    public TMP_InputField inputSeconds;
    public Button applyButton;

    private bool running = false;
    private bool stopwatchMode = true;
    private float timeValue = 0f; // Time in seconds
    public float startSeconds = 60f;

    void Start()
    {
        startButton.onClick.AddListener(OnStart);
        stopButton.onClick.AddListener(OnStopPause);
        resetButton.onClick.AddListener(OnReset);
        toggleModeButton.onClick.AddListener(OnToggleMode);
        applyButton.onClick.AddListener(ApplyInputSeconds);

        ResetToMode();
        UpdateDisplay();
    }

    void Update()
    {
        if (!running) return;

        if (stopwatchMode)
        {
            timeValue += Time.deltaTime;
        }
        else
        {
            timeValue -= Time.deltaTime;
            if (timeValue <= 0f)
            {
                timeValue = 0f;
                running = false;
            }
        }

        UpdateDisplay();
    }

    // Updates the displayed time (hours:minutes:seconds or minutes:seconds.milliseconds)
    void UpdateDisplay()
    {
        int hrs = (int)(timeValue / 3600);
        int mins = (int)(timeValue % 3600 / 60);
        int secs = (int)(timeValue % 60);
        int ms = (int)((timeValue - Mathf.Floor(timeValue)) * 1000);

        if (hrs > 0)
            display.text = string.Format("{0:00}:{1:00}:{2:00}", hrs, mins, secs);
        else
            display.text = string.Format("{0:00}:{1:00}.{2:000}", mins, secs, ms);
    }

    public void OnStart()
    {
        running = true;
    }

    public void OnStopPause()
    {
        running = false;
    }

    public void OnReset()
    {
        ResetToMode();
        UpdateDisplay();
    }

    public void OnToggleMode()
    {
        stopwatchMode = !stopwatchMode;
        modeText.text = stopwatchMode ? "Stopwatch" : "Timer";
        inputSeconds.gameObject.SetActive(stopwatchMode ? false : true);
        applyButton.gameObject.SetActive(stopwatchMode ? false : true);
        ResetToMode();
        UpdateDisplay();
    }

    public void SetTime(float seconds)
    {
        timeValue = Mathf.Max(0f, seconds);
        UpdateDisplay();
    }

    public void SetStartSeconds(float seconds)
    {
        startSeconds = Mathf.Max(0f, seconds);
    }

    private void ApplyInputSeconds()
    {
        if (float.TryParse(inputSeconds.text, out float seconds))
        {
            SetTime(seconds);
            SetStartSeconds(seconds);
            running = false;
        }
        else
        {
            Debug.LogWarning("Invalid input, please enter a number.");
        }
    }
    void ResetToMode()
    {
        running = false;
        timeValue = stopwatchMode ? 0f : Mathf.Max(0f, startSeconds);
    }
}