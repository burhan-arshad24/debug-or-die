using UnityEngine;

public class VentInteraction : MonoBehaviour, IInteractable {
    [Header("Requirements")]
    public string requiredItemId = "Screwdriver";

    [Header("References")]
    public VentController ventController; // drag in inspector

    private bool unlocked = false;

    public string GetInteractPrompt() {
        return unlocked ? "Enter Vent" : "Vent is Locked, Find ScrewDriver to Open";
    }

    public void Interact() {
        if (!unlocked) {
            // Check screwdriver
            if (!Inventory.Instance.HasItem(requiredItemId)) {
                Debug.Log("Vent locked. Need screwdriver.");
                return;
            }

            // consume screwdriver
            Inventory.Instance.RemoveItem(requiredItemId);
            unlocked = true;

            Debug.Log("Vent unlocked.");
            TaskManager.Instance.OnVentOpened();
            return;
        }

        // Enter vent
        if (ventController == null) {
            // Safety fallback (no deprecation warnings)
            ventController = Object.FindFirstObjectByType<VentController>();
        }

        if (ventController != null) {
            TaskManager.Instance.OnVentEntered(); // important for timer jump
            ventController.StartVentTravel();
        }
        else {
            Debug.LogWarning("VentController not found in scene!");
        }
    }
}
