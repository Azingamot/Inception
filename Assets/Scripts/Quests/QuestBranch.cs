using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestBranch
{
    public string Name;

    [Header("Quest To Validate")]
    public QuestData validatedQuest;

    [Header("Failure")]
    public DialogueEventData OnFailedDialogue;
    public UnityEvent<EventData> FailedDialogueStart;
    public UnityEvent<EventData> FailedDialogueEnd;

    [Header("Success")]
    public DialogueEventData OnSucceedDialogue;
    public UnityEvent<EventData> SuccessDialogueStart;
    public UnityEvent<EventData> SuccessDialogueEnd;
}
