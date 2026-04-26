using UnityEngine;

public class BookInteraction : MonoBehaviour, IInteractable {
    private bool bookVisible = false;

    private void Awake() {
        gameObject.SetActive(false);
    }

    public void ShowBook() {
        bookVisible = true;
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
    }

    public void HideBook() {
        bookVisible = false;
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt() {
        if (!TaskManager.Instance.IsTask3Started || !bookVisible)
            return "";

        bool hasUV = Inventory.Instance.HasItem("UVLamp");
        bool hasBattery = Inventory.Instance.HasItem("Battery");

        if (!hasUV && !hasBattery)
            return "Book is blank. Something is hidden. Find UV light to reveal.";

        if (hasUV && !hasBattery)
            return "UV lamp has no power. Find battery.";

        if (hasUV && hasBattery)
            return "Reveal hidden text";

        return "Read Book";
    }

    public void Interact() {
        if (!TaskManager.Instance.IsTask3Started || !bookVisible)
            return;

        bool hasUV = Inventory.Instance.HasItem("UVLamp");
        bool hasBattery = Inventory.Instance.HasItem("Battery");

        if (hasUV && hasBattery) {
            TaskManager.Instance.OnBookRead();
            TaskManager.Instance.CompleteTask3();
            HideBook();
        }
    }
}
