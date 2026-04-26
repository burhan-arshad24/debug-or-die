using UnityEngine;

public class NoteInteractable : MonoBehaviour, IInteractable {
    [Header("Note Data")]
    public NoteData noteData; // Assign in Inspector
    public GameObject noteObject; // Assign your scene object (mesh or panel)

    private bool isHidden = true;

    public string GetInteractPrompt() {
        return "Read Note";
    }

    public void Interact() {
        if (noteData == null) {
            Debug.LogError("NoteData not assigned!");
            return;
        }

        // Show note UI
        NoteUIManager.Instance.ShowNote(noteData);

        // Trigger TaskManager
        TaskManager.Instance.OnNotePicked();

        // Hide the note object in scene
        HideNote();
    }

    public void HideNote() {
        if (noteObject != null) {
            noteObject.SetActive(false);
            isHidden = true;
        }
    }

    public void ShowNoteObject() {
        if (noteObject != null) {
            noteObject.SetActive(true);
            isHidden = false;
        }
    }
}
