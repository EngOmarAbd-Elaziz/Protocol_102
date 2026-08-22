using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanningManager : MonoBehaviour
{
    // Public :-
    public static PlanningManager Instance { get; private set; }

    // Private :-
    private List<PlannedAction> plannedActions = new List<PlannedAction>();
    private InteractableObject firstSelection;

    [Header("Selection Feedback")]
    [SerializeField] private float actionFeedbackDuration = 0.15f;

    private bool isProcessingAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectObject(InteractableObject interactable)
    {
        if (interactable == null)
        {
            return;
        }

        // Prevent another click while the current action
        // is showing its visual feedback.
        if (isProcessingAction)
        {
            return;
        }

        // -----------------------------------------
        // First Selection
        // -----------------------------------------

        if (firstSelection == null)
        {
            firstSelection = interactable;

            firstSelection.Select();

            Debug.Log($"Selected Source: {firstSelection.name}");

            return;
        }

        // -----------------------------------------
        // Same Object
        // -----------------------------------------

        if (firstSelection == interactable)
        {
            firstSelection.Deselect();

            firstSelection = null;

            Debug.Log($"Selection cancelled: {interactable.name}");

            return;
        }

        // -----------------------------------------
        // Second Selection
        // -----------------------------------------

        StartCoroutine(CompleteAction(interactable));
    }


    private IEnumerator CompleteAction(InteractableObject target)
    {
        isProcessingAction = true;


        InteractableObject source = firstSelection;


        // Target gets selected visually.
        target.Select();


        // Wait so the player can see:
        // Source = normal
        // Target = selected
        yield return new WaitForSeconds(actionFeedbackDuration);


        // Create and store the action.
        PlannedAction newAction =
            new PlannedAction(source, target);

        plannedActions.Add(newAction);

        Debug.Log(
            $"Added Action: " +
            $"{source.name} → {target.name}"
        );


        // Clear visual selection.
        source.Deselect();
        target.Deselect();


        // Reset current selection.
        firstSelection = null;

        isProcessingAction = false;
    }


    public List<PlannedAction> GetPlannedActions()
    {
        return plannedActions;
    }


    public void ClearActions()
    {
        plannedActions.Clear();

        ClearSelection();

        Debug.Log("Planned actions cleared.");
    }


    public void ClearSelection()
    {
        if (firstSelection != null)
        {
            firstSelection.Deselect();

            firstSelection = null;
        }

        isProcessingAction = false;
    }
}