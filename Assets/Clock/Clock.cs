using System;
using UnityEngine;

[ExecuteAlways]
public class Clock : MonoBehaviour
{
    [Header("Lock Hands (Stay Static)")]
    [Tooltip("If true, clock hands remain static at their scene/inspector rotation and will not be overridden by code.")]
    [SerializeField] private bool lockHands = false;

    [Header("Clock Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    [Header("Clock Display Time (Fixed / Static)")]
    [Range(0, 23)] [SerializeField] private int hour = 8;
    [Range(0, 59)] [SerializeField] private int minute = 0;
    [Range(0, 59)] [SerializeField] private float second = 0f;

    [Tooltip("If true, hands interpolate smoothly between hours/minutes.")]
    [SerializeField] private bool continuous = true;

    // Sprite orientation offsets (since hour/minute sprites point down (towards 6) and red points up (towards 12))
    private const float HourBaseOffset = 180f;
    private const float MinuteBaseOffset = 180f;
    private const float SecondBaseOffset = 0f;

    private const float DegreesPerHour = 360f / 12f;     // 30 degrees
    private const float DegreesPerMinute = 360f / 60f;   // 6 degrees
    private const float DegreesPerSecond = 360f / 60f;   // 6 degrees

    public bool LockHands
    {
        get => lockHands;
        set => lockHands = value;
    }

    public int Hour => hour;
    public int Minute => minute;
    public float Second => second;

    private void Awake()
    {
        if (!lockHands)
        {
            AutoFindHandsIfMissing();
            ApplyTime();
        }
    }

    private void OnValidate()
    {
        if (!lockHands)
        {
            AutoFindHandsIfMissing();
            ApplyTime();
        }
    }

    private void Start()
    {
        if (!lockHands)
        {
            ApplyTime();
        }
    }

    private void AutoFindHandsIfMissing()
    {
        if (hourHand == null)
        {
            Transform t = transform.Find("alarm hour");
            if (t != null) hourHand = t;
        }

        if (minuteHand == null)
        {
            Transform t = transform.Find("alarm minute");
            if (t != null) minuteHand = t;
        }

        if (secondHand == null)
        {
            Transform t = transform.Find("alarm red");
            if (t != null) secondHand = t;
        }
    }

    public void SetTime(int newHour, int newMinute, float newSecond = 0f)
    {
        if (lockHands) return;

        hour = Mathf.Clamp(newHour, 0, 23);
        minute = Mathf.Clamp(newMinute, 0, 59);
        second = Mathf.Clamp(newSecond, 0f, 59.99f);
        ApplyTime();
    }

    public void ApplyTime()
    {
        if (lockHands) return;

        float h = hour % 12;
        float m = minute;
        float s = second;

        if (continuous)
        {
            m += s / 60f;
            h += m / 60f;
        }

        if (hourHand != null)
        {
            float hourRotation = HourBaseOffset - (h * DegreesPerHour);
            hourHand.localRotation = Quaternion.Euler(0f, 0f, hourRotation);
        }

        if (minuteHand != null)
        {
            float minuteRotation = MinuteBaseOffset - (m * DegreesPerMinute);
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, minuteRotation);
        }

        if (secondHand != null)
        {
            float secondRotation = SecondBaseOffset - (s * DegreesPerSecond);
            secondHand.localRotation = Quaternion.Euler(0f, 0f, secondRotation);
        }
    }
}


