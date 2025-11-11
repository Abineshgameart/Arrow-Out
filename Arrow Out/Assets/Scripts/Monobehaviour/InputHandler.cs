using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // Public
    public InputActionAsset inputActions;


    // Private
    private InputAction touchPressAction;

    private void Awake()
    {
        touchPressAction = inputActions.FindActionMap("TouchActionMap").FindAction("Touch");
    }

    private void OnEnable()
    {
        touchPressAction.Enable();
        touchPressAction.performed += OnTouchPress;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchPress;
        touchPressAction.Disable();
    }

    private void OnTouchPress(InputAction.CallbackContext context)
    {
        // Get the touch position
        Vector2 touchPosition = Pointer.current.position.ReadValue();

        // Create a PointerEventData to use with the EvenSystem
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;

        // Raycast against UI Elements
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Process the results
        foreach (RaycastResult result in results)
        {
            Debug.Log("Touched UI GameObjects" + result.gameObject.transform.name);
        }

    }
}
