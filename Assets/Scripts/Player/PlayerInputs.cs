using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Player (0 or 1)")]
    public int playerIndex; // set in inspector (0 = P1, 1 = P2)

    public InputActionAsset playerInput;
    [HideInInspector]
    public InputAction move;
    [HideInInspector]
    public InputAction look;
    [HideInInspector]
    public InputAction jump;
    [HideInInspector]
    public InputAction holdThrow;

    void Start()
    {
        string mapName = playerIndex == 0 ? "Player1" : "Player2";
        var map = playerInput.FindActionMap(mapName);

        move = map.FindAction("Move");
        look = map.FindAction("Look");
        jump = map.FindAction("Jump");
        holdThrow = map.FindAction("Pickup_Throw");
    }

}