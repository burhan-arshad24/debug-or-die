using UnityEngine;

namespace SojaExiles {
    public class MouseLook : MonoBehaviour {

        public float mouseSensitivity = 100f;
        public Transform playerBody;

        public bool overrideInput = false;

        private float pitch = 0f;

        void Start() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update() {

            if (PlayerState.IsLocked || overrideInput) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Handle pitch (up/down)
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            // Handle yaw (left/right)
            if (!PlayerControlManager.Instance.IsRotationLocked()) {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        public void ResetLook() {
            pitch = 0f;
            transform.localRotation = Quaternion.identity;
        }
    }
}
