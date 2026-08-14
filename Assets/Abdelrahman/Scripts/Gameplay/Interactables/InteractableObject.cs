using UnityEngine;

public abstract class InteractableObject : MonoBehaviour , IInteractable
{
    // Public :-
    public InteractableType InteractableType => interactableType;
    public static InteractableObject Instance { get; private set; }

    // Private :-
    [Header("Visual")]
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color selectedColor = Color.yellow;
    private Color originalColor;
    [Header("Interactable Settings")]
    [SerializeField] private InteractableType interactableType;
    private bool isReacted = false;


    private void Awake()
    {
        originalColor = objectRenderer.material.color;
    }


    public virtual void Select()
    {
        objectRenderer.material.color = selectedColor;
        Debug.Log($"{name} selected");
    }

    public virtual void Deselect()
    {
        objectRenderer.material.color = originalColor;
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

    public virtual void ResetState()
    {
        isReacted = false;
    }

    public abstract void Interact();
    
}
