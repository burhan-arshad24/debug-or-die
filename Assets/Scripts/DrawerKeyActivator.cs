using UnityEngine;

public class DrawerKeyActivator : MonoBehaviour {
    public GameObject key; // assign your key
    public Animator drawerAnimator;

    // call via animation event at end of opening animation
    public void ActivateKey() {
        key.SetActive(true);
    }
}
