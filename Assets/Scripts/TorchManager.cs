using UnityEngine;
using System.Collections;

public class TorchManager : MonoBehaviour {
    public static TorchManager Instance;

    [Header("Torch Light Object")]
    public Light torchLight;

    public bool HasTorch { get; private set; } = false;
    private bool isTorchOn = false;

    [Header("Flicker Settings")]
    public float flickerStartDelay = 10f;
    public float flickerChance = 0.05f;
    public float flickerInterval = 1.5f;
    public float flickerDurationMin = 0.05f;
    public float flickerDurationMax = 0.25f;

    private float flickerTimer = 0f;
    private float nextFlickerCheckTime;

    private void Awake() {
        if (Instance == null) Instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        if (torchLight == null)
            Debug.LogError("TorchManager: torchLight not assigned!");
        else
            torchLight.enabled = false;
    }

    private void Update() {
        if (!HasTorch) return;

        HandleToggleInput();
        HandleFlicker();
    }

    public void GrantTorch(bool autoOn) {
        HasTorch = true;
        isTorchOn = autoOn;
        if (torchLight != null) torchLight.enabled = autoOn;
    }


    private void HandleToggleInput() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleTorch();
        }
    }

    private void ToggleTorch() {
        isTorchOn = !isTorchOn;

        if (torchLight != null)
            torchLight.enabled = isTorchOn;

        Debug.Log("Torch: " + (isTorchOn ? "ON" : "OFF"));
    }

    // Updated HandleFlicker
    private void HandleFlicker() {
        if (!isTorchOn || torchLight == null) return;

        flickerTimer += Time.deltaTime;
        if (flickerTimer < flickerStartDelay) return;

        if (Time.time >= nextFlickerCheckTime) {
            nextFlickerCheckTime = Time.time + flickerInterval;

            if (Random.value < flickerChance)
                StartCoroutine(FlickerRoutine());
        }
    }

    // Updated FlickerRoutine
    private IEnumerator FlickerRoutine() {
        if (torchLight == null) yield break;

        float originalIntensity = torchLight.intensity;

        // Flicker duration
        float duration = Random.Range(flickerDurationMin, flickerDurationMax);
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            // Randomly vary intensity to simulate flame flicker
            torchLight.intensity = originalIntensity * Random.Range(0.7f, 1.2f);

            yield return null;
        }

        // Occasionally do a quick blackout for realism
        if (Random.value < 0.1f) {
            torchLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            torchLight.enabled = true;
        }

        // Reset intensity
        torchLight.intensity = originalIntensity;
    }

}
