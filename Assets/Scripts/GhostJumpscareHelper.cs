using System.Collections;
using UnityEngine;

public class GhostJumpscareHelper : MonoBehaviour {
    public static GhostJumpscareHelper Instance;

    [Header("Camera Settings")]
    public bool useCameraLook = true;
    public Transform playerCamera;
    public float cameraRotateSpeed = 5f;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayGhost(GhostJumpscare ghost) {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpscareClip, 1f);
        StartCoroutine(PlayGhostRoutine(ghost));
    }

    private IEnumerator PlayGhostRoutine(GhostJumpscare ghost) {
    if (ghost == null || ghost.ghostObject == null)
        yield break;

    // Store original player camera rotation
    Quaternion originalCamRotation = playerCamera != null ? playerCamera.rotation : Quaternion.identity;

    // Lock player
    PlayerControlManager.Instance.LockControl();
    PlayerInteractor.Instance?.DisableInteraction();

    ghost.ghostObject.SetActive(true);


        // Start position
        if (ghost.waypoints != null && ghost.waypoints.Length > 0)
        ghost.ghostObject.transform.position = ghost.waypoints[0].position;

    yield return null;
    // Play animation
    if (ghost.animator != null)
        ghost.animator.SetTrigger("play");

    // Move through waypoints
    if (ghost.waypoints != null && ghost.waypoints.Length > 0) {
        for (int i = 1; i < ghost.waypoints.Length; i++) {
            Transform target = ghost.waypoints[i];

            while (true) {
                Vector3 current = ghost.ghostObject.transform.position;
                Vector3 targetPos = new Vector3(target.position.x, current.y, target.position.z);

                if (Vector3.Distance(current, targetPos) < 0.05f)
                    break;

                // Move ghost toward target
                Vector3 nextPos = Vector3.MoveTowards(current, targetPos, ghost.moveSpeed * Time.deltaTime);
                ghost.ghostObject.transform.position = nextPos;

                // Rotate ghost horizontally toward target
                Vector3 lookDir = targetPos - nextPos;
                lookDir.y = 0;
                if (lookDir != Vector3.zero) {
                    ghost.ghostObject.transform.rotation = Quaternion.Slerp(
                        ghost.ghostObject.transform.rotation,
                        Quaternion.LookRotation(lookDir),
                        ghost.moveSpeed * Time.deltaTime
                    );
                }

                // Rotate player camera toward ghost
                if (useCameraLook && playerCamera != null) {
                    Vector3 camDir = nextPos - playerCamera.position;
                    camDir.y = 0;
                    if (camDir != Vector3.zero) {
                        playerCamera.rotation = Quaternion.Slerp(
                            playerCamera.rotation,
                            Quaternion.LookRotation(camDir),
                            cameraRotateSpeed * Time.deltaTime
                        );
                    }
                }

                yield return null;
            }
        }
    }

    // Handle idle ghosts (only one waypoint or no movement)
    if (ghost.waypoints == null || ghost.waypoints.Length <= 1) {
        float timer = 0f;
        while (timer < ghost.duration) {
            timer += Time.deltaTime;

            // Keep rotating camera toward ghost
            if (useCameraLook && playerCamera != null) {
                Vector3 camDir = ghost.ghostObject.transform.position - playerCamera.position;
                camDir.y = 0;
                if (camDir != Vector3.zero) {
                    playerCamera.rotation = Quaternion.Slerp(
                        playerCamera.rotation,
                        Quaternion.LookRotation(camDir),
                        cameraRotateSpeed * Time.deltaTime
                    );
                }
            }

            yield return null;
        }
    }

    // Stay visible
    yield return new WaitForSeconds(ghost.duration);

    ghost.ghostObject.SetActive(false);
    


        // Unlock player
        PlayerControlManager.Instance.UnlockControl();
    PlayerInteractor.Instance?.EnableInteraction();

    // Smoothly return camera to original rotation
    if (playerCamera != null) {
        float elapsed = 0f;
        float duration = 1.0f; // Adjust duration for smoother/faster return
        Quaternion startRotation = playerCamera.rotation;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            playerCamera.rotation = Quaternion.Slerp(startRotation, originalCamRotation, t);
            yield return null;
        }

        // Ensure exact final rotation
        playerCamera.rotation = originalCamRotation;
    }

    JumpscareManager.Instance.IsJumpscareActive = false;
}

}
