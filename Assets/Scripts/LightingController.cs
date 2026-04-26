using UnityEngine;
using UnityEngine.Rendering;

public class LightingController : MonoBehaviour {
    [Header("Lighting Objects")]
    public GameObject normalLighting;
    public GameObject horrorLighting;

    [Header("Volumes")]
    public Volume normalVolume;   // Assign your Normal Volume here
    public Volume horrorVolume;   // Assign your Horror Volume here

    [Header("Ambient Colors")]
    public Color normalAmbientColor = new Color(0.35f, 0.35f, 0.35f);
    public Color horrorAmbientColor = Color.black;

    [Header("Fog Settings")]
    public bool useFogInHorror = true;
    public Color horrorFogColor = Color.black;
    public float horrorFogDensity = 0.02f;

    void Start() {
        SwitchToNormal(); // Start in normal mode
    }

    void Update() {
        // Testing keys (can remove later)
        if (Input.GetKeyDown(KeyCode.F1)) SwitchToNormal();
        if (Input.GetKeyDown(KeyCode.F2)) SwitchToHorror();
    }

    public void SwitchToNormal() {
        // Lighting
        normalLighting.SetActive(true);
        horrorLighting.SetActive(false);

        // Ambient
        RenderSettings.ambientLight = normalAmbientColor;

        // Fog
        RenderSettings.fog = false;

        // Volumes
        if (normalVolume != null) normalVolume.enabled = true;
        if (horrorVolume != null) horrorVolume.enabled = false;
    }

    public void SwitchToHorror() {
        // Lighting
        normalLighting.SetActive(false);
        horrorLighting.SetActive(true);

        // Ambient
        RenderSettings.ambientLight = horrorAmbientColor;

        // Fog
        if (useFogInHorror) {
            RenderSettings.fog = true;
            RenderSettings.fogColor = horrorFogColor;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = horrorFogDensity;
        }

        // Volumes
        if (normalVolume != null) normalVolume.enabled = false;
        if (horrorVolume != null) horrorVolume.enabled = true;
    }
}
