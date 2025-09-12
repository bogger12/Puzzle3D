using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Player (0 or 1)")]
    public int playerIndex; // set in inspector (0 = P1, 1 = P2)

    public InputActionAsset playerInput;
    [HideInInspector]
    public InputActionReference move;
    [HideInInspector]
    public InputActionReference look;
    [HideInInspector]
    public InputActionReference jump;
    [HideInInspector]
    public InputActionReference holdThrow;

    void Start()
    {
        string mapName = playerIndex == 0 ? "Player1" : "Player2";
        var map = playerInput.FindActionMap(mapName);

        move = InputActionReference.Create(map.FindAction("Move"));
        look = InputActionReference.Create(map.FindAction("Look"));
        jump = InputActionReference.Create(map.FindAction("Jump"));
        holdThrow = InputActionReference.Create(map.FindAction("Pickup_Throw"));
    }

    private InputDevice lastUsedDevice;


    // Input action events to determine device used
    // I'm not gonna update text on every change cus I lazy and no one will change controllers while UI hint is visible. 
    void OnEnable()
    {
        // Subscribe to input events
        InputSystem.onActionChange += OnActionChange;
    }

    void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var action = (InputAction)obj;
            lastUsedDevice = action.activeControl?.device;
            // UpdatePrompt();
        }
    }

    public string GetButtonText(InputActionReference actionReference)
    {
        var action = actionReference.action;

        if (action.bindings.Count == 0 || lastUsedDevice == null)
        {
            return InputControlPath.ToHumanReadableString(
                action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

        }

        // Find the binding that matches the current device
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];

            if (binding.groups.Contains(lastUsedDevice.layout))
            {
                string keyString = InputControlPath.ToHumanReadableString(
                    binding.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );

                return keyString;
                // promptText.text = $"Press [{keyString}] to {action.name}";
            }
        }
        return "FUCKNOTFOUND";
    }
}