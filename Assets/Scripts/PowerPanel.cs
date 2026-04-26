using UnityEngine;

public class PowerPanel : MonoBehaviour, IInteractable {
    private enum PowerStage {
        Damaged,
        BurntFuse,
        Fixed
    }

    private PowerStage stage = PowerStage.Damaged;

    public string requiredFuseId = "Fuse";

    public string GetInteractPrompt() {
        // Gate interaction: only after vent exit or PC shows offline
        if (!TaskManager.Instance.VentOpened && !TaskManager.Instance.PcChecked)
            return ""; // No prompt = cannot interact

        switch (stage) {
            case PowerStage.Damaged: return "Inspect Power Panel";
            case PowerStage.BurntFuse: return "Fuse Burnt, Find new Fuse";
            case PowerStage.Fixed: return "Power Restored";
        }
        return "";
    }

    public void Interact() {
        // Gate interaction: only after vent exit or PC shows offline
        if (!TaskManager.Instance.VentOpened && !TaskManager.Instance.PcChecked)
            return;

        switch (stage) {
            case PowerStage.Damaged:
                Debug.Log("Power supply damaged");
                TaskManager.Instance.OnPowerPanelChecked();
                stage = PowerStage.BurntFuse;
                break;

            case PowerStage.BurntFuse:
                if (!Inventory.Instance.HasItem(requiredFuseId)) {
                    Debug.Log("Fuse burnt. Find replacement");
                    return;
                }

                Inventory.Instance.RemoveItem(requiredFuseId);
                stage = PowerStage.Fixed;

                TaskManager.Instance.OnFuseInserted();
                TaskManager.Instance.OnElectricityRestored();
                break;

            case PowerStage.Fixed:
                Debug.Log("Power already restored");
                break;
        }
    }
}
