using UnityEngine;

public class AccessCardPickup : MonoBehaviour, IInteractable {
    public string itemId = "AccessCard";

    private void Awake() {
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt() {
        return "Pick up Access Card";
    }

    public void Interact() {
        Inventory.Instance.AddItem(itemId);
        Debug.Log("Access Card picked up!");
        gameObject.SetActive(false);

        TaskManager.Instance.OnAccessCardPicked();
    }
}
