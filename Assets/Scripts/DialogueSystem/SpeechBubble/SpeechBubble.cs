using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SpeechBubble : MonoBehaviour
{

  private Transform lookTransform;

  public enum IconType
  {
    Happy,
    Sad,
    Neutral,
  }

  [SerializeField] private Sprite happyIconSprite;
  [SerializeField] private Sprite sadIconSprite;
  [SerializeField] private Sprite neutralIconSprite;


  private SpriteRenderer backgroundSpriteRenderer;
  private SpriteRenderer iconSpriteRenderer;

  private TextMeshPro textMeshPro;

  private void Update()
  {
    if (lookTransform != null)
    {
      Vector3 direction = lookTransform.position - transform.position;
      direction.y = 0f; // keep upright
      if (direction.sqrMagnitude > 0.001f)
      {
        transform.rotation = UnityEngine.Quaternion.LookRotation(-direction);
      }
    }
  }

  private void Awake()
  {
    backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
    iconSpriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
    textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
  }

  // private void Start()
  // {
  //   Setup(IconType.Happy, "INITIAL TEXT");
  // }

  public void Initialise(Vector3 localPosition, IconType iconType, string text, Transform lookAt = null)
  {
    transform.localPosition = localPosition;

    lookTransform = lookAt;
    Setup(iconType, text);
  }
  public void Setup(IconType iconType, string text)
  {
    textMeshPro.SetText(text);
    textMeshPro.ForceMeshUpdate();

    Vector2 textSize = textMeshPro.GetRenderedValues(false);
    Vector2 padding = new Vector2(2f, 0.5f);

    backgroundSpriteRenderer.size = textSize + padding;

    Vector2 offset = new Vector2(-2.2f, 0f);
    backgroundSpriteRenderer.transform.localPosition =
        new Vector3(backgroundSpriteRenderer.size.x / 2f + offset.x, 0f);

    iconSpriteRenderer.sprite = GetIconSprite(iconType);
  }


  private Sprite GetIconSprite(IconType iconType)
  {
    switch (iconType)
    {
      case IconType.Happy:
        return happyIconSprite;
      case IconType.Sad:
        return sadIconSprite;
      case IconType.Neutral:
        return neutralIconSprite;
      default:
        return null;
    }
  }
}
