using UnityEngine;
using System.Collections.Generic;

public class PlanningManager : MonoBehaviour
{
    // Public :-
    public static PlanningManager Instance { get; private set; }

    // Private :-

    // علشان محدش يغير الحاجات اللي جوا الليست من برا
    //private IReadOnlyList<PlannedAction> plannedActions = new List<PlannedAction>();
    private List<PlannedAction> plannedActions = new List<PlannedAction>();
    private InteractableObject firstSelection;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SelectObject(InteractableObject interactable)
    {
        if (firstSelection == null)
        {
            firstSelection = interactable;
            firstSelection.Select();
            return;
        }

        if (firstSelection == interactable)
        {
            firstSelection.Deselect();
            firstSelection = null;
            return;
        }

        plannedActions.Add(new PlannedAction(firstSelection, interactable));
        Debug.Log($"Add item {firstSelection.name} to item {interactable.name}");

        firstSelection.Deselect();
        interactable.Deselect();

        firstSelection = null;
    }

    public List<PlannedAction> GetPlannedActions()
    {
        return plannedActions;
    }
}
