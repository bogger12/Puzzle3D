using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
  [SerializeField] private Transform npcTransform;
  public void Interact()
  {
    Debug.Log("Interact");
    if (npcTransform == null)
    {
      Debug.LogError("npcTransform is not assigned in the inspector.");
      return;
    }
    SpeechBubble.Create(npcTransform, new Vector3(0, 2f), SpeechBubble.IconType.Neutral, "Hello there!");
  }
}
