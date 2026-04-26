using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable {
    [Header("Key Settings")]
    public string keyId;
    public string displayName;



    public string GetInteractPrompt() {
        return "Pick Up Key";
    }

    public void Interact() {
        Inventory.Instance.AddItem(keyId);
        PickupUIManager.Instance.ShowPickup(displayName);
        TaskManager.Instance.OnTask2KeyPicked();
        Destroy(gameObject);
    }

}
