using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugGroundedDisplay : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public Color[] groundedColors = {Color.green, Color.red};

    private Image backgroundImage;
    private TextMeshProUGUI groundedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundImage = GetComponentInChildren<Image>();
        groundedText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = playerMovement.IsGrounded;
        backgroundImage.color = grounded ? groundedColors[0] : groundedColors[1];
        groundedText.text = grounded ? "Grounded" : "Not Grounded";
    }
}
