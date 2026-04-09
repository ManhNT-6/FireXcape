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

    void Update()
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

    void HandleCovered()
    {
        intensity -= extinguishSpeed * Time.deltaTime;
        intensity = Mathf.Clamp(intensity, 0, maxIntensity);

        UpdateFireVisual();

        if (intensity <= 0.05f)
        {
            ExtinguishSuccess();
        }
    }

    void HandleBurning()
    {
        // Lửa mạnh dần theo thời gian
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

    void UpdateFireVisual()
    {
        if (fireFX == null) return;

        var main = fireFX.main;
        main.startSize = Mathf.Lerp(0.5f, 2.5f, intensity / maxIntensity);
    }

    void EnterDangerState()
    {
        // Chỉ log 1 lần (tránh spam)
        if (intensity < maxIntensity * 0.8f) return;

        Debug.Log("WARNING: Fire is getting dangerous!");
    }

    void ExtinguishSuccess()
    {
        isExtinguished = true;

        fireFX.Stop();

        Debug.Log("SUCCESS: Correctly extinguished oil fire!");
    }

    void FailTraining(string reason)
    {
        if (isFailed) return;

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

    IEnumerator FlareUp()
    {
        isFailed = true;

        // Bùng lửa mạnh
        intensity = maxIntensity;
        UpdateFireVisual();

        var main = fireFX.main;
        main.startSize = 3.5f;

        // Spawn hiệu ứng bùng
        if (fireBurstPrefab != null)
        {
            Instantiate(fireBurstPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Debug.Log("FAIL: Water caused flare-up!");

        yield return null;
    }

    // =============================
    // ===== LID DETECTION =========
    // =============================

    private void OnCollisionEnter(Collision other)
    {
        if  (other.gameObject.CompareTag("Lid"))
        {
            isCovered = true;
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