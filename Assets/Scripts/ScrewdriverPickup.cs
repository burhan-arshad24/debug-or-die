using UnityEngine;

public class ScrewdriverPickup : MonoBehaviour, IInteractable {
    public string itemId = "Screwdriver";

    public string GetInteractPrompt() {
        return "Pick up Screwdriver";
    }

    public void Interact() {
        Inventory.Instance.AddItem(itemId);
        Debug.Log("Screwdriver picked up");

        TaskManager.Instance.OnScrewdriverFound();

        gameObject.SetActive(false);
    }
}
