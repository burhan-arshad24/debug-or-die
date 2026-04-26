using System.Collections;
using UnityEngine;

namespace SojaExiles {
    public class opencloseLibDoor : MonoBehaviour, IInteractable {
        public Animator openandclose;
        public bool open;

        [Header("Lock (Optional)")]
        public bool isLocked;
        public string requiredKeyId;
        public int stateToPrint = 0;

        public string GetInteractPrompt() {
            if (isLocked) {
                if(stateToPrint==0)
                return "Locked. Find Access Card to Unlock it";
                else
                    return "Find Another Way to Get out";

            }
            return open ? "Close Door" : "Open Door";
        }

        public void Interact() {
            if (isLocked) {
                TryUnlock();
                return;
            }

            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        void TryUnlock() {
            if (!Inventory.Instance.HasItem(requiredKeyId)) {
                Debug.Log("Door is locked");
                return;
            }

            Inventory.Instance.RemoveItem(requiredKeyId);
            isLocked = false;
            StartCoroutine(opening());
        }

        IEnumerator opening() {
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing() {
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
