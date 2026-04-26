using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable {
    public string interactPrompt = "Pick Battery";
    public bool isPicked = false;

    public void Interact() {
        if (!isPicked) {
            isPicked = true;
            Inventory.Instance.AddItem("Battery");
            gameObject.SetActive(false);

            TaskManager.Instance.OnBatteryFound();
        }
    }

    public string GetInteractPrompt() => interactPrompt;
}
