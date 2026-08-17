using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    // Public :-
    public static GameInputManager Instance { get; private set; }
    public event Action OnInteract;

    // Private :-
    private MyNewInputSystem inputSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputSystem = new MyNewInputSystem();
    }

    private void OnEnable()
    {
        Debug.Log("GAME INPUT MANAGER ENABLED");

        inputSystem.Enable();

        inputSystem.Player.Interact.performed += GameInputManager_OnInteract;
    }

    private void OnDisable()
    {
        inputSystem.Player.Interact.performed -= GameInputManager_OnInteract;

        inputSystem.Disable();
    }


    private void GameInputManager_OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log($"INTERACT RECEIVED: {context.control}");

        if (context.performed)
        {
            Debug.Log("INTERACT PRESSED");
            OnInteract?.Invoke(); 
        }
    }

    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
    
}
