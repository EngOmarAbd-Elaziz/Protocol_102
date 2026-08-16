using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverHighlight : MonoBehaviour
{
    [Header("Hover Outline")]
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 0.02f;

    private Camera mainCamera;

    private bool isHovered;

    // We don't use this as the parent of the actual outline anymore.
    private GameObject outlineRoot;

    // Keep references to the ACTUAL outline objects
    private readonly List<GameObject> outlineObjects =
        new List<GameObject>();

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleHover();
    }

    private void HandleHover()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (Mouse.current == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                    StartHover();

                return;
            }
        }

        if (isHovered)
            StopHover();
    }

    private void StartHover()
    {
        isHovered = true;

        CreateOutline();

        // Enable the ACTUAL outline objects
        foreach (GameObject outline in outlineObjects)
        {
            if (outline != null)
                outline.SetActive(true);
        }
    }

    private void StopHover()
    {
        isHovered = false;

        // Disable the ACTUAL outline objects
        foreach (GameObject outline in outlineObjects)
        {
            if (outline != null)
                outline.SetActive(false);
        }
    }

    private void CreateOutline()
    {
        // Don't create it more than once
        if (outlineObjects.Count > 0)
            return;

        if (outlineMaterial == null)
        {
            Debug.LogError(
                $"{name}: Outline Material is missing!",
                this
            );

            return;
        }

        outlineRoot = new GameObject(
            $"{name}_HoverOutline"
        );

        outlineRoot.transform.SetParent(transform);

        outlineRoot.transform.localPosition =
            Vector3.zero;

        outlineRoot.transform.localRotation =
            Quaternion.identity;

        outlineRoot.transform.localScale =
            Vector3.one;

        MeshFilter[] meshFilters =
            GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter sourceMeshFilter in meshFilters)
        {
            if (sourceMeshFilter.sharedMesh == null)
                continue;

            // Don't create outlines for our own generated objects
            if (sourceMeshFilter.gameObject.name.Contains(
                "_Outline"))
                continue;

            CreateOutlineMesh(sourceMeshFilter);
        }

        // Root itself doesn't control the outlines anymore.
        // Keep it inactive/hidden.
        outlineRoot.SetActive(false);
    }

    private void CreateOutlineMesh(
        MeshFilter sourceMeshFilter)
    {
        GameObject outlineObject =
            new GameObject(
                $"{sourceMeshFilter.name}_Outline"
            );

        // IMPORTANT:
        // Keep the old parenting that made the paper work.
        outlineObject.transform.SetParent(
            sourceMeshFilter.transform
        );

        outlineObject.transform.localPosition =
            Vector3.zero;

        outlineObject.transform.localRotation =
            Quaternion.identity;

        outlineObject.transform.localScale =
            Vector3.one;

        MeshFilter outlineMesh =
            outlineObject.AddComponent<MeshFilter>();

        outlineMesh.sharedMesh =
            sourceMeshFilter.sharedMesh;

        MeshRenderer outlineRenderer =
            outlineObject.AddComponent<MeshRenderer>();

        Material outlineMat =
            new Material(outlineMaterial);

        outlineMat.SetColor(
            "_OutlineColor",
            outlineColor
        );

        outlineMat.SetFloat(
            "_OutlineWidth",
            outlineWidth
        );

        // Apply outline material to ALL submeshes
        int subMeshCount =
            sourceMeshFilter.sharedMesh.subMeshCount;

        Material[] outlineMaterials =
            new Material[subMeshCount];

        for (int i = 0; i < subMeshCount; i++)
        {
            outlineMaterials[i] = outlineMat;
        }

        outlineRenderer.sharedMaterials =
            outlineMaterials;

        // IMPORTANT:
        // Store the ACTUAL outline object
        // so we can disable it during unhover.
        outlineObjects.Add(outlineObject);

        // Start disabled
        outlineObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopHover();
    }

    private void OnDestroy()
    {
        if (outlineRoot != null)
            Destroy(outlineRoot);

        outlineObjects.Clear();
    }
}