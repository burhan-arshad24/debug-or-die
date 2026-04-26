using UnityEngine;

public class DoorLock : MonoBehaviour {
    [SerializeField] private bool startLocked = true;
    [SerializeField] private string requiredKeyId;

    private bool locked;

    private void Awake() {
        locked = startLocked;
    }

    public bool IsLocked() {
        return locked;
    }

    public bool CanUnlock() {
        return Inventory.Instance.HasItem(requiredKeyId);
    }

    public void Unlock() {
        locked = false;
    }

    public string GetKeyId() {
        return requiredKeyId;
    }
}
