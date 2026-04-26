using UnityEngine;

namespace SojaExiles {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour {
        public CharacterController controller;
        public float speed = 5f;
        public float gravity = -15f;

        private Vector3 lastPosition;
        private Vector3 velocity;

        private void Update() {
            if (PlayerState.IsLocked) {
                AudioManager.Instance.StopWalkSFX();
                return;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            HandleFootsteps();
        }

        private void HandleFootsteps() {
            // Check if player moved this frame
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            if (distanceMoved > 0.01f) {
                AudioManager.Instance.PlayWalkSFX();
            }
            else {
                AudioManager.Instance.StopWalkSFX();
            }

            lastPosition = transform.position;
        }
    }
}
