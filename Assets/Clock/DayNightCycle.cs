using System;
using UnityEngine;
using UnityEngine.Events;

public enum DayNightCycleType
{
    Morning, // Pagi / Siang
    Night    // Malam
}

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Mode")]
    [Tooltip("Current active cycle: Morning (Pagi) or Night (Malam).")]
    [SerializeField] private DayNightCycleType currentCycle = DayNightCycleType.Morning;

    [Tooltip("If true, automatically toggles between Morning and Night on a timer.")]
    [SerializeField] private bool autoCycle = true;

    [Tooltip("Duration of each cycle (Morning/Night) in seconds when autoCycle is true.")]
    [SerializeField] private float cycleDurationSeconds = 10f;

    [Header("Clock GameObjects (Static / Locked Hands)")]
    [Tooltip("GameObject Clock for Morning / Siang.")]
    [SerializeField] private GameObject clockMorning;

    [Tooltip("GameObject Clock for Night / Malam.")]
    [SerializeField] private GameObject clockNight;

    [Header("2D Lighting (URP Light2D)")]
    [Tooltip("Global Light 2D to adjust between morning and night.")]
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight2D;
    [SerializeField] private bool controlGlobalLight = true;
    [SerializeField] private Color morningLightColor = new Color(1f, 0.98f, 0.9f, 1f);
    [SerializeField] private Color nightLightColor = new Color(0.25f, 0.35f, 0.6f, 1f);
    [SerializeField] private float morningIntensity = 1f;
    [SerializeField] private float nightIntensity = 0.35f;

    [Header("Events")]
    public UnityEvent<DayNightCycleType> onCycleChanged;
    public UnityEvent<string> onCycleNameChanged; // "Pagi" or "Malam"

    public DayNightCycleType CurrentCycle => currentCycle;
    public bool IsMorning => currentCycle == DayNightCycleType.Morning;
    public bool IsNight => currentCycle == DayNightCycleType.Night;

    private float timer = 0f;
    private DayNightCycleType lastAppliedCycle = (DayNightCycleType)(-1);

    private void Awake()
    {
        FindReferencesIfMissing();
        ApplyCycle(currentCycle, true);
    }

    private void OnValidate()
    {
        FindReferencesIfMissing();
        ApplyCycle(currentCycle, true);
    }

    private void Start()
    {
        ApplyCycle(currentCycle, true);
    }

    private void Update()
    {
        if (Application.isPlaying && autoCycle && cycleDurationSeconds > 0f)
        {
            timer += Time.deltaTime;
            if (timer >= cycleDurationSeconds)
            {
                timer = 0f;
                // Toggle cycle
                DayNightCycleType nextCycle = (currentCycle == DayNightCycleType.Morning) 
                    ? DayNightCycleType.Night 
                    : DayNightCycleType.Morning;
                SetCycle(nextCycle);
            }
        }
    }

    public void SetCycle(DayNightCycleType cycle)
    {
        currentCycle = cycle;
        timer = 0f;
        ApplyCycle(currentCycle);
    }

    public void SetMorning()
    {
        SetCycle(DayNightCycleType.Morning);
    }

    public void SetNight()
    {
        SetCycle(DayNightCycleType.Night);
    }

    public void ToggleCycle()
    {
        SetCycle(currentCycle == DayNightCycleType.Morning ? DayNightCycleType.Night : DayNightCycleType.Morning);
    }

    private void ApplyCycle(DayNightCycleType cycle, bool force = false)
    {
        bool isMorning = (cycle == DayNightCycleType.Morning);

        // Toggle Clock GameObjects directly without touching hands rotation
        if (clockMorning != null)
        {
            clockMorning.SetActive(isMorning);
        }

        if (clockNight != null)
        {
            clockNight.SetActive(!isMorning);
        }

        // Apply Global Light
        if (controlGlobalLight && globalLight2D != null)
        {
            globalLight2D.color = isMorning ? morningLightColor : nightLightColor;
            globalLight2D.intensity = isMorning ? morningIntensity : nightIntensity;
        }

        if (force || cycle != lastAppliedCycle)
        {
            lastAppliedCycle = cycle;
            onCycleChanged?.Invoke(cycle);
            onCycleNameChanged?.Invoke(isMorning ? "Pagi" : "Malam");
        }
    }

    private void FindReferencesIfMissing()
    {
        if (clockMorning == null || clockNight == null)
        {
            var allTransforms = FindObjectsByType<Transform>(FindObjectsInactive.Include);
            foreach (var t in allTransforms)
            {
                if (clockMorning == null && t.gameObject.name == "ClockMorning")
                {
                    clockMorning = t.gameObject;
                }
                if (clockNight == null && t.gameObject.name == "ClockNight")
                {
                    clockNight = t.gameObject;
                }
            }
        }

        if (globalLight2D == null)
        {
            globalLight2D = FindAnyObjectByType<UnityEngine.Rendering.Universal.Light2D>();
        }
    }

    [ContextMenu("Switch to Morning (Pagi)")]
    private void ContextMenuMorning()
    {
        SetMorning();
    }

    [ContextMenu("Switch to Night (Malam)")]
    private void ContextMenuNight()
    {
        SetNight();
    }
}
