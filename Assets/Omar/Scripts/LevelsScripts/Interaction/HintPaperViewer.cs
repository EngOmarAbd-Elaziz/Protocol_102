using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class HintPaperViewer : MonoBehaviour
{
    [Header("Open Settings")]
    [SerializeField] private float moveDuration = 0.4f;
    [SerializeField] private Vector3 rotationOffset;

    [Header("Close Button")]
    [SerializeField] private GameObject closeButton;

    private Camera mainCamera;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isOpen;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        mainCamera = Camera.main;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (closeButton != null)
            closeButton.SetActive(false);
    }

    private void Update()
    {
        if (isOpen)
            return;

        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        HandleClick();
    }

    private void HandleClick()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                OpenPaper();
            }
        }
    }

    public void OpenPaper()
    {
        if (isOpen)
            return;

        isOpen = true;

        Vector3 targetPosition = new Vector3(
            3.81f,
            1.27f,
            -9.04f
        );

        Quaternion targetRotation =
            mainCamera.transform.rotation;

        targetRotation *=
            Quaternion.Euler(rotationOffset);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(
            MovePaper(
                targetPosition,
                targetRotation
            )
        );

        if (closeButton != null)
            closeButton.SetActive(true);
    }

    public void ClosePaper()
    {
        if (!isOpen)
            return;

        isOpen = false;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine =
            StartCoroutine(
                MovePaper(
                    originalPosition,
                    originalRotation
                )
            );

        if (closeButton != null)
            closeButton.SetActive(false);
    }

    private IEnumerator MovePaper(
        Vector3 targetPosition,
        Quaternion targetRotation)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / moveDuration
            );

            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            yield return null;
        }

        transform.rotation = targetRotation;
        Debug.Log(
    $"Paper World Position = {transform.position}"
);
    }
}
