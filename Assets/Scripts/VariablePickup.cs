using UnityEngine;

public class VariablePickup : MonoBehaviour, IInteractable {

    [Header("Variable")]
    public string variableName;
    public bool isCorrectVariable;

    public string GetInteractPrompt() {
        return variableName;
    }

    public void Interact() {
        TaskManager.Instance.OnVariablePicked(isCorrectVariable);
        gameObject.SetActive(false);
    }
}
