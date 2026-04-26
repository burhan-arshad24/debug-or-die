using SojaExiles;
using System.Collections;
using UnityEngine;

public class VentController : MonoBehaviour {
    [Header("Waypoints")]
    public Transform[] waypoints;      // Assign waypoints in inspector
    public float moveSpeed = 3f;       // Player movement along vent
    public int jumpscareWaypointIndex = 1; // 0-based index for jumpscare

    [Header("Player References")]
    public Transform player;           // Drag Player transform here
    public MouseLook mouseLook;        // Drag your MouseLook component
    public PlayerMovement playerMovement; // Drag PlayerMovement component

    private int currentWaypoint = 0;
    private bool isTravelling = false;

    public void StartVentTravel() {
        if (waypoints == null || waypoints.Length == 0 || player == null) return;

        // Disable normal movement, enable vent mode
        if (mouseLook != null) mouseLook.enabled = true;   // Mouse look stays free
        if (playerMovement != null) playerMovement.enabled = false;

        TaskManager.Instance.OnVentEntered(); // Notify Task4
        currentWaypoint = 0;
        isTravelling = true;
        StartCoroutine(MoveAlongWaypoints());
    }

    private IEnumerator MoveAlongWaypoints() {
        // ===== Automatically move to first waypoint =====
        Transform firstTarget = waypoints[0];
        while (Vector3.Distance(player.position, firstTarget.position) > 0.05f) {
            player.position = Vector3.MoveTowards(player.position, firstTarget.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        currentWaypoint = 1; // Ready for manual movement

        // ===== Manual movement through remaining waypoints =====
        while (currentWaypoint < waypoints.Length) {
            // Wait for W key press
            yield return new WaitUntil(() => Input.GetKey(KeyCode.W));

            Transform target = waypoints[currentWaypoint];

            // Move towards next waypoint while holding W
            while (Vector3.Distance(player.position, target.position) > 0.05f) {
                if (!Input.GetKey(KeyCode.W)) break; // Stop moving if W released
                player.position = Vector3.MoveTowards(player.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // If player reached target, trigger jumpscare if applicable
            if (Vector3.Distance(player.position, target.position) <= 0.05f) {
                if (currentWaypoint == jumpscareWaypointIndex) {
                    Debug.Log("Vent jumpscare triggered at waypoint " + currentWaypoint);
                    TaskManager.Instance.OnVentJumpscareTriggered();
                }
                currentWaypoint++;
            }

            yield return null;
        }

        // ===== Re-enable normal controls =====
        if (mouseLook != null) mouseLook.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;

        TaskManager.Instance.OnVentExited(); // Notify Task4
        isTravelling = false;
    }
}
