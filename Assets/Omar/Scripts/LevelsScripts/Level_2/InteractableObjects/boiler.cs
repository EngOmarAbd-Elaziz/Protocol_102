using UnityEngine;

public class boiler : InteractableObject
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int liquidMaterialIndex = 1;
    [SerializeField] private Material theNewMaterial;
    private Material originalLiquidMaterial;

    private bool isMixed = false;



    private void Start()
    {
        originalLiquidMaterial = meshRenderer.materials[liquidMaterialIndex];
    }

    public override void Interact()
    {
        
    }

    public override void ApplyReaction(ReactionType reactionType)
    {
        base.ApplyReaction(reactionType);

        switch (reactionType) 
        {
            case ReactionType.Mix2:
                isMixed = true; 
                break;
        }
    }
}
