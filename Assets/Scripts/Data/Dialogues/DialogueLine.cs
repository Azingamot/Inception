using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string Title;
    [TextArea(5, 20)]
    public string Content;
    public CharacterEmotion Emotion;
}