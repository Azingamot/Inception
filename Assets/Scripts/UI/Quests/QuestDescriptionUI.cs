using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questDescription;
    [SerializeField] private LayoutGroup requirementsLayout;
    [SerializeField] private QuestRequirementUI requirementUI;
    [SerializeField] private TMP_Text requirementTitle;

    private void Awake()
    {
        SetEmptyDescription();
    }

    public void SetDescription(QuestData questData)
    {
        ClearRequirements();
        requirementTitle.text = NamesHelper.ReceiveName("Required");
        questTitle.text = questData.Title;
        questDescription.text = questData.Description;
        LoadRequirements(questData);
    }

    private void SetEmptyDescription()
    {
        ClearRequirements();
        requirementTitle.text = NamesHelper.ReceiveName("EmptyQuest");
        questTitle.text = "";
        questDescription.text = "";
    }

    private void LoadRequirements(QuestData questData)
    {
        foreach (var item in questData.Requirements)
        {
            QuestRequirementUI requirement = Instantiate(requirementUI, requirementsLayout.transform, false);
            requirement.Initialize(item);
        }
    }

    private void ClearRequirements()
    {
        for (int i = 0; i < requirementsLayout.transform.childCount; i++)
        {
            Destroy(requirementsLayout.transform.GetChild(i).gameObject);
        }
    }
}
