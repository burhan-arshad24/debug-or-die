using UnityEngine;
using System.Collections;

public class PCInteractable : MonoBehaviour, IInteractable {
    [Header("Task Data")]
    public PCTaskData firstTaskData;
    public PCTaskData task1CompletionData;

    public PCTaskData secondTaskData;
    public PCTaskData task2CompletionData;

    public PCTaskData thirdTaskData;
    public PCTaskData thirdTaskCompletionData;

    [Header("Task4 Data")]
    public PCTaskData fourthTaskData;
    public PCTaskData fourthTaskPowerOfflineData;
    public PCTaskData fourthTaskCompletionData;

    private bool isShowingTask1Completion;

    public string GetInteractPrompt() => "Use Computer";

    public void Interact() {
        // ===== TASK 1 =====
        if (!TaskManager.Instance.IsTask1Started) {
            PCUIManager.Instance.OpenPC(firstTaskData);
            TaskManager.Instance.OnTask1Started();
            return;
        }

        if (!TaskManager.Instance.IsTask1Completed) {
            PCUIManager.Instance.OpenPC(firstTaskData);
            return;
        }

        if (!TaskManager.Instance.IsTask2Started) {
            if (!isShowingTask1Completion)
                TaskManager.Instance.StartCoroutine(ShowTask1CompletionThenStartTask2());
            return;
        }

        // ===== TASK 2 =====
        if (TaskManager.Instance.IsTask2Started && !TaskManager.Instance.IsTask2Completed) {
            PCUIManager.Instance.OpenPC(secondTaskData);
            return;
        }

        if (TaskManager.Instance.IsTask2Completed && !TaskManager.Instance.IsTask3Started) {
            PCUIManager.Instance.OpenPC(thirdTaskData);
            TaskManager.Instance.StartTask3();
            return;
        }

        // ===== TASK 3 =====
        if (TaskManager.Instance.IsTask3Started && !TaskManager.Instance.IsTask3Completed) {
            PCUIManager.Instance.OpenPC(thirdTaskData);
            return;
        }

        if (TaskManager.Instance.IsTask3Completed && !TaskManager.Instance.IsTask4Started) {
            // Auto-start Task4 when Task3 is done
            PCUIManager.Instance.OpenPC(fourthTaskData);
            TaskManager.Instance.StartTask4();
            return;
        }

        // ===== TASK 4 =====
        if (TaskManager.Instance.IsTask4Started && !TaskManager.Instance.HasLibraryBook) {
            // Library data missing
            PCUIManager.Instance.OpenPC(fourthTaskData);
            return;
        }

        // Book installed but no power yet
        if (TaskManager.Instance.HasLibraryBook && !TaskManager.Instance.ElectricityOn) {
            PCUIManager.Instance.OpenPC(fourthTaskPowerOfflineData);
            return;
        }

        // Power restored and library data installed -> finalize PC step
        if (!TaskManager.Instance.PcChecked) {
            PCUIManager.Instance.OpenPC(fourthTaskCompletionData);
            TaskManager.Instance.OnPcChecked();
            return;
        }

        // Completed state
        if (TaskManager.Instance.PcChecked) {
            PCUIManager.Instance.OpenPC(fourthTaskCompletionData);
            return;
        }

    }

    private IEnumerator ShowTask1CompletionThenStartTask2() {
        isShowingTask1Completion = true;

        PCUIManager.Instance.OpenPC(task1CompletionData);
        yield return new WaitForSeconds(2f);
        PCUIManager.Instance.ClosePC();

        PCUIManager.Instance.OpenPC(secondTaskData);
        TaskManager.Instance.OnTask2Started();

        isShowingTask1Completion = false;
    }
}
