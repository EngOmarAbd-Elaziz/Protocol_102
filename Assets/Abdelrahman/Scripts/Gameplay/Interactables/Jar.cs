using UnityEngine;

public class Jar : InteractableObject
{

    public bool IsMixed => isMixed;


    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int liquidMaterialIndex = 3;
    [SerializeField] private Material purpleMaterial;
    private Material currentLiquidMaterial;
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
        // هنا بخليه يشغل الكلاس اللي في الاب وبعدين يجي يكمل هنا
        base.ApplyReaction(reactionType);

        // هنا بعمل سويتش عشان اعمل الرياكشن اللي انا عايزه + معرفتش اعمله بإف علشان تحويل الرياكشن لبولينج وكده
        switch (reactionType)
        {
            case ReactionType.Mix:
                isMixed = true;
                ChangeLiquidMaterial(purpleMaterial);
                Debug.Log("Jar Mixed");
                break;
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        isMixed = false;
        ChangeLiquidMaterial(originalLiquidMaterial);
    }

    public void ChangeLiquidMaterial(Material newMaterial)
    {
        Material[] materials = meshRenderer.materials;
        materials[liquidMaterialIndex] = newMaterial;
        meshRenderer.materials = materials;
        currentLiquidMaterial = newMaterial;
    }

}
