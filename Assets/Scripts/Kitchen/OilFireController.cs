using System;
using System.Collections;
using UnityEngine;

public class OilFireController : MonoBehaviour
{
    [Header("Fire FX")]
    public ParticleSystem fireFX;
    public GameObject fireBurstPrefab;

    [Header("Fire Settings")]
    [Range(0f, 5f)] public float intensity = 1f;
    public float maxIntensity = 3f;

    [Header("Timing")]
    public float timeToDanger = 10f;
    public float timeToFail = 20f;

    [Header("Extinguish")]
    public float extinguishSpeed = 1.0f;

    private float timer = 0f;

    private bool isCovered = false;
    private bool isExtinguished = false;
    private bool isFailed = false;

    private void Update()
    {
        if (isExtinguished || isFailed) return;

        timer += Time.deltaTime;

        if (isCovered)
        {
            HandleCovered();
        }
        else
        {
            HandleBurning();
        }
    }

    private void HandleCovered()
    {
        intensity -= extinguishSpeed * Time.deltaTime;
        intensity = Mathf.Clamp(intensity, 0, maxIntensity);

        UpdateFireVisual();

        if (intensity <= 0.05f)
        {
            ExtinguishSuccess();
        }
    }

    private void HandleBurning()
    {
        intensity += 0.3f * Time.deltaTime;
        intensity = Mathf.Clamp(intensity, 0, maxIntensity);

        UpdateFireVisual();

        if (timer > timeToDanger)
        {
            EnterDangerState();
        }

        if (timer > timeToFail)
        {
            FailTraining("Too late - fire spread!");
        }
    }

    private void UpdateFireVisual()
    {
        if (fireFX == null) return;

        var main = fireFX.main;
        main.startSize = Mathf.Lerp(0.5f, 1f, intensity / maxIntensity);
    }

    private void EnterDangerState()
    {
        if (intensity < maxIntensity * 0.8f) return;

        // should have smoke
        Debug.Log("WARNING: Fire is getting dangerous!");
    }

    private void ExtinguishSuccess()
    {
        isExtinguished = true;

        fireFX.gameObject.SetActive(false);
        fireFX.Stop();

        var message = "Correctly extinguished oil fire!";
        Debug.Log("SUCCESS: Correctly extinguished oil fire!");
        GameController.Instance.Success(message);
    }

    private void FailTraining(string reason)
    {
        if (isFailed) return;
        GameController.Instance.Fail("FAIL" + reason);
        isFailed = true;
        Debug.Log("FAIL: " + reason);
    }

    // =============================
    // ===== WATER INTERACTION =====
    // =============================
    public void OnWaterPour()
    {
        if (isExtinguished || isFailed) return;

        Debug.Log("Water poured on oil fire!");

        StartCoroutine(FlareUp());
    }

    private IEnumerator FlareUp()
    {
        isFailed = true;

        intensity = maxIntensity;
        UpdateFireVisual();

        var main = fireFX.main;
        main.startSize = 3.5f;
        
        if (fireBurstPrefab != null)
        {
            Instantiate(fireBurstPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        
        GameController.Instance.Fail("FAIL Flare up");

        Debug.Log("FAIL: Water caused flare-up!");

        yield return null;
    }

    // =============================
    // ===== LID DETECTION =========
    // =============================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lid"))
        {
            isCovered = true;
            ExtinguishSuccess();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lid"))
        {
            isCovered = false;
        }
    }
}