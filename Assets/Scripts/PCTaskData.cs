using UnityEngine;

[CreateAssetMenu(menuName = "Horror/PC Task Data")]
public class PCTaskData : ScriptableObject {
    [Header("Task Info")]
    public string taskTitle;

    [TextArea(5, 20)]
    public string taskDescription;
}
