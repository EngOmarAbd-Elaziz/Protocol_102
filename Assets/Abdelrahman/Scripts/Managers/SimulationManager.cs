using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [SerializeField] private List<ReactionData> reactionData;
    private bool isSimulationRunning;
    public bool IsSimulationRunning => isSimulationRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



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
            return;
        }

        StartCoroutine(RunSimulation());
    }

    private IEnumerator RunSimulation()
    {
        isSimulationRunning = true;
        List<PlannedAction> actions = PlanningManager.Instance.GetPlannedActions();
        foreach (PlannedAction action in actions)
        {
            ExecuteAction(action);
            yield return new WaitForSeconds(2f); 
        }

        PlanningManager.Instance.ClearActions();
        isSimulationRunning = false;
        Debug.Log("Simulation Finished.");
    }

    private void ExecuteAction(PlannedAction action)
    {
        ReactionType reaction = GetReactionData(action.Source.InteractableType, action.Target.InteractableType);

        Debug.Log(reaction);

        if (reaction == ReactionType.None)
        {
            return;
        }
        // هنا بتاكد ان البرطمان بتاعي اتفاعل مع اللي قبله علشان يقدر يولع النار
        if (action.Source is Jar jar && !jar.IsMixed)
        {
            Debug.Log("Jar is not mixed yet.");
            return;
        }

        //action.Source.ApplyReaction(reaction);
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
    // ده فانكشن حاطه على زرار برا علشان ابدا منه السيموليشن
    public void ClickToStart() 
    {
        StartSimulation();
    }

}
