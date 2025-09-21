using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputStore : MonoBehaviour
{
    [Header("Player (0 or 1)")]
    public int playerIndex; // set in inspector (0 = P1, 1 = P2)

    [HideInInspector]
    public PlayerInput playerInput;
    public InputDevice assignedDevice;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        assignedDevice = playerInput.devices.Count==0 ? Keyboard.current : playerInput.devices[0];
        Debug.Log($"Player {playerIndex + 1} joined using {assignedDevice.displayName}");
    }
    void Start()
    {

    }

    void Update()
    {
        // Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        // Debug.Log(lookInput);
    }


    public string GetButtonText(InputAction action)
    {

        if (action.bindings.Count == 0 || assignedDevice == null)
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

            if (binding.groups.Contains(assignedDevice.layout))
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