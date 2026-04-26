using System.Collections;
using UnityEngine;

public class HorrorEventManager : MonoBehaviour
{
    public float minInterval = 10f;
    public float maxInterval = 30f;

    private void Start()
    {
        StartCoroutine(RandomHorrorEvents());
    }

    private IEnumerator RandomHorrorEvents()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Skip if cutscene is active
            if (CutsceneManager.IsCutsceneActive)
                continue;

            // Pick random event
            int choice = Random.Range(0, 2); // 0 = laugh, 1 = knock
            switch (choice)
            {
                case 0:
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.horrorLaughClip);
                    break;
                case 1:
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.doorKnockClip);
                    break;
            }
        }
    }
}
