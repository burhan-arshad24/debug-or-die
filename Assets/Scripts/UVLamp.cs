using UnityEngine;

public class UVLamp : MonoBehaviour, IInteractable {
    private bool isPicked = false;

    private void Awake() {
        gameObject.SetActive(false);
    }

    public void MakeAvailable() {
        isPicked = false;
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
    }

    public string GetInteractPrompt() {
        if (isPicked) return "";
        return "Pick UV Lamp";
    }

    public void Interact() {
        if (isPicked) return;

        isPicked = true;
        Inventory.Instance.AddItem("UVLamp");
        TaskManager.Instance.OnUVFound();
        gameObject.SetActive(false);
    }
}
