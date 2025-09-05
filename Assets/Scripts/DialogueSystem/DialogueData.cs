using System;

[Serializable]
public class DialogueLine
{
  public string icon;
  public string text;
}

[Serializable]
public class DialogueData
{
  public string npcId;
  public DialogueLine[] lines;
}