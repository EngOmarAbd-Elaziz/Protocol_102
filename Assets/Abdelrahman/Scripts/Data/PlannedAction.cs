using System;
using UnityEngine;

[Serializable]
public class PlannedAction
{
    public InteractableObject Source;
    public InteractableObject Target;

    public PlannedAction(InteractableObject source, InteractableObject target)
    {
        Source = source;
        Target = target;
    }
}
