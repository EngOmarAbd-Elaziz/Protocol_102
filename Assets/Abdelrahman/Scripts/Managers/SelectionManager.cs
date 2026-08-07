using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // Public :-


    // Private :-
    [SerializeField] private Camera mainCamera;
    //private IInteractable currentSelection;


    private void OnEnable()
    {
        GameInputManager.Instance.OnInteract += HandleSelection;
    }

    private void OnDisable()
    {
        GameInputManager.Instance.OnInteract -= HandleSelection;
    }




    private void HandleSelection()
    {
        if (SimulationManager.Instance.IsSimulationRunning)
        {
            return;
        }

        Vector2 mousePosition = GameInputManager.Instance.GetMousePosition();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out InteractableObject interactable))
            {
                PlanningManager.Instance.SelectObject(interactable);
            }
        }
    }
}
