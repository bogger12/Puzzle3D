using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class ListenForSecondKeyboard : MonoBehaviour
{

    public InputActionAsset player1Input;
    public InputActionAsset player2Input;

    private PlayerInputManager playerInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        var moveAction = player1Input.FindActionMap("PlayerAlt", true)["Move"];

        moveAction.performed += OnMove;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        playerInputManager.JoinPlayerFromAction(ctx);
        Debug.Log("Tyring to join");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for any keayboard inputs
        bool player1pressed = player1Input.FindActionMap("PlayerAlt", true)["Move"].ReadValue<Vector2>().magnitude > 0;
        bool player2pressed = player2Input.FindActionMap("PlayerAlt", true)["Move"].ReadValue<Vector2>().magnitude > 0;
        if (playerInputManager.playerCount == 1 && (player1pressed || player2pressed))
        {
            PlayerInput newPlayer = playerInputManager.JoinPlayer(1, -1, "Keyboard2", Keyboard.current);
            newPlayer.GetComponent<AssignUniquePlayerValues>().UseSpecificCamera(0, 1);
        }
    }
}
