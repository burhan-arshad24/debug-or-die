using TMPro;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class GhostJumpscare {
    public string name;
    public GameObject ghostObject;
    public Animator animator;
    public string animationState = "Idle";

    [Header("Movement")]
    public Transform[] waypoints;
    public float moveSpeed = 8f;

    [Header("Timing")]
    public float duration = 1.5f;

    [Header("Jumpscare Text")]
    [TextArea]
    public string jumpscareText;
    [HideInInspector] public bool played = false;
}



public class JumpscareManager : MonoBehaviour {
    public static JumpscareManager Instance;

    [Header("All Ghosts")]
    public GhostJumpscare[] ghosts;

    [Header("Jumpscare UI")]
    public Canvas storyCanvas;
    public TMP_Text cutsceneText;

    [Header("Text Settings")]
    public float textDisplayDuration = 2f;

    public bool IsJumpscareActive { get; set; }


    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        // Disable all ghosts at start
        foreach (var g in ghosts) {
            if (g.ghostObject != null)
                g.ghostObject.SetActive(false);
        }
    }

    public void StartJumpscare(string ghostName, bool allowRepeat = false) {

        Debug.Log("Trying to start jumpscare: " + ghostName);

        if (IsJumpscareActive) {
            Debug.Log("Blocked: IsJumpscareActive");
            return;
        }

        GhostJumpscare ghost = GetGhostByName(ghostName);

        if (ghost == null) {
            Debug.Log("Blocked: Ghost not found");
            return;
        }

        if (!allowRepeat && ghost.played) {
            Debug.Log("Blocked: Ghost already played");
            return;
        }
        if (!string.IsNullOrEmpty(ghost.jumpscareText))
            StartCoroutine(ShowJumpscareText(ghost.jumpscareText));

        ghost.played = true;
        IsJumpscareActive = true;

        GhostJumpscareHelper.Instance.PlayGhost(ghost);
    }

    private IEnumerator ShowJumpscareText(string message) {
        if (storyCanvas == null || cutsceneText == null)
            yield break;

        storyCanvas.gameObject.SetActive(true);
        cutsceneText.text = message;

        yield return new WaitForSeconds(textDisplayDuration);

        storyCanvas.gameObject.SetActive(false);
    }

    public GhostJumpscare GetGhostByName(string ghostName) {
        foreach (var g in ghosts) {
            if (g.name == ghostName)
                return g;
        }
        Debug.LogWarning($"Ghost '{ghostName}' not found!");
        return null;
    }


}
