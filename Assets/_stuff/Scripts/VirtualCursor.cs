using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class VirtualCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    public RectTransform cursorTransform; // UI image representing the cursor
    public float cursorSpeed = 1000f;     // pixels per second
    public Canvas canvas;                  // canvas holding the cursor image

    private Mouse virtualMouse;
    private Camera mainCamera;

    // World position for weapons
    public static Vector3 WorldPosition { get; private set; }

    void OnEnable()
    {
        mainCamera = Camera.main;

        // Add or get virtual mouse
        if (virtualMouse == null)
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        InputUser.PerformPairingWithDevice(virtualMouse);

        // Start cursor at screen center
        Vector2 startPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        InputState.Change(virtualMouse.position, startPos);

        Cursor.visible = false;

        // Update after input system update
        InputSystem.onAfterUpdate += UpdateVirtualCursor;
    }

    void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);

        InputSystem.onAfterUpdate -= UpdateVirtualCursor;
    }

    private void UpdateVirtualCursor()
    {
        if (virtualMouse == null || Player.Instance == null) return;

        // --- Move cursor ---
        Vector2 rightStick = Player.Instance.input.Player.Look.ReadValue<Vector2>();
        if (rightStick.magnitude < 0.1f) rightStick = Vector2.zero;

        float dt = Time.unscaledDeltaTime;
        Vector2 currentPosition = virtualMouse.position.ReadValue();
        currentPosition += rightStick * cursorSpeed * dt;

        currentPosition.x = Mathf.Clamp(currentPosition.x, 0, Screen.width);
        currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, currentPosition);

        // --- Update world position ---
        Vector3 wp = mainCamera.ScreenToWorldPoint(new Vector3(currentPosition.x, currentPosition.y, 0f));
        wp.z = 0f;
        WorldPosition = wp;

        // --- Update cursor UI ---
        if (cursorTransform != null && canvas != null)
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                currentPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out anchoredPos
            );
            cursorTransform.anchoredPosition = anchoredPos;
        }

        // --- Simulate A button as left click ---
        if (Gamepad.current != null)
        {
            bool aPressed = Gamepad.current.buttonSouth.isPressed;

            // Send press/release events to virtual mouse
            if (aPressed && !virtualMouse.leftButton.isPressed)
                InputSystem.QueueStateEvent(virtualMouse, new MouseState { buttons = 1 });
            else if (!aPressed && virtualMouse.leftButton.isPressed)
                InputSystem.QueueStateEvent(virtualMouse, new MouseState { buttons = 0 });
        }

        // --- Raycast UI buttons and invoke click manually ---
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = currentPosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var r in results)
            {
                UnityEngine.UI.Button btn = r.gameObject.GetComponent<UnityEngine.UI.Button>();
                if (btn != null)
                    btn.onClick.Invoke();
            }
        }
    }
}