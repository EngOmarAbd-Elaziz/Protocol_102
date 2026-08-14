using System;
using System.Collections;
using UnityEngine;

public class Alarm : MonoBehaviour
{

    public static event Action OnAlarmActivated;
    public static Alarm Instance { get; private set; }

    [SerializeField] private float activationDelay = 1f;
    [SerializeField] private GameObject alarmLight;
    private bool isActivated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        alarmLight.SetActive(false);
    }

    private void OnEnable()
    {
        Burner.OnBurnerActivated += HandleBurnerActivated;
    }

    private void OnDisable()
    {
        Burner.OnBurnerActivated -= HandleBurnerActivated;
    }

    public void HandleBurnerActivated()
    {
        StartCoroutine(ActivateAlarmDelay());
    }

    IEnumerator ActivateAlarmDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        ActivateAlarm();
    }

    private void ActivateAlarm()
    {

        if (isActivated)
        {
            return;
        }

        isActivated = true;
        alarmLight.SetActive(true);

        OnAlarmActivated?.Invoke();
    }

    public void ResetAlarm()
    {
        isActivated = false;
        alarmLight.SetActive(false);
    }

}
