using UnityEngine;
using UnityEngine.Events;

public class NPCDialogueAdapter : MonoBehaviour
{
    [SerializeField] private UnityEvent dialogueStart;
    [SerializeField] private UnityEvent<EventData> dialogueEnd;
    [SerializeField] private DialogueStarter starter;

    public void StartDialogue(EventData eventData)
    {
        if (eventData is DialogueEventData dialogueEventData)
        {
            dialogueStart.Invoke();
            starter.StartDialogue(dialogueEventData.DialogueName,dialogueEnd, eventData);
        } 
    }
}
