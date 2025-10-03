using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
  [SerializeField] private string npcId = "npc1";

  private DialogueData dialogueData;
  private int currentLineIndex = 0;
  [HideInInspector]
  public GameObject currentBubble; // track bubble

  public GameObject speechBubbleObject;
  public Transform bubbleAnchor;

  public void LoadDialogue()
  {
    dialogueData = DialogueLoader.Load(npcId);
  }

  public void SpawnSpeechBubble(Canvas canvas)
  {
    currentBubble = Instantiate(speechBubbleObject, canvas.transform);
    currentBubble.GetComponent<AnchorOn3DPoint>().anchor = bubbleAnchor;
  }

  public void DeleteCurrentBubble()
  {
    Destroy(currentBubble);
  }
}
