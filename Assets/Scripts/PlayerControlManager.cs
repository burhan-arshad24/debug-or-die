using UnityEngine;

public class PlayerControlManager : MonoBehaviour {
    public static PlayerControlManager Instance;

    [Header("Player Components")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private MonoBehaviour mouseLook;
    [SerializeField] private Transform playerRoot;
    [Header("Player Camera")]
    public Transform playerCamera; // assign your Main Camera here

    private bool rotationLocked;
    private bool hidingLookMode;

    public void EnableHidingLook() {
        hidingLookMode = true;
    }

    public void DisableHidingLook() {
        hidingLookMode = false;
    }

    public bool IsHidingLookActive() {
        return hidingLookMode;
    }


    public void LockRotation() {
        rotationLocked = true;
    }

    public void UnlockRotation() {
        rotationLocked = false;
    }

    public bool IsRotationLocked() {
        return rotationLocked;
    }


    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LockControl() {
        if (playerMovement) playerMovement.enabled = false;
        if (mouseLook) mouseLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockControl() {
        if (playerMovement) playerMovement.enabled = true;
        if (mouseLook) mouseLook.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void LockMovementOnly() {
        if (playerMovement) playerMovement.enabled = false;
    }

    public void UnlockMovementOnly() {
        if (playerMovement) playerMovement.enabled = true;
    }

}
