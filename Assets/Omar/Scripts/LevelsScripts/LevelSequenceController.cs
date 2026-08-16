using UnityEngine;

public abstract class LevelSequenceController : MonoBehaviour
{
    protected virtual void Start()
    {
        if (SimulationManager.Instance == null)
        {
            Debug.LogError(
                "SimulationManager.Instance is NULL!",
                this
            );

            return;
        }

        SimulationManager.Instance.OnSimulationFinished += StartSequence;

        Debug.Log("LevelSequenceController subscribed successfully.", this);
    }

    protected virtual void OnDestroy()
    {
        if (SimulationManager.Instance != null)
        {
            SimulationManager.Instance.OnSimulationFinished -= StartSequence;
        }
    }

    protected abstract void StartSequence();

}
