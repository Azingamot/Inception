using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private string dialogueName;

    public void StartDialogue()
    {
        DialogueSystem.Instance.StartDialogue(dialogueName);
    }

    public void StartDialogue(string dialogueName, UnityEvent<EventData> dialogueEndEvent, EventData eventData)
    {
        DialogueSystem.Instance.StartDialogue(dialogueName, dialogueEndEvent, eventData);
    }
}
