using UnityEngine;

public static class DialogueLoader
{
  public static DialogueData Load(string npcId)
  {
    TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{npcId}");
    if (jsonFile == null)
    {
      Debug.LogError($"Dialogue file for NPC '{npcId}' not found in Resources/Dialogues/");
      return null;
    }
    return JsonUtility.FromJson<DialogueData>(jsonFile.text);
  }
}