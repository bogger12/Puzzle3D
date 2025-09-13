using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
  [SerializeField] private string npcId = "npc1";

  private DialogueData dialogueData;
  private int currentLineIndex = 0;
  private SpeechBubble activeBubble; // track bubble

  public GameObject speechBubblePrefab;

  private void Start()
  {
    dialogueData = DialogueLoader.Load(npcId);
  }

  public void Interact(Transform playerCamera)
  {
    if (dialogueData == null || dialogueData.lines.Length == 0)
    {
      Debug.LogWarning($"No dialogue available for {npcId}");
      return;
    }

    if (currentLineIndex >= dialogueData.lines.Length)
    {
      if (activeBubble != null)
      {
        Destroy(activeBubble.gameObject);
        activeBubble = null;
      }
      currentLineIndex = 0;
      return;
    }

    DialogueLine line = dialogueData.lines[currentLineIndex];

    if (!System.Enum.TryParse(line.icon, out SpeechBubble.IconType iconType))
    {
      iconType = SpeechBubble.IconType.Neutral;
    }

    if (activeBubble == null)
    {
      GameObject bubble = Instantiate(speechBubblePrefab, transform);
      activeBubble = bubble.GetComponent<SpeechBubble>();
      activeBubble.Initialise(
          new Vector3(0, 2f),
          iconType,
          line.text,
          playerCamera // ✅ now we pass in the correct camera
      );
    }
    else
    {
      activeBubble.Setup(iconType, line.text);
    }

    currentLineIndex++;
  }
}
