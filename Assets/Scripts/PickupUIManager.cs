using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUIManager : MonoBehaviour {
    public static PickupUIManager Instance;

    [SerializeField] private TMP_Text pickupText;
    [SerializeField] private float displayTime = 2f;

    private Coroutine currentRoutine;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        pickupText.gameObject.SetActive(false);
    }

    public void ShowPickup(string message) {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message) {
        pickupText.text = message;
        pickupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        pickupText.gameObject.SetActive(false);
        currentRoutine = null;
    }
}
