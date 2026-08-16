
using System.Collections;
using UnityEngine;

public class Alarm : MonoBehaviour
{

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


    public void ActivateAlarm()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;
        alarmLight.SetActive(true);
    }

    public void ResetAlarm()
    {
        isActivated = false;
        alarmLight.SetActive(false);
    }

}
