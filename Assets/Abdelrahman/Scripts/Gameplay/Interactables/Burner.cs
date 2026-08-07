using System.Collections;
using UnityEngine;
using System;

public class Burner : InteractableObject
{
    public static event Action OnBurnerActivated;

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

        switch (reactionType)
        {
            case ReactionType.Ignite:
                TurnOn();
                break;
        }
    }

    private void TurnOn()
    {
        if (isOn)
        {
            return;
        }

        isOn = true;
        fireEffect.SetActive(true);
        lightOfFire.SetActive(true);

        // بنده الفانشكن اللي جوا لما النار تشتغل علشان افتح الباب بيها
        OnBurnerActivated?.Invoke();
    }
}
