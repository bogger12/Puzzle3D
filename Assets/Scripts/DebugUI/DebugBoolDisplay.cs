using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBoolDisplay : MonoBehaviour
{

    public string label = "bool";
    public bool activeBool;
    public Color[] boolColors = {Color.green, Color.red};
    public string trueText;
    public string falseText;

    private Image backgroundImage;
    private TextMeshProUGUI boolText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundImage = GetComponentInChildren<Image>();
        boolText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        backgroundImage.color = activeBool ? boolColors[0] : boolColors[1];
        boolText.text = activeBool ? trueText : falseText;
    }
}
