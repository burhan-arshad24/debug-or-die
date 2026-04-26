using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour {
    public static NoteUIManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteText;

    private bool isOpen;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        notePanel.SetActive(false);
    }

    private void Update() {
        if (!isOpen) return;

        // Left Mouse Button OR Escape closes the note
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) {
            CloseNote();
        }
    }


    public void ShowNote(NoteData data) {
        noteText.text = data.noteText;
        notePanel.SetActive(true);
        isOpen = true;

        // Lock player
        PlayerControlManager.Instance.LockControl();
    }

    public void CloseNote() {
        notePanel.SetActive(false);
        isOpen = false;

        // Unlock player
        PlayerControlManager.Instance.UnlockControl();
    }
}
