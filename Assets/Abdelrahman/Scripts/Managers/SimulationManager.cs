using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [SerializeField] private List<ReactionData> reactionData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    public void StartSimulation()
    {
        StartCoroutine(RunSimulation());
    }

    private IEnumerator RunSimulation()
    {
        List<PlannedAction> actions = PlanningManager.Instance.GetPlannedActions();
        foreach (PlannedAction action in actions)
        {
            ExecuteAction(action);
            yield return new WaitForSeconds(0.5f); 
        }

        Debug.Log("Simulation started.");
    }

    private void ExecuteAction(PlannedAction action)
    {
        Debug.Log($"Executing action: {action.Source.InteractableType} -> " +
                                    $"{action.Target.InteractableType}");
    }

    private ReactionType GetReactionData(InteractableType source, InteractableType target)
    {
        foreach (ReactionData data in reactionData)
        {
            if (data.sourceType == source && data.targetType == target)
            {
                return data.reactionType;
            }
        }
        return ReactionType.None;
    }

    public void ClickToStart() 
    {
        StartSimulation();
    }

}
