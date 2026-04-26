using System.Collections;
using UnityEngine;

namespace SojaExiles {
    public class opencloseDoor1 : MonoBehaviour, IInteractable {
        [Header("Animator")]
        public Animator openandclose1;
        public bool open;

        public string GetInteractPrompt() {
            return open ? "Close" : "Open";
        }

        public void Interact() {
            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        IEnumerator opening() {
            openandclose1.Play("Opening 1");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing() {
            openandclose1.Play("Closing 1");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
