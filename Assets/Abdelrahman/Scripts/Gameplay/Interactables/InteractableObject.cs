using UnityEngine;

public abstract class InteractableObject : MonoBehaviour , IInteractable
{
    // Public :-
    public InteractableType InteractableType => interactableType;

    // Private :-
    [Header("Visual")]
    [SerializeField] private Renderer objectRenderer;
    [Header("Interactable Settings")]
    [SerializeField] private InteractableType interactableType;
    private bool isReacted = false;


    public virtual void Select()
    {
        Debug.Log($"{name} selected");
    }

    public virtual void Deselect()
    {
        Debug.Log($"{name} DeSelected");
    }

    public virtual void ApplyReaction(ReactionType reactionType)
    {
        if (isReacted)
        {
            return;
        }

        isReacted = true;
        Debug.Log($"{name} reacted with {reactionType}");
    }

    public abstract void Interact();
    
}
