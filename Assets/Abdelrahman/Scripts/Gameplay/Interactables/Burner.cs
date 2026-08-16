using System.Collections;
using UnityEngine;

public class Burner : InteractableObject
{


    [Header("Burner")]
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject lightOfFire;
    private bool isOn;


    private void Start()
    {
        fireEffect.SetActive(false);
        lightOfFire.SetActive(false);
    }

    public override void Interact()
    {

    }

    public override void ApplyReaction(ReactionType reactionType)
    {
        base.ApplyReaction(reactionType);
    }

    public override void ResetState()
    {
        base.ResetState();
        isOn = false;
        fireEffect.SetActive(false);
        lightOfFire.SetActive(false);
    }

    public void TurnOn()
    {
        if (isOn)
        {
            return;
        }

        isOn = true;
        fireEffect.SetActive(true);
        lightOfFire.SetActive(true);

    }
}
