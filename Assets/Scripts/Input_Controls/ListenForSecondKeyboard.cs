using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class ListenForSecondKeyboard : MonoBehaviour
{

    public KeyCode player2PressToJoin;

    private PlayerInputManager playerInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for p2 join inputs

        if (playerInputManager.playerCount == 1 && Input.GetKeyDown(player2PressToJoin))
        {
            PlayerInput newPlayer = playerInputManager.JoinPlayer(1, -1, "Keyboard2", Keyboard.current);
            // newPlayer.GetComponent<AssignUniquePlayerValues>().UseSpecificCamera(0, 1);
            newPlayer.GetComponent<AssignUniquePlayerValues>().SetUseSpeciifCameraOnStart(0);
            Debug.Log("P2 has keyboard");
        }
    }
}