using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestBranching : MonoBehaviour
{
    [SerializeField] private List<QuestBranch> questBranches = new List<QuestBranch>();
    [SerializeField] private DialogueStarter dialogueStarter;
    [SerializeField] private QuestValidator validator;
    
    public void BranchOff(EventData eventData)
    {
        if (eventData is QuestDialogueEventData questDialogueEvent)
        {
            QuestBranch branch = BranchWithData(questDialogueEvent.QuestData);
            if (validator.Validate(branch.validatedQuest))
            {
                branch.SuccessDialogueStart.Invoke(questDialogueEvent);
                dialogueStarter.StartDialogue(branch.OnSucceedDialogue.DialogueName, branch.SuccessDialogueEnd, questDialogueEvent);
            }
            else
            {
                branch.FailedDialogueStart.Invoke(questDialogueEvent);
                dialogueStarter.StartDialogue(branch.OnFailedDialogue.DialogueName, branch.FailedDialogueEnd, questDialogueEvent);
            }   
        }
    }

    private QuestBranch BranchWithData(QuestData quest)
    {
        return questBranches.FirstOrDefault(u => u.validatedQuest == quest);
    }
}
