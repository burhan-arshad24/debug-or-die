using UnityEngine;

public class SemicolonPickup : MonoBehaviour, IInteractable {
    public string itemId = "SEMICOLON";
    public GameObject semicolonObject; // Assign the scene object

    private bool isHidden = true;

    public string GetInteractPrompt() {
        return "Pick Semicolon";
    }

    public void Interact() {
        // Add to inventory
        Inventory.Instance.AddItem(itemId);

        // Notify TaskManager
        TaskManager.Instance.OnSemicolonPicked();

        // Hide semicolon in scene
        HideSemicolon();
    }

    public void HideSemicolon() {
        if (semicolonObject != null) {
            semicolonObject.SetActive(false);
            isHidden = true;
        }
    }

    public void ShowSemicolonObject() {
        if (semicolonObject != null) {
            semicolonObject.SetActive(true);
            isHidden = false;
        }
    }
}
