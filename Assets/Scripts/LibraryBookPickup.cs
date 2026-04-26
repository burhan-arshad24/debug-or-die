using UnityEngine;

public class LibraryBookPickup : MonoBehaviour, IInteractable {
    public string itemId = "LibraryBook";

    private void Awake() {
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt() {
        return "Pick up Library Book";
    }

    public void Interact() {
        Inventory.Instance.AddItem(itemId);
        Debug.Log("Library Book found!");
        gameObject.SetActive(false);

        TaskManager.Instance.OnLibraryBookFound(); // FIXED call
    }
}
