using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
  [SerializeField] private Transform npcTransform;
  [SerializeField] private string npcId = "npc1";

  private DialogueData dialogueData;
  private int currentLineIndex = 0;
  private SpeechBubble activeBubble; // track bubble

  private void Start()
  {
    dialogueData = DialogueLoader.Load(npcId);
  }

  public void Interact()
  {
    if (npcTransform == null)
    {
      Debug.LogError("npcTransform is not assigned in the inspector.");
      return;
    }

    if (dialogueData == null || dialogueData.lines.Length == 0)
    {
      Debug.LogWarning($"No dialogue available for {npcId}");
      return;
    }

    // If we're at the end, close the bubble and stop
    if (currentLineIndex >= dialogueData.lines.Length)
    {
      if (activeBubble != null)
      {
        Destroy(activeBubble.gameObject);
        activeBubble = null;
      }
      currentLineIndex = 0; // reset if you want repeatable dialogue
      return;
    }

    DialogueLine line = dialogueData.lines[currentLineIndex];

    if (!System.Enum.TryParse(line.icon, out SpeechBubble.IconType iconType))
    {
      iconType = SpeechBubble.IconType.Neutral;
    }

    // If bubble already exists, just update it
    if (activeBubble == null)
    {
      activeBubble = SpeechBubble.Create(
        npcTransform,
        new Vector3(0, 2f),
        iconType,
        line.text,
        Camera.main.transform // 👈 pass player camera or player transform
      );
    }
    else
    {
      activeBubble.Setup(iconType, line.text);
    }

    currentLineIndex++;
  }
}
