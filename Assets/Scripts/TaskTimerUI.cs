using UnityEngine;
using TMPro;
using System.Collections;

public class TaskTimerUI : MonoBehaviour {
    public static TaskTimerUI Instance;

    [Header("UI Elements")]
    public TMP_Text timerText;             // Assign TaskTimerText
    public GameObject taskNotification;    // Optional: Task Completed notification

    private float timeRemaining;
    private bool timerActive = false;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update() {
        if (!timerActive) return;

        if (timeRemaining > 0f) {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else {
            timeRemaining = 0f;
            timerActive = false;
            GameOver();
        }
    }

    private void UpdateTimerDisplay() {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer(float seconds) {
        timeRemaining = seconds;
        timerActive = true;
        UpdateTimerDisplay();
    }

    public void StopTimer() {
        timerActive = false;
    }

    public void ShowTaskNotification(string message, float duration = 2f) {
        if (taskNotification != null) {
            taskNotification.SetActive(true);
            TMP_Text text = taskNotification.GetComponent<TMP_Text>();
            if (text != null) text.text = message;
            StartCoroutine(HideNotificationAfter(duration));
        }
    }

    private IEnumerator HideNotificationAfter(float duration) {
        yield return new WaitForSeconds(duration);
        if (taskNotification != null)
            taskNotification.SetActive(false);
    }

    private void GameOver() {
        Debug.Log("Game Over: Time ran out!");
        // Call your GameOver UI or Scene reload here
    }
}
