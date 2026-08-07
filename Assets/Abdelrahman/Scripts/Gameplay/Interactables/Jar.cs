using UnityEngine;

public class Jar : InteractableObject
{
    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] private Renderer defaultMaterial;
    [SerializeField] private Material purpleMaterial;


    private bool isMixed = false;

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
                liquidRenderer.material = purpleMaterial;
                break;
        }
    }

}
