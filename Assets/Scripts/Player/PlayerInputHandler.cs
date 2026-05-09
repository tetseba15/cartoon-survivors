using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsGamepad { get; private set; }

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    void Update()
    {
        // El joystick virtual en pantalla alimentará este vector directamente
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        var lookAction = _inputActions.Player.Look;
        LookInput = lookAction.ReadValue<Vector2>();

        if (lookAction.activeControl != null)
        {
            IsGamepad = lookAction.activeControl.device is Gamepad;
        }
    }
}