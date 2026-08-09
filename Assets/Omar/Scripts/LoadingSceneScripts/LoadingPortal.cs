using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPortal : MonoBehaviour
{
    [SerializeField] private Image _fadingImage;
    [SerializeField] private float _fadeDuration = 0.6f;

    private CameraForwardMovement cameraMovement;

    private void Awake()
    {
        cameraMovement = FindFirstObjectByType<CameraForwardMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraMovement._isStopped = true;
            StartCoroutine(PortalTransition());
        }
    }

    private IEnumerator PortalTransition()
    {
        yield return new WaitForSeconds(1.5f);
        cameraMovement.StopMusic();

        yield return StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (_fadingImage == null)
        {
            Loader.LoadTargetScene();
            yield break;
        }

        _fadingImage.color = Color.clear;
        _fadingImage.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / _fadeDuration);

            Color color = Color.black;
            color.a = progress;
            _fadingImage.color = color;

            yield return null;
        }

        _fadingImage.color = Color.black;
        Loader.LoadTargetScene();
    }
}
