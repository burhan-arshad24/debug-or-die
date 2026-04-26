using System.Collections;
using TMPro;
using UnityEngine;
using SojaExiles;

[System.Serializable]
public class TimedText
{
    [TextArea(3, 10)]
    public string text;

    [Min(0.1f)]
    public float displayTime = 3f;
}

public class CutsceneManager : MonoBehaviour
{
    [Header("DEBUG")]
    public bool skipCutscene = false;

    [Header("UI")]
    public Canvas storyCanvas;
    public TMP_Text cutsceneText;
    public CanvasGroup fadeGroup;

    [Header("Player")]
    public Transform player;
    public Transform cameraTransform;
    public float moveSpeed = 2f;

    [Header("Interaction")]
    public static bool IsCutsceneActive;

    [Header("Waypoints")]
    public WaypointData[] waypoints;

    [Header("Lighting")]
    public LightingController lightingController;

    [Header("Timings")]
    public float fadeDuration = 1.5f;

    [Header("References")]
    public MouseLook mouseLook;

    [Header("Voice Over")]
    public AudioClip openingVoice;
    public AudioClip tiredVoice;
    public AudioClip lightsOffVoice;

    [Range(0.5f, 2f)]
    public float voiceSpeed = 1f;

    private AudioSource voiceSource;

    private Quaternion originalCameraLocalRotation;
    private Coroutine cutsceneRoutine;
    private bool isCutsceneRunning = false;
    public static CutsceneManager Instance;

    [Header("Opening Text Sequence")]
    public TimedText[] walkingStoryText;

    [Header("Tired Text Sequence")]
    public TimedText[] tiredText;

    [Header("Lights Off Text")]
    public TimedText[] lightsOffText;

    private void Awake()
    {
        Instance = this;

        if (cameraTransform != null)
            originalCameraLocalRotation = cameraTransform.localRotation;

        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.loop = false;
        voiceSource.playOnAwake = false;
        voiceSource.spatialBlend = 0f;
        voiceSource.volume = 0.8f;
    }

    private void Update()
    {
        if (isCutsceneRunning && Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutsceneInstant();
        }
    }

    public void PlayOpeningCutscene()
    {
        if (isCutsceneRunning) return;

        if (skipCutscene)
        {
            SkipCutsceneInstant();
            return;
        }

        cutsceneRoutine = StartCoroutine(CutsceneFlow());
    }

    public IEnumerator CutsceneFlow()
    {
        TaskManager.Instance.StopTaskTimer();
        isCutsceneRunning = true;
        IsCutsceneActive = true;

        PlayerControlManager.Instance.LockControl();
        DisableMouseLook();

        PlayVoice(openingVoice);

        storyCanvas.gameObject.SetActive(true);
        fadeGroup.alpha = 1f;
        cutsceneText.text = "";

        yield return Fade(1f, 0f);

        Coroutine walkingTextRoutine = StartCoroutine(PlayTimedText(walkingStoryText));

        foreach (var wp in waypoints)
        {
            yield return MoveTo(wp.point);

            if (wp.doorToOpen != null && wp.doorToOpen.gameObject.activeInHierarchy)
            {
                wp.doorToOpen.StartCoroutine("opening");
                yield return new WaitForSeconds(0.6f);
            }

            if (wp.waitAfter > 0)
                yield return new WaitForSeconds(wp.waitAfter);
        }

        if (walkingTextRoutine != null)
            yield return walkingTextRoutine;

        // Desk Phase
        yield return Fade(0f, 1f);
        cutsceneText.text = "3 hours later...";
        yield return new WaitForSeconds(1.5f);
        yield return Fade(1f, 0f);

        PlayVoice(tiredVoice);
        yield return PlayTimedText(tiredText);

        // Sleep
        if (cameraTransform != null)
            yield return TiltHeadDown(45f, 1.5f);

        yield return new WaitForSeconds(0.5f);
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(1f);

        // Horror Wake
        lightingController.SwitchToHorror();
        PlayVoice(lightsOffVoice);

        yield return Fade(1f, 0f);
        yield return PlayTimedText(lightsOffText);

        EndCutscene();
    }

    private IEnumerator PlayTimedText(TimedText[] sequence)
    {
        if (sequence == null || sequence.Length == 0)
            yield break;

        foreach (var entry in sequence)
        {
            if (!isCutsceneRunning) yield break;  // 🔥 HARD STOP

            cutsceneText.text = entry.text;
            yield return new WaitForSeconds(entry.displayTime);
        }
    }


    private IEnumerator TiltHeadDown(float targetAngle, float duration)
    {
        if (cameraTransform == null) yield break;

        Quaternion startRotation = cameraTransform.localRotation;
        Quaternion targetRotation =
            originalCameraLocalRotation * Quaternion.Euler(targetAngle, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cameraTransform.localRotation =
                Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        cameraTransform.localRotation = targetRotation;
    }

    private void EndCutscene()
    {
        if (!isCutsceneRunning) return;

        StopVoice();

        if (cutsceneRoutine != null)
            StopCoroutine(cutsceneRoutine);

        isCutsceneRunning = false;
        IsCutsceneActive = false;

        storyCanvas.gameObject.SetActive(false);
        fadeGroup.alpha = 0f;

        ResetCamera();
        EnableMouseLook();
        PlayerControlManager.Instance.UnlockControl();

        if (TaskManager.Instance != null)
            TaskManager.Instance.StartTask0();
    }

    private void SkipCutsceneInstant()
    {
        if (!isCutsceneRunning) return;

        isCutsceneRunning = false;   // 🔥 STOP EVERYTHING FIRST
        IsCutsceneActive = false;

        StopAllCoroutines();         // 🔥 KILL EVERYTHING IMMEDIATELY

        StopVoice();

        if (waypoints != null && waypoints.Length > 0)
        {
            Transform last = waypoints[waypoints.Length - 1].point;
            player.position = last.position;
            player.rotation = Quaternion.Euler(0f, last.eulerAngles.y, 0f);
        }

        lightingController.SwitchToHorror();

        storyCanvas.gameObject.SetActive(false);
        fadeGroup.alpha = 0f;

        ResetCamera();
        EnableMouseLook();
        PlayerControlManager.Instance.UnlockControl();

        TaskManager.Instance.StartTask0();
    }


    private void ResetCamera()
    {
        if (cameraTransform != null)
            cameraTransform.localRotation =
                Quaternion.Euler(originalCameraLocalRotation.eulerAngles.x, 0f, 0f);
    }

    private IEnumerator MoveTo(Transform target)
    {
        float rotationSpeed = 2f;
        float distanceThreshold = 0.05f;

        while (Vector3.Distance(player.position, target.position) > distanceThreshold)
        {
            if (!isCutsceneRunning) yield break;   // 🔥 HARD STOP

            player.position = Vector3.MoveTowards(
                player.position,
                target.position,
                moveSpeed * 0.5f * Time.deltaTime);

            Quaternion targetRotation =
                Quaternion.Euler(0f, target.eulerAngles.y, 0f);

            player.rotation =
                Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        player.position = target.position;
        player.rotation =
            Quaternion.Euler(0f, target.eulerAngles.y, 0f);
    }


    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        fadeGroup.alpha = to;
    }

    private void DisableMouseLook()
    {
        if (mouseLook != null)
            mouseLook.overrideInput = true;
    }

    private void EnableMouseLook()
    {
        if (mouseLook != null)
            mouseLook.overrideInput = false;
    }

    private void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;

        voiceSource.Stop();
        voiceSource.pitch = voiceSpeed;
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    private void StopVoice()
    {
        if (voiceSource.isPlaying)
            voiceSource.Stop();
    }
    public void PlayEndCutscene()
    {
        if (isCutsceneRunning) return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        cutsceneRoutine = StartCoroutine(EndGameCutsceneFlow());
    }

    [Header("Credits")]
    public GameObject creditsCanvas;


    private IEnumerator EndGameCutsceneFlow()
    {
        isCutsceneRunning = true;
        IsCutsceneActive = true;

        PlayerControlManager.Instance.LockControl();
        DisableMouseLook();

        storyCanvas.gameObject.SetActive(true);
        fadeGroup.alpha = 1f;

        // Fade in from black
        yield return Fade(1f, 0f);

        yield return new WaitForSeconds(0.5f);

        // Teleport player to last waypoint
        if (waypoints != null && waypoints.Length > 0)
        {
            Transform last = waypoints[waypoints.Length - 1].point;
            player.position = last.position;
            player.rotation = Quaternion.Euler(0f, last.eulerAngles.y, 0f);
        }

        // 🔥 VERY IMPORTANT (restore lights)
        if (lightingController != null)
            lightingController.SwitchToNormal();

        yield return new WaitForSeconds(0.5f);

        cutsceneText.text =
            "Shukar hai ye sapna tha, Powernap le k Kam az kam Errors to pta chal gy,\nAb assignment Complete kr k submit kar do ga";

        yield return new WaitForSeconds(5f);

        storyCanvas.gameObject.SetActive(false);
        fadeGroup.alpha = 0f;

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(true);
        }

        ResetCamera();
        EnableMouseLook();
        PlayerControlManager.Instance.UnlockControl();

        isCutsceneRunning = false;
        IsCutsceneActive = false;
    }

}
