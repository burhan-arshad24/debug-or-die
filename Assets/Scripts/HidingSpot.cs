using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractable {

    [Header("Camera")]
    [SerializeField] private Transform hideCameraPoint;
    public KeyCode exitKey = KeyCode.Escape;

    private Transform playerCamera;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private bool isHiding;

    public string GetInteractPrompt() {
        // For testing, always show prompt
        return isHiding ? "Exit Hide (ESC)" : "Hide";
    }

    public void Interact() {
        if (isHiding)
            ExitHide();
        else
            EnterHide();
    }

    private void EnterHide() {
        playerCamera = Camera.main.transform;

        originalCamPos = playerCamera.position;
        originalCamRot = playerCamera.rotation;

        playerCamera.position = hideCameraPoint.position;
        playerCamera.rotation = hideCameraPoint.rotation;

        PlayerControlManager.Instance.LockMovementOnly();
        PlayerControlManager.Instance.LockRotation();
        PlayerControlManager.Instance.EnableHidingLook();
        PlayerInteractor.Instance.DisableInteraction();

        isHiding = true;
    }

    private void ExitHide() {
        playerCamera.position = originalCamPos;
        playerCamera.rotation = originalCamRot;

        PlayerControlManager.Instance.UnlockMovementOnly();
        PlayerControlManager.Instance.UnlockRotation();
        PlayerControlManager.Instance.DisableHidingLook();
        PlayerInteractor.Instance.EnableInteraction();

        isHiding = false;
    }

    private void Update() {
        if (!isHiding) return;

        // Manual exit
        if (Input.GetKeyDown(exitKey)) {
            ExitHide();
        }

        if (isHiding &&
    JumpscareManager.Instance != null &&
    !JumpscareManager.Instance.IsJumpscareActive) {
            ExitHide();
        }

    }
}
