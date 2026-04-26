using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour {
    [Header("Raycast")]
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private LayerMask interactableLayer;
    public static PlayerInteractor Instance;
    private bool interactionDisabled;

    [Header("UI")]
    [SerializeField] private TMP_Text interactText;

    private IInteractable currentInteractable;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (interactText != null) {
            interactText.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (interactionDisabled || CutsceneManager.IsCutsceneActive)
            return;

        CheckForInteractable();

        if (currentInteractable != null && Input.GetMouseButtonDown(0)) {
            currentInteractable.Interact();
        }

        Debug.DrawRay(transform.position, transform.forward * interactDistance, Color.green);
    }

    void CheckForInteractable() {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && IsBehaviourEnabled(interactable)) {
                currentInteractable = interactable;
                if (interactText != null) {
                    interactText.text = interactable.GetInteractPrompt();
                    interactText.gameObject.SetActive(true);
                }
                return;
            }
        }

        currentInteractable = null;
        if (interactText != null) {
            interactText.gameObject.SetActive(false);
        }
    }

    private bool IsBehaviourEnabled(IInteractable interactable) {
        var mb = interactable as MonoBehaviour;
        return mb != null && mb.isActiveAndEnabled;
    }

    public void DisableInteraction() {
        interactionDisabled = true;
    }

    public void EnableInteraction() {
        interactionDisabled = false;
    }
}
