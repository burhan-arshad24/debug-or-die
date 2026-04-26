using UnityEngine;

public class CrowbarPickup : MonoBehaviour, IInteractable {
    public string itemId = "Crowbar";

    private void Awake() {
        // Ensure Crowbar is hidden at start
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt() {
        return "Pick up Crowbar";
    }

    public void Interact() {
        Inventory.Instance.AddItem(itemId);
        Debug.Log(itemId + " picked up!");
        gameObject.SetActive(false);
    }

    // Call this from TaskManager to make it available
    public void MakeAvailable() {
        gameObject.SetActive(true);
    }
}
