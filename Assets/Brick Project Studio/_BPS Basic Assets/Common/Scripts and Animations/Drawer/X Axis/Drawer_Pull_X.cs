using System.Collections;
using UnityEngine;

namespace SojaExiles {
    public class Drawer_Pull_X : MonoBehaviour, IInteractable {
        public Animator pull_01;
        public bool open;

        public string GetInteractPrompt() {
            return open ? "Close Drawer" : "Open Drawer";
        }

        public void Interact() {
            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        IEnumerator opening() {
            pull_01.Play("openpull_01");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing() {
            pull_01.Play("closepush_01");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
