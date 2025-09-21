using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashButton : MonoBehaviour
{
    private Image image;
    public float flashSpeed = 1;

    public float minOpacity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float opacity = Mathf.Lerp(minOpacity, 1f, Mathf.Sin(Time.time * flashSpeed) * 0.5f + 0.5f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
    }
}
