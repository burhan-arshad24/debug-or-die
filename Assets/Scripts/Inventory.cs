using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    private HashSet<string> items = new HashSet<string>();

    [Header("SFX")]
    public bool playPickupSound = true; // toggle in inspector
    public float pickupVolume = 0.5f;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string itemId) {
        if (string.IsNullOrEmpty(itemId)) return;

        bool isNewItem = items.Add(itemId); // true if item wasn't already in inventory

        if (isNewItem && playPickupSound) {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupClip, pickupVolume);
        }
    }

    public bool HasItem(string itemId) {
        return items.Contains(itemId);
    }

    public void RemoveItem(string itemId) {
        if (items.Contains(itemId))
            items.Remove(itemId);
    }

    public void ClearInventory() {
        items.Clear();
    }

    public IEnumerable<string> GetAllItems() {
        return items;
    }
}
