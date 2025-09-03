using UnityEngine;

public class GameHandler_SpeechBubble : MonoBehaviour
{
  [SerializeField] private Transform npcTransform;

  private void Start()
  {
    Debug.Log(npcTransform);
    if (npcTransform == null)
    {
      Debug.LogError("npcTransform is not assigned in the inspector.");
      return;
    }
    // SpeechBubble.Create(npcTransform, new Vector3(0, 2f), SpeechBubble.IconType.Neutral, "Hello there!");
  }
}
