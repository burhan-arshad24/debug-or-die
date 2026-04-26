using UnityEngine;

[CreateAssetMenu(menuName = "Horror/Note Data")]
public class NoteData : ScriptableObject {
    [TextArea(5, 20)]
    public string noteText;
}
