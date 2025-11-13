using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // Public
    public InputActionAsset inputActions;  // Getting Input Action


    // Private
    private AudioManager audioManager; 
    private InputAction touchPressAction;
    private ArrowHandler arrowHandler;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        arrowHandler = GetComponent<ArrowHandler>();
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
        Vector2 touchPosition = context.ReadValue<Vector2>();

        // Create a PointerEventData for UI raycast
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            // Get the first UI GameObject hit
            GameObject touchedUI = results[0].gameObject;
            // Debug.Log($"Touched UI: {touchedUI.name}");
            // Debug.Log($"Touched UI: {touchedUI.transform.rotation.z}");
            if (touchedUI.tag == "Arrow")
            {
                audioManager.PlaySFX(audioManager.arrowClick);
                arrowHandler.nextTileCheck(touchedUI);
            }
        }
        else
        {
            Debug.Log("No UI object touched.");
        }


    }
}
