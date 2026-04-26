using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CreditsScroller : MonoBehaviour {
    public float scrollSpeed = 50f;
    public RectTransform panel; 
    private RectTransform textRect;
    private float startY;
    private float endY;
    private bool rolling = false;
    public float startDelay = 1f;

    private void Awake() {
        textRect = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        StartCoroutine(StartRoll());
    }

    private IEnumerator StartRoll() {
        if (panel == null || textRect == null) yield break;

        // Make sure pivot is at bottom center
        textRect.pivot = new Vector2(0.5f, 0f);

        // Start just below panel
        startY = -textRect.rect.height; // Fully below panel
        textRect.anchoredPosition = new Vector2(0, startY);

        // End just above panel
        endY = panel.rect.height;

        // Wait before scrolling (text visible immediately)
        yield return new WaitForSeconds(startDelay);

        rolling = true;
    }

    private void Update() {
        if (!rolling || textRect == null) return;

        // Scroll text upward
        textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Check if finished
        if (textRect.anchoredPosition.y >= endY) {
            rolling = false;

            // Disable parent Canvas after credits end
            if (textRect.parent != null)
                textRect.parent.gameObject.SetActive(false);
        }
    }
}
