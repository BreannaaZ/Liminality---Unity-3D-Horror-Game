using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightsOutController : MonoBehaviour
{
    public Volume globalVolume;
    public float checkInterval = 5f;         
    public float duration = 3f;              
    public float noiseThreshold = 0.50f;    

    private ColorAdjustments colorAdjustments;
    private bool isDark = false;
    private float noiseSeed;

    public GameObject mannequin;
    public MannequinController mannequinController = null;
    public Animator animator; 

    void Start()
    {
        if (mannequin != null)
        {
            mannequinController = mannequin.GetComponent<MannequinController>();
        }

        if (animator != null)
        {
            animator.speed = 0f;
        }

        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            noiseSeed = Random.Range(0f, 1000f);
            StartCoroutine(LightsRoutine());
        }
        else
        {
            Debug.LogError("Fix global volume profile");
        }
    }

    IEnumerator LightsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            float noise = Mathf.PerlinNoise(Time.time, noiseSeed);
            if (noise > noiseThreshold && !isDark)
            {
                // Add flicker effect first
                SetExposure(-7f);
                yield return new WaitForSeconds(0.2f);
                SetExposure(0f);
                yield return new WaitForSeconds(0.4f);
                SetExposure(-8f);
                yield return new WaitForSeconds(0.2f);
                SetExposure(0f);
                yield return new WaitForSeconds(0.4f);

                // Now turn lights off for set time
                SetExposure(-10f);
                isDark = true;
                if (mannequinController != null) {
                    mannequinController.lightsOut = true;
                }
                if (animator != null)
                {
                    animator.speed = 0.1f; // Move slightly on lights off
                }

                yield return new WaitForSeconds(duration);

                SetExposure(0f);
                isDark = false;
                if (mannequinController != null)
                {
                    mannequinController.lightsOut = false;
                }
                if (animator != null)
                {
                    animator.speed = 0f; // Freeze on lights on
                }
            }
        }
    }

    void SetExposure(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.Override(value);
        }
    }
}
