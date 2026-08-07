using UnityEngine;

[CreateAssetMenu(fileName = "New Reaction Data", menuName = "Scriptable Object/Reaction Data")]
public class ReactionData : ScriptableObject
{
    [Header("Required Objects")]
    public InteractableType sourceType;
    public InteractableType targetType;

    [Header("Result")]
    public ReactionType reactionType;
}
