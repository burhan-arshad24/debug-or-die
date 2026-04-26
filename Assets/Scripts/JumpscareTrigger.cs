using UnityEngine;

public class JumpscareTrigger : MonoBehaviour {
    [Header("Ghost Settings")]
    public string ghostName;
    public bool allowRepeat = false;

    [Header("Optional Conditions")]
    public bool requireNoteRead = false;
    public bool requireTask2Key = false;
    public bool requireTask3ChestUnlocked = false;
    public bool requireTask3BookRead = false;
    public bool requireTask4BookPicked = false;
    public bool requirePowerPanelChecked = false;


    public TaskManager taskManager;

    public bool triggered = false;

    [Header("Trigger Type")]
    public bool isColliderTrigger = true; // true = needs player collider, false = direct call only

    // --- COLLIDER-BASED TRIGGER ---
    private void OnTriggerEnter(Collider other) {
        if (!isColliderTrigger) return; // Ignore collider if not meant for collider

        if (!other.CompareTag("Player"))
            return;

        if (triggered && !allowRepeat)
            return;

        if (taskManager == null) {
            Debug.LogError($"TaskManager NOT assigned in JumpscareTrigger for {ghostName}");
            return;
        }

        // Check conditions for collider triggers
        // Only check conditions for collider triggers
        if (isColliderTrigger) {
            if ((requireNoteRead && !taskManager.IsNoteRead) ||
    (requireTask2Key && !taskManager.IsTask2KeyPicked) ||
    (requireTask3ChestUnlocked && !taskManager.IsChestUnlocked) ||
    (requireTask3BookRead && !taskManager.IsBookRead) ||
    (requireTask4BookPicked && !taskManager.HasLibraryBook) ||
    (requirePowerPanelChecked && !taskManager.HasCheckedPowerPanel)) {
                Debug.Log($"Jumpscare blocked for {ghostName}, conditions not met");
                return;
            }

        }


        TriggerJumpscare();
    }

    // --- ACTUAL JUMPSCARE LOGIC ---
    public void TriggerJumpscare() {
        if (triggered && !allowRepeat)
            return;

        triggered = true;
        Debug.Log($"Triggering jumpscare for {ghostName}");
        JumpscareManager.Instance.StartJumpscare(ghostName, allowRepeat);
    }

    // --- DIRECT CALL FROM TASKMANAGER ---
    public void TriggerJumpscareDirect() {
        if (triggered && !allowRepeat)
            return;

        // Mark as direct (does NOT need collider)
        isColliderTrigger = false;

        Debug.Log($"Direct jumpscare called for {ghostName}");
        triggered = true;
        JumpscareManager.Instance.StartJumpscare(ghostName, allowRepeat);
    }

    // --- RESET (optional) ---
    public void ResetTrigger() {
        triggered = false;
        isColliderTrigger = true; // reset to default
    }
}
