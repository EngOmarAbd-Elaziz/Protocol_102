using System.Collections;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] private Transform doorModel;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float openDelay = 0.7f;
    private bool isOpen;
    private Quaternion closedRotation;
    private Quaternion openedRotation;


    private void Start()
    {
        closedRotation = doorModel.localRotation;
        openedRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        if (isOpen)
        {
            doorModel.localRotation = Quaternion.RotateTowards(doorModel.localRotation, openedRotation, rotationSpeed * Time.deltaTime);
        }


    }

    private void OnEnable()
    {
        Burner.OnBurnerActivated += HandleBurnerActivated;
    }

    private void OnDisable()
    {
        Burner.OnBurnerActivated -= HandleBurnerActivated;
    }

    public override void Interact()
    {

    }

    public override void ApplyReaction(ReactionType reactionType)
    {
        base.ApplyReaction(reactionType);

        switch (reactionType)
        {
            case ReactionType.OpenDoor:
                OpenDoor();
                break;
        }
    }

    private void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }
        
        isOpen = true;
    }

    // بشغل الفانكشن في الايفينت فوق لما النار هناك تتفعل علشان نفتح منه الباب على طول
    private void HandleBurnerActivated()
    {
        StartCoroutine(OpenDoorAfterDelay());
    }

    IEnumerator OpenDoorAfterDelay()
    {
        yield return new WaitForSeconds(openDelay);
        ApplyReaction(ReactionType.OpenDoor);
    }
}
