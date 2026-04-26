using UnityEngine;

public class FusePickup : MonoBehaviour, IInteractable {
    public string itemId = "Fuse";

    public string GetInteractPrompt() {
        return "Pick up Fuse";
    }

    public void Interact() {
        Inventory.Instance.AddItem(itemId);
        Debug.Log("Fuse picked up");

        TaskManager.Instance.OnFuseFound();

        gameObject.SetActive(false);
    }
}
