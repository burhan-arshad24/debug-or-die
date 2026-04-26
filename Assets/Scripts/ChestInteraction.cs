using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable {
    public string requiredItem = "Crowbar";
    public Transform lid;
    public float openSpeed = 10f;
    public float maxOpenAngle = -60f;

    private bool isUnlocked = false;
    private bool isOpened = false;
    private float initialRotationX;

    private void Start() {
        if (lid != null)
            initialRotationX = lid.localEulerAngles.x;
    }

    private void Update() {
        if (isOpened && lid != null) {
            Vector3 rot = lid.localEulerAngles;
            float step = openSpeed * Time.deltaTime;
            rot.x = Mathf.MoveTowardsAngle(rot.x, maxOpenAngle, step);
            lid.localEulerAngles = rot;
        }
    }

    public string GetInteractPrompt() {
        if (!TaskManager.Instance.IsTask3Started) return "";
        if (isOpened) return "Chest Opened";
        if (Inventory.Instance.HasItem(requiredItem)) return "Open Chest";
        return "Chest is locked! Find Crowbar";
    }

    public void Interact() {
        if (!TaskManager.Instance.IsTask3Started) return;

        if (isUnlocked || isOpened) return;

        if (Inventory.Instance.HasItem(requiredItem)) {
            isUnlocked = true;
            isOpened = true;

            
            TaskManager.Instance.RevealBookAndUV(); // Activate both immediately
            TaskManager.Instance.OnChestUnlocked();
        }
        else {
            
        }
    }

    public void ResetChest() {
        isUnlocked = false;
        isOpened = false;

        if (lid != null) {
            Vector3 rot = lid.localEulerAngles;
            rot.x = initialRotationX;
            lid.localEulerAngles = rot;
        }
    }
}
