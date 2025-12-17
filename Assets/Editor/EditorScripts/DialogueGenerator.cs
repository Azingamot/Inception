using UnityEngine;

public class DialogueGenerator : MonoBehaviour
{
    [SerializeField] private string dialogueName;
    [SerializeField] private string language;
    [SerializeField] private DialogueData dialogueData;
    
    public void Generate()
    {
        DialogueFileHandler.GenerateDialogueFile(dialogueName, dialogueData);
    }

    public void GenerateSpecified()
    {
        DialogueFileHandler.GenerateDialogueFile(dialogueName, language, dialogueData);
    }

    public void GenerateEmpty()
    {
        DialogueFileHandler.GenerateDialogueFile(dialogueName);
    }
}
