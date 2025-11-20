using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private string dialogueName;

    public void StartDialogue()
    {
        DialogueSystem.Instance.StartDialogue(dialogueName);
    }

    public void StartDialogue(string dialogueName, UnityEvent dialogueEndEvent)
    {
        DialogueSystem.Instance.StartDialogue(dialogueName, dialogueEndEvent);
    }
}
