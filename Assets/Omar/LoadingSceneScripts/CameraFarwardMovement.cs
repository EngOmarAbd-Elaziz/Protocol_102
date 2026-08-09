using UnityEngine;

public class CameraFarwardMovement : MonoBehaviour
{
    public static CameraFarwardMovement Instance { get; private set; }
    [SerializeField] private float _moveSpeed = 15f;

    public bool _isStopped = false;

    [Header("Loading Music")]
    [SerializeField] private AudioSource _musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (_musicSource != null)
        {
            _musicSource.Play();
        }
    }

    void Update()
    {
        if (_isStopped)
            return;

        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
        }
    }
}
