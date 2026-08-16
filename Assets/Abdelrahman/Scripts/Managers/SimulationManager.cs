using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [SerializeField] private List<ReactionData> reactionData;
    [Space(30)]
    [SerializeField] private InteractableObject[] interactableObjects;
    [Space(10)]
    [SerializeField] private Alarm alarm;
    private bool isSimulationRunning;
    public bool IsSimulationRunning => isSimulationRunning;

    bool simulationFailed = false;


    public event Action OnSimulationFinished;
    public event Action OnSimulationFailed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // هنا بعمل فانكشن علشان ابدأ السيملشن وارجع تحذير لو السيملشن شغال او مفيش حاجة اتخططت وبيشتغل بزرار هناك
    public void StartSimulation()
    {

        if (isSimulationRunning)
        {
            Debug.LogWarning("Simulation is already running.");
            return;
        }

        if (PlanningManager.Instance.GetPlannedActions().Count == 0)
        {
            Debug.LogWarning("No planned actions to simulate.");
            simulationFailed = true;
            OnSimulationFailed?.Invoke();
            return;
        }

        StartCoroutine(RunSimulation());
    }

    private IEnumerator RunSimulation()
    {
        simulationFailed = false;
        isSimulationRunning = true;
        List<PlannedAction> actions = PlanningManager.Instance.GetPlannedActions();
        foreach (PlannedAction action in actions)
        {
            ExecuteAction(action);
            yield return new WaitForSeconds(0.5f);
        }

        PlanningManager.Instance.ClearActions();
        isSimulationRunning = false;

        if (simulationFailed)
        {
            Debug.LogWarning("Simulation Failed!");

            OnSimulationFailed?.Invoke();
            yield break;
        }

        Debug.Log("Simulation Finished.");

        OnSimulationFinished?.Invoke();
    }

    private void ExecuteAction(PlannedAction action)
    {
        ReactionType reaction = GetReactionData(action.Source.InteractableType, action.Target.InteractableType);

        Debug.Log(reaction);

        if (reaction == ReactionType.None)
        {
            simulationFailed = true;
            return;
        }
        // هنا بقول للتارجت انه يتفاعل مع الرياكشن اللي حصل علشان نكمل السلسلة
        action.Target.ApplyReaction(reaction);
    }

    private ReactionType GetReactionData(InteractableType source, InteractableType target)
    {
        foreach (ReactionData data in reactionData)
        {
            // بتاكد من ترتيب المتفاعلات علشان يحصل صح
            if (data.sourceType == source && data.targetType == target)
            {
                return data.reactionType;
            }
        }
        return ReactionType.None;
    }

    // بعمل ريست للسيملشن وارجع كل حاجة زي ما كانت عن رطيق زرار هناك
    public void ResetSimulation()
    {
        StopAllCoroutines();
        isSimulationRunning = false;
        simulationFailed = false;
        PlanningManager.Instance.ClearActions();
        foreach (InteractableObject obj in interactableObjects)
        {
            obj.ResetState();
        }
        Alarm.Instance.ResetAlarm();
        Debug.Log("Simulation Reset.");
    }

}
