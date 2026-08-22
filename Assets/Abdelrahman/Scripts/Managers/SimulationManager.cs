using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [Header("Required Sequence")]
    [SerializeField] private List<ReactionData> reactionData;

    [Space(30)]

    [Header("Interactable Objects")]
    [SerializeField] private InteractableObject[] interactableObjects;

    [Space(10)]

    [Header("Alarm")]
    [SerializeField] private Alarm alarm;

    private bool isSimulationRunning;
    public bool IsSimulationRunning => isSimulationRunning;

    private bool simulationFailed;

    public event Action OnSimulationFinished;
    public event Action OnSimulationFailed;


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


    // Starts the simulation after validating the complete sequence.
    public void StartSimulation()
    {
        if (isSimulationRunning)
        {
            Debug.LogWarning("Simulation is already running.");
            return;
        }

        List<PlannedAction> actions =
            PlanningManager.Instance.GetPlannedActions();

        // Make sure the player planned at least one action.
        if (actions.Count == 0)
        {
            Debug.LogWarning("No planned actions to simulate.");

            simulationFailed = true;
            OnSimulationFailed?.Invoke();

            return;
        }

        // Validate the complete sequence before starting the simulation.
        if (!ValidateSequence())
        {
            Debug.LogWarning("Simulation failed: Wrong sequence.");

            simulationFailed = true;
            OnSimulationFailed?.Invoke();

            return;
        }

        StartCoroutine(RunSimulation());
    }


    // Checks whether the player's planned actions
    // match the complete required sequence of the current level.
    private bool ValidateSequence()
    {
        List<PlannedAction> plannedActions =
            PlanningManager.Instance.GetPlannedActions();

        // Check the number of actions first.
        if (plannedActions.Count != reactionData.Count)
        {
            Debug.LogWarning(
                $"Wrong sequence length. " +
                $"Expected: {reactionData.Count} actions, " +
                $"Received: {plannedActions.Count}."
            );

            return false;
        }

        // Check every action against the required sequence
        // using the exact same order.
        for (int i = 0; i < reactionData.Count; i++)
        {
            ReactionData expectedReaction = reactionData[i];
            PlannedAction plannedAction = plannedActions[i];

            if (plannedAction.Source == null ||
                plannedAction.Target == null)
            {
                Debug.LogWarning(
                    $"Invalid PlannedAction at step {i}: " +
                    "Source or Target is null."
                );

                return false;
            }

            InteractableType actualSource =
                plannedAction.Source.InteractableType;

            InteractableType actualTarget =
                plannedAction.Target.InteractableType;

            // Compare the player's action with the required action.
            if (actualSource != expectedReaction.sourceType ||
                actualTarget != expectedReaction.targetType)
            {
                Debug.LogWarning(
                    $"Wrong sequence at step {i}. " +
                    $"Expected: {expectedReaction.sourceType} -> {expectedReaction.targetType}, " +
                    $"Received: {actualSource} -> {actualTarget}."
                );

                return false;
            }
        }

        Debug.Log("Complete sequence validated successfully.");

        return true;
    }


    private IEnumerator RunSimulation()
    {
        simulationFailed = false;
        isSimulationRunning = true;

        List<PlannedAction> actions =
            PlanningManager.Instance.GetPlannedActions();

        // Execute every planned action in order.
        for (int i = 0; i < actions.Count; i++)
        {
            ExecuteAction(actions[i], reactionData[i]);

            // Stop immediately if something goes wrong.
            if (simulationFailed)
            {
                isSimulationRunning = false;

                Debug.LogWarning("Simulation Failed.");

                OnSimulationFailed?.Invoke();

                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }

        // Clear the player's planned actions after a successful simulation.
        PlanningManager.Instance.ClearActions();

        isSimulationRunning = false;

        Debug.Log("Simulation Finished.");

        OnSimulationFinished?.Invoke();
    }


    // Executes a specific action using the ReactionData
    // assigned to the same sequence index.
    private void ExecuteAction(
        PlannedAction action,
        ReactionData expectedReaction)
    {
        if (action.Source == null || action.Target == null)
        {
            simulationFailed = true;

            Debug.LogWarning(
                "Simulation failed: Source or Target is null."
            );

            return;
        }

        if (expectedReaction == null)
        {
            simulationFailed = true;

            Debug.LogWarning(
                "Simulation failed: ReactionData is null."
            );

            return;
        }

        Debug.Log(
            $"Executing Step: " +
            $"{expectedReaction.sourceType} -> " +
            $"{expectedReaction.targetType} " +
            $"= {expectedReaction.reactionType}"
        );

        // Apply the reaction that belongs to this sequence step.
        action.Target.ApplyReaction(expectedReaction.reactionType);
    }


    // Resets the simulation and restores all interactable objects.
    public void ResetSimulation()
    {
        StopAllCoroutines();

        isSimulationRunning = false;
        simulationFailed = false;

        PlanningManager.Instance.ClearActions();

        foreach (InteractableObject obj in interactableObjects)
        {
            if (obj != null)
            {
                obj.ResetState();
            }
        }

        if (Alarm.Instance != null)
        {
            Alarm.Instance.ResetAlarm();
        }

        Debug.Log("Simulation Reset.");
    }
}
