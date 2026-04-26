using SojaExiles;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour {
    public static TaskManager Instance;

    [Header("Debug / Testing")]
    [SerializeField] private bool skipTask1 = false;
    [SerializeField] private bool skipTask2 = false;
    [SerializeField] private bool skipTask3 = false;

    [Header("HUD")]
    public TMP_Text currentTaskText;

    [Header("Main Menu UI")]
    public Canvas mainMenuCanvas;
    private bool gameStarted = false;

    [Header("Pause Menu UI")]
    public Canvas pauseMenuCanvas;
    private bool isPaused = false;

    [Header("Task 0 State")]
    public bool IsTask0Started { get; private set; }
    public bool IsTask0Completed { get; private set; }




    [Header("Task 1 Objects")]
    public NoteInteractable noteObject;
    public SemicolonPickup semicolonObject;
    public GameObject task1Ghost;


    [Header("Task 1 State")]
    public bool IsTask1Started { get; private set; }
    public bool IsTask1Completed { get; private set; }
    public bool IsNoteRead { get; private set; }
    public bool IsSemicolonPicked { get; private set; }


    [Header("Task 2 Objects")]
    public GameObject[] variableObjects;
    public KeyPickup task2Key;
    public opencloseDoor officeDoor;

    [Header("Task 2 State")]
    public bool IsTask2Started { get; private set; }
    public bool IsTask2Completed { get; private set; }
    public bool IsVariablePicked { get; private set; }
    public bool CorrectVariablePicked { get; private set; }
    public bool IsTask2KeyPicked { get; private set; }

    [Header("Task 3 Objects")]
    public ChestInteraction secretChest;
    public BookInteraction bookObject;
    public UVLamp uvLamp;
    public BatteryPickup batteryPickup;
    public CrowbarPickup crowbar;
    [Header("Task 3 Jumpscare Triggers")]
    public JumpscareTrigger chestJumpscareTrigger; 
    public JumpscareTrigger hiddenTextJumpscareTrigger;
    public JumpscareTrigger libraryBookJumpscare;

    [Header("Task 3 State")]
    public bool IsTask3Started { get; private set; }
    public bool IsTask3Completed { get; private set; }
    public bool IsChestFound { get; private set; }
    public bool IsChestUnlocked { get; private set; }
    public bool IsBookFound { get; private set; }
    public bool IsUVFound { get; private set; }
    public bool IsBatteryFound { get; private set; }
    public bool IsBookRead { get; private set; }

    [Header("Task Timers")]
    [SerializeField] private float task1Duration = 120f;
    [SerializeField] private float task2Duration = 180f;
    [SerializeField] private float task3Duration = 300f;
    public float task4Duration = 240f;

    private float taskTimer;
    private bool timerActive = false;

    [Header("HUD & GameOver UI")]
    public Canvas gameOverCanvas;
    public TMP_Text timerText;
    public Transform pcSpawnPoint;
    private bool isGameOver = false;

    private void Awake() {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        StartCoroutine(InitializeAndShowMenu());


        if (noteObject != null) noteObject.HideNote();
        if (semicolonObject != null) semicolonObject.HideSemicolon();

        foreach (var v in variableObjects) v.SetActive(false);

        if (task2Key != null) task2Key.gameObject.SetActive(false);
        if (officeDoor != null) officeDoor.isLocked = false;
        if (secretChest != null) secretChest.ResetChest();
        if (bookObject != null) bookObject.HideBook();
        if (uvLamp != null) uvLamp.gameObject.SetActive(false);
        if (batteryPickup != null) batteryPickup.gameObject.SetActive(false);
        // ===== TASK 4 Initialization =====
        //if (libraryDoor != null) libraryDoor.SetActive(false);
        if (accessCard != null) accessCard.SetActive(false);
        if (libraryBook != null) libraryBook.SetActive(false);
        //if (ventEntry != null) ventEntry.SetActive(false);
        if (screwdriver != null) screwdriver.SetActive(false);
        if (fuseObject != null) fuseObject.SetActive(false);
        //if (electricityBoard != null) electricityBoard.SetActive(false);
        //if (pcInteraction != null) pcInteraction.SetActive(false);


        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (skipTask1) {
            //Task1 Complete
            IsTask1Started = true;
            IsTask1Completed = true;
            IsNoteRead = true;
            IsSemicolonPicked = true;
            CurrentTaskIndex = 2;
        }
        if (skipTask2) {

            //Task2 Compelete
            IsTask2Started = true;
            IsTask2Completed = true;
            CorrectVariablePicked = true;
            IsTask2KeyPicked = true;
            CurrentTaskIndex = 3;
        }
        if (skipTask3) { 

            //task3 complte 
            IsTask3Started = true;
            IsTask3Completed = true;
            IsChestFound = true;
            IsChestUnlocked = true;
            IsBookFound = true;
            IsUVFound = true;
            IsBatteryFound = true;
            IsBookRead = true;
            CurrentTaskIndex = 4;

        }
        if (cutsceneManager != null && cutsceneManager.storyCanvas != null)
            cutsceneManager.storyCanvas.gameObject.SetActive(false);
    }

    

    private void Update() {

        // Pause toggle
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isGameOver) return;
            if (mainMenuCanvas != null && mainMenuCanvas.gameObject.activeSelf) return;

            if (isPaused) ResumeFromPause();
            else PauseGame();
        }
        if (!timerActive) return;

        taskTimer -= Time.deltaTime;

        if (timerText != null) {
            timerText.gameObject.SetActive(true);
            int minutes = Mathf.FloorToInt(taskTimer / 60);
            int seconds = Mathf.FloorToInt(taskTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (taskTimer <= 0f && !isGameOver) {
            timerActive = false;
            TriggerGameOver();
        }

        if (timerText != null) {
            timerText.gameObject.SetActive(true);
            int minutes = Mathf.FloorToInt(taskTimer / 60);
            int seconds = Mathf.FloorToInt(taskTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (taskTimer <= 0f && !isGameOver) {
            timerActive = false;
            TriggerGameOver();
        }

        if (IsTask0Started && !IsTask0Completed && Input.GetKeyDown(KeyCode.E)) {
            OnTorchTurnedOn();
        }
    }

    public CutsceneManager cutsceneManager;
    // Main Menu

    private IEnumerator InitializeAndShowMenu()
    {
        // Wait a frame to ensure all singletons Awake() methods have run
        yield return null;

        if (mainMenuCanvas != null)
            mainMenuCanvas.gameObject.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);

        if (PlayerControlManager.Instance != null)
            PlayerControlManager.Instance.LockControl();

        StopTaskTimer();
    }

    public void PlayFromMainMenu() {
        Debug.Log("Play pressed");

        gameStarted = true;
        CurrentTaskIndex = 1;

        HideMainMenu();
        // Start cutscene first
        if (cutsceneManager != null)
            StartCoroutine(cutsceneManager.CutsceneFlow());

    }

    public void ContinueFromMainMenu() {
        if (!gameStarted) {
            Debug.Log("Continue pressed but game not started yet");
            return;
        }

        Debug.Log($"Continue pressed at Task {CurrentTaskIndex}");

        HideMainMenu();

        // Resume timer based on current task
        switch (CurrentTaskIndex) {
            case 1: StartTaskTimer(task1Duration); break;
            case 2: StartTaskTimer(task2Duration); break;
            case 3: StartTaskTimer(task3Duration); break;
            case 4: StartTaskTimer(task4Duration); break;
        }
    }


    private void ShowMainMenu() {
        if (mainMenuCanvas != null)
            mainMenuCanvas.gameObject.SetActive(true);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);

        PlayerControlManager.Instance.LockControl();
        StopTaskTimer();
    }

    private void HideMainMenu() {
        if (mainMenuCanvas != null)
            mainMenuCanvas.gameObject.SetActive(false);
        AudioManager.Instance.StopMusic();
        PlayerControlManager.Instance.UnlockControl();
    }

    // Pause game 
    private void PauseGame() {
        if (isPaused) return;

        isPaused = true;

        Time.timeScale = 0f;

        StopTaskTimer();

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.gameObject.SetActive(true);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);


        PlayerControlManager.Instance.LockControl();

        Debug.Log("Game Paused");
    }

    public void ResumeFromPause() {
        if (!isPaused) return;

        isPaused = false;

        Time.timeScale = 1f;

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.gameObject.SetActive(false);

        PlayerControlManager.Instance.UnlockControl();

        // Resume timer based on current task
        switch (CurrentTaskIndex) {
            case 1: StartTaskTimer(task1Duration); break;
            case 2: StartTaskTimer(task2Duration); break;
            case 3: StartTaskTimer(task3Duration); break;
            case 4: StartTaskTimer(task4Duration); break;
        }

        Debug.Log("Game Resumed");
    }

    public void GoToMainMenuFromPause() {
        ResumeFromPause();
        ShowMainMenu();
    }

    public void QuitFromPause() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


    private void UpdateCurrentTaskText(string objective) {
        if (currentTaskText != null)
            currentTaskText.text = "Objective: " + objective;
    }


    // ===== TASK 0 =====
    public void StartTask0() {
        IsTask0Started = true;
        IsTask0Completed = false;

        // Give torch directly
        TorchManager.Instance.GrantTorch(false);
        Inventory.Instance.AddItem("Torch");

        UpdateCurrentTaskText("Press E to turn on Torch And Interact with Pc to Complete Assignment");
    }


    public void OnTorchTurnedOn() {
        if (!IsTask0Started || IsTask0Completed) return;

        IsTask0Completed = true;
        IsTask0Started = false;

        Debug.Log("Task 0 Complete: Torch turned ON");

    }


    // ===== TASK 1 =====
    public void OnTask1Started() {
        IsTask1Started = true;
        IsNoteRead = false;
        IsSemicolonPicked = false;

        // Only show Task1 objects
        if (noteObject != null)
            noteObject.ShowNoteObject();   // or whatever your show function is

        if (semicolonObject != null)
            semicolonObject.HideSemicolon();


        UpdateCurrentTaskText("Find Note");
        StartTaskTimer(task1Duration);

    }

    // ===== TASK 1 — TORCH ===




    public void OnNotePicked() {

        IsNoteRead = true;

        if (semicolonObject != null)
            semicolonObject.ShowSemicolonObject();
        UpdateCurrentTaskText("Find Semicolon in the place written on Note");
    }


    public void OnSemicolonPicked() {
        IsSemicolonPicked = true;
        UpdateCurrentTaskText("Go to PC for Submission");
        StartCoroutine(FinishTask1Sequence());
    }

    private IEnumerator FinishTask1Sequence() {
        yield return new WaitForSeconds(1.5f);
        IsTask1Completed = true;
    }

    // ===== TASK 2 =====
    public void OnTask2Started() {
        StopTaskTimer();
        if (IsTask2Started) return;
        IsTask2Started = true;
        foreach (var v in variableObjects) v.SetActive(true);
        StartTaskTimer(task2Duration);
        UpdateCurrentTaskText("Find Correct Variable Name In living Room");
    }

    public void OnVariablePicked(bool correct) {
        IsVariablePicked = true;
        CorrectVariablePicked = correct;

        if (!correct) return;

        if (officeDoor != null) {
            if (officeDoor.open) StartCoroutine(CloseThenLockDoor());
            else officeDoor.isLocked = true;
        }
        UpdateCurrentTaskText("Door is Locked. Find Key");
        if (task2Key != null)
            task2Key.gameObject.SetActive(true);
    }

    private IEnumerator CloseThenLockDoor() {
        officeDoor.StartCoroutine("closing");
        yield return new WaitForSeconds(0.6f);
        officeDoor.isLocked = true;
    }

    public void OnTask2KeyPicked() {
        if (!CorrectVariablePicked) return;

        IsTask2KeyPicked = true;
        
        UpdateCurrentTaskText("Go to PC for Submission");
        IsTask2Completed = true;

    }

    // ===== TASK 3 =====
    public void StartTask3() {
        
        StopTaskTimer();
        if (IsTask3Started) return;
        IsTask3Started = true;

        if (secretChest != null)
            secretChest.gameObject.SetActive(true);

        if (crowbar != null)
            crowbar.MakeAvailable();
        UpdateCurrentTaskText("Find and Open Chest");

        StartTaskTimer(task3Duration);
    }

    public void OnChestFound() {
        IsChestFound = true;
        UpdateCurrentTaskText("Find Crowbar to open it.");
    }

    public void OnChestUnlocked() {
        IsChestUnlocked = true;

        if (bookObject != null)
            bookObject.ShowBook();

        UpdateCurrentTaskText("Something is Hidden in Book. Find UVLamp to Reveal");

        if (chestJumpscareTrigger != null)
            chestJumpscareTrigger.TriggerJumpscareDirect(); // triggers immediately
    }

    public void OnUVFound() {
        IsUVFound = true;
        if (batteryPickup != null) batteryPickup.gameObject.SetActive(true);
        UpdateCurrentTaskText("UVLamp has no Power. Find Battery");
    }

    public void OnBatteryFound() {
        IsBatteryFound = true;
        UpdateCurrentTaskText("Go to Chest.");
    }

    public void OnBookRead() {
        if (!IsUVFound || !IsBatteryFound) return;

        IsBookRead = true;
        UpdateCurrentTaskText("Function Definition found. Go to PC");

    }

    public void CompleteTask3() {
        if (!IsBookRead) return;
        IsTask3Completed = true;
    }

    // ===== TIMER =====
    private void StartTaskTimer(float duration) {
        taskTimer = duration;
        timerActive = true;
        if (timerText != null) timerText.gameObject.SetActive(true);
    }

    public void StopTaskTimer() {
        timerActive = false;
        if (timerText != null) {
            timerText.text = "00:00";
            timerText.gameObject.SetActive(false);
        }
    }

    // ===== GAME OVER =====
    private void TriggerGameOver() {
        if (isGameOver) return;

        isGameOver = true;


        StopTaskTimer();

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(true);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);

        PlayerControlManager.Instance.LockControl();

        Debug.Log($"GAME OVER on Task {CurrentTaskIndex}");
    }




    // ===== CHEST AUTO REVEAL =====
    public void RevealBookAndUV() {
        if (bookObject != null)
            bookObject.ShowBook();

        if (uvLamp != null) {
            uvLamp.gameObject.SetActive(true);
            uvLamp.MakeAvailable();
        }
    }

    public void OnBookFound() {
        IsBookFound = true;
    }


    // ===== Task 4 Objects =====
    [Header("Task 4 Objects")]
    public GameObject libraryDoor;        // Scene library door
    public GameObject accessCard;         // Pickup
    public GameObject libraryBook;        // Pickup
    public GameObject ventEntry;          // Vent entrance
    public GameObject screwdriver;        // Pickup
    public GameObject fuseObject;         // Pickup
    public GameObject electricityBoard;   // PowerPanel
    public GameObject pcInteraction;      // PC
    private VentInteraction ventScript;
    private PowerPanel powerPanelScript;
    private PCInteractable pcScript;
    public bool HasCheckedPowerPanel = false;


    // ===== Task 4 State =====
    public bool IsTask4Started { get; private set; }
    public bool IsTask4Completed { get; private set; }

    public bool HasAccessCard { get; private set; }
    public bool LibraryUnlocked { get; private set; }
    public bool HasLibraryBook { get; private set; }
    public bool VentOpened { get; private set; }
    public bool ScrewdriverFound { get; private set; }
    public bool FuseFound { get; private set; }
    public bool ElectricityOn { get; private set; }
    public bool PcChecked { get; private set; }

    public bool IsLibraryFileInstalled { get; private set; }
    public bool IsPowerRestored { get; private set; }

    public void StartTask4() {
        StopTaskTimer();
        if (IsTask4Started) return;
        IsTask4Started = true;

        // Cache references
        ventScript = ventEntry?.GetComponent<VentInteraction>();
        powerPanelScript = electricityBoard?.GetComponent<PowerPanel>();
        pcScript = pcInteraction?.GetComponent<PCInteractable>();

        // Set visuals
        accessCard?.SetActive(true);
        libraryBook?.SetActive(false);
        screwdriver?.SetActive(false);
        fuseObject?.SetActive(false);

        ventEntry?.SetActive(true);
        electricityBoard?.SetActive(true);
        pcInteraction?.SetActive(true);

        // Lock library door at start
        if (libraryDoor != null) {
            var door = libraryDoor.GetComponent<opencloseDoor>();
            if (door != null) {
                door.isLocked = true;
                door.open = false;
            }
        }

        // DO NOT disable interactables here; gating will be handled inside scripts
        StartTaskTimer(task4Duration);
        UpdateCurrentTaskText("Go to Library.");
    }

    // ===== Access Card Pickup =====
    public void OnAccessCardPicked() {
        HasAccessCard = true;

        // Unlock door once
        if (libraryDoor != null) {
            var doorScript = libraryDoor.GetComponent<opencloseDoor>();
            if (doorScript != null) doorScript.isLocked = false;
        }

        // Consume the access card
        Inventory.Instance.RemoveItem("AccessCard");
        HasAccessCard = false;

        if (accessCard != null) Destroy(accessCard);

        // Reveal book
        if (libraryBook != null) libraryBook.SetActive(true);
        UpdateCurrentTaskText("Go to Library and Find Book");
    }

    // ===== Library Book Pickup =====
    public void OnLibraryBookFound() {
        HasLibraryBook = true;
        IsLibraryFileInstalled = true;

        // Trigger jumpscare first
        if (libraryBookJumpscare != null)
            libraryBookJumpscare.TriggerJumpscareDirect();


        // Close and lock library door with delay after jumpscare
        StartCoroutine(CloseAndLockLibraryDoor());
        // Vent becomes interactable
        if (ventScript != null) ventScript.enabled = true;

        // Screwdriver appears
        if (screwdriver != null) screwdriver.SetActive(true);
    }

    private IEnumerator CloseAndLockLibraryDoor() {
        // Wait 1 second to make it look like ghost closed the door
        yield return new WaitForSeconds(1f);

        if (libraryDoor == null) yield break;
        var doorScript = libraryDoor.GetComponent<opencloseDoor>();
        if (doorScript == null) yield break;

        doorScript.stateToPrint = 1;
        if (doorScript.open) doorScript.StartCoroutine("closing");
        yield return new WaitForSeconds(0.6f);

        UpdateCurrentTaskText("Find ScrewDriver to Open Vent");

        doorScript.isLocked = true;
        doorScript.open = false;
    }

    // ===== Screwdriver Pickup =====
    public void OnScrewdriverFound() {
        ScrewdriverFound = true;

        // Fuse appears
        if (fuseObject != null) fuseObject.SetActive(true);
        UpdateCurrentTaskText("Open Vent");
    }

    // ===== Vent Opened =====
    public void OnVentOpened() {
        if (!ScrewdriverFound) return;
        VentOpened = true;

        // Now PowerPanel can be used after vent exit
        if (powerPanelScript != null && pcScript != null) {
            powerPanelScript.enabled = true;
            pcScript.enabled = true;
        }

        // Fuse can now be picked
        if (fuseObject != null) fuseObject.SetActive(true);
        UpdateCurrentTaskText("Enter Vent");
    }


    // ===== Fuse Pickup =====
    public void OnFuseFound() {
        FuseFound = true;
        UpdateCurrentTaskText("Go to Power Panel");
    }

    // ===== Electricity Restored =====
    public void OnElectricityRestored() {
        if (!FuseFound) return;
        ElectricityOn = true;

        // PC stays always interactable; PowerPanel is now functional
        if (powerPanelScript != null) powerPanelScript.enabled = true;
        if (pcScript != null) pcScript.enabled = true;
    }

    // ===== PC Checked =====
    public void OnPcChecked() {
        PcChecked = true;

        // Task completes only if electricity restored
        if (ElectricityOn) CompleteTask4();
    }

    // ===== Complete Task 4 =====
    private void CompleteTask4() {
        if (IsTask4Completed) return;
        IsTask4Completed = true;
        StopTaskTimer();
        Debug.Log("Task 4 Complete!");
        TriggerGameFinish();
        

    }
    private void TriggerGameFinish() {
        CutsceneManager.Instance.PlayEndCutscene();
        Debug.Log("GAME FINISHED - Ending Cutscene will play here later.");
    }





    // ===== VENT EVENTS =====
    public void OnVentEntered() {
        UpdateCurrentTaskText("Press W to Move"); 
        Debug.Log("Task4: Vent entered"); }
    public void OnVentJumpscareTriggered() { Debug.Log("Task4: jumpscare"); }
    public void OnVentExited() {
        UpdateCurrentTaskText("Go to PC.");
        Debug.Log("Task4: exited vent"); }

    // ===== POWER PANEL EVENTS =====
    public void OnPowerPanelChecked() {
        HasCheckedPowerPanel = true;
        UpdateCurrentTaskText("Find Fuse"); 
        Debug.Log("Task4: panel checked"); }
    public void OnFuseInserted() {
        UpdateCurrentTaskText("Go to PC for Submission"); 
        Debug.Log("Task4: fuse inserted"); }

    // =======================================
    // ======== GAME MANAGEMENT API ==========
    // =======================================

    public int CurrentTaskIndex = 1;

    // ----- Retry Dispatcher -----
    public void RetryTask() {
        switch (CurrentTaskIndex) {
            case 1: RetryTask1(); break;
            case 2: RetryTask2(); break;
            case 3: RetryTask3(); break;
            case 4: RetryTask4(); break;
            default:
                Debug.LogWarning("Retry requested but no active task!");
                break;
        }
    }

    // =======================================
    // ======== TASK 1 RESET LOGIC ============
    // =======================================
    private void RetryTask1() {
        Debug.Log("Retrying Task 1");

        // ===== Reset Task 1 State =====
        IsTask1Started = false;
        IsTask1Completed = false;
        IsNoteRead = false;
        IsSemicolonPicked = false;

        // ===== Reset Objects =====

        if (noteObject != null)
            noteObject.HideNote();

        if (semicolonObject != null)
            semicolonObject.HideSemicolon();

        // ===== Reset Inventory =====
        Inventory.Instance.ClearInventory();

        // ===== Reset Timer =====
        StopTaskTimer();
        StartTaskTimer(task1Duration);

        // ===== Teleport Player =====
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && pcSpawnPoint != null) {
            player.transform.position = pcSpawnPoint.position;
            player.transform.rotation = pcSpawnPoint.rotation;
        }

        // ===== Restart Task 1 =====
        CurrentTaskIndex = 1;
        OnTask1Started();
    }


    // =======================================
    // ======== TASK 2 RESET LOGIC ============
    // =======================================
    private void RetryTask2() {
        Debug.Log("Retrying Task 2");

        // Reset state
        IsTask2Started = false;
        IsTask2Completed = false;
        IsVariablePicked = false;
        CorrectVariablePicked = false;
        IsTask2KeyPicked = false;

        // Reset objects
        foreach (var v in variableObjects)
            v.SetActive(false);

        if (task2Key != null) task2Key.gameObject.SetActive(false);

        if (officeDoor != null) {
            officeDoor.isLocked = false;
            officeDoor.open = false;
        }

        // Reset inventory
        Inventory.Instance.ClearInventory();

        // Timer restart
        StartTaskTimer(task2Duration);
        // Teleport back to PC spawn
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && pcSpawnPoint != null) {
            player.transform.position = pcSpawnPoint.position;
            player.transform.rotation = pcSpawnPoint.rotation;
        }

        CurrentTaskIndex = 2;
    }

    // =======================================
    // ======== TASK 3 RESET LOGIC ============
    // =======================================
    private void RetryTask3() {
        Debug.Log("Retrying Task 3");

        // Use your existing reset logic
        RetryTask3Internal();

        // Timer restart
        StartTaskTimer(task3Duration);

        CurrentTaskIndex = 3;
    }

    // Use existing function but renamed internally
    private void RetryTask3Internal() {
        if (secretChest != null) secretChest.ResetChest();
        if (bookObject != null) bookObject.HideBook();
        if (uvLamp != null) uvLamp.gameObject.SetActive(false);
        if (batteryPickup != null) batteryPickup.gameObject.SetActive(false);

        IsTask3Started = false;
        IsTask3Completed = false;
        IsChestFound = false;
        IsChestUnlocked = false;
        IsBookFound = false;
        IsUVFound = false;
        IsBatteryFound = false;
        IsBookRead = false;

        // Teleport back to PC spawn
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && pcSpawnPoint != null) {
            player.transform.position = pcSpawnPoint.position;
            player.transform.rotation = pcSpawnPoint.rotation;
        }

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        PlayerControlManager.Instance.UnlockControl();
        isGameOver = false;
    }

    // =======================================
    // ======== TASK 4 RESET LOGIC ============
    // =======================================
    private void RetryTask4() {
        Debug.Log("Retrying Task 4");

        // Reset state
        IsTask4Started = false;
        IsTask4Completed = false;
        HasAccessCard = false;
        LibraryUnlocked = false;
        HasLibraryBook = false;
        VentOpened = false;
        ScrewdriverFound = false;
        FuseFound = false;
        ElectricityOn = false;
        PcChecked = false;
        IsLibraryFileInstalled = false;
        IsPowerRestored = false;

        // Reset inventory
        Inventory.Instance.ClearInventory();

        // Reset objects
        if (accessCard != null) accessCard.SetActive(true);
        if (libraryBook != null) libraryBook.SetActive(false);
        if (screwdriver != null) screwdriver.SetActive(false);
        if (fuseObject != null) fuseObject.SetActive(false);

        if (ventEntry != null) ventEntry.SetActive(true);

        if (electricityBoard != null) electricityBoard.SetActive(true);

        if (pcInteraction != null) pcInteraction.SetActive(true);

        // Reset library door
        if (libraryDoor != null) {
            var door = libraryDoor.GetComponent<opencloseDoor>();
            if (door != null) {
                door.isLocked = true;
                door.open = false;
            }
        }
        // Teleport back to PC spawn
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && pcSpawnPoint != null) {
            player.transform.position = pcSpawnPoint.position;
            player.transform.rotation = pcSpawnPoint.rotation;
        }

        // Reset Timer
        StartTaskTimer(task4Duration);

        CurrentTaskIndex = 4;
    }
    public void OnRetryButtonPressed() {
        if (!isGameOver) return;

        Debug.Log($"Retry pressed for Task {CurrentTaskIndex}");

        // Hide Game Over UI
        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        // Reset game over state
        isGameOver = false;

        // Unlock player BEFORE resetting task
        PlayerControlManager.Instance.UnlockControl();

        // Retry ONLY the current task
        RetryTask();
    }

    public void OnQuitButtonPressed() {
        Debug.Log("Quit button pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


}

