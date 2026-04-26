using UnityEngine;
using TMPro;

public class PCUIManager : MonoBehaviour {
    public static PCUIManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject pcPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    private bool isOpen;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Do not change serialized references here
        if (pcPanel != null)
            pcPanel.SetActive(false);
    }

    private void Update() {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
            ClosePC();
    }

    public void OpenPC(PCTaskData data) {
        if (data == null) return;

        titleText.text = data.taskTitle;
        bodyText.text = data.taskDescription;

        pcPanel.SetActive(true);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Canvas.ForceUpdateCanvases();
        PlayerControlManager.Instance.LockControl();
    }

    public void ClosePC() {
        pcPanel.SetActive(false);
        isOpen = false;
        PlayerControlManager.Instance.UnlockControl();
    }

    public void ShowCompletion(string message) {
        titleText.text = "Task Completed!";
        bodyText.text = message;

        pcPanel.SetActive(true);
        isOpen = true;
        PlayerControlManager.Instance.LockControl();
    }

    private void OnDestroy() {
        if (Instance == this)
            Instance = null;
    }
}
