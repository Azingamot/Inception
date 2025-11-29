using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private ClockController clockController;
    [SerializeField] private TMP_Text dialogueTitle;
    [SerializeField] private TMP_Text dialogueContent;

    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;

    private Queue<DialogueLine> currentDialogue = new Queue<DialogueLine>();
    private Coroutine textLoadCoroutine;

    private DialogueLine currentLine;
    private string currentText;

    private bool inDialogue = false;
    private EventData dialogueEventData;

    private UnityEvent<EventData> dialogueEventEnd;

    public static DialogueSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    public void StartDialogue(string dialogueName)
    {
        if (inDialogue)
            return;

        OnDialogueStart.Invoke();

        clockController.enabled = false;

        inDialogue = true;

        dialogueObject.SetActive(true);
        DialogueData data = DialogueFileHandler.ReceiveDialogueData(dialogueName);
        currentDialogue.Clear();

        foreach (DialogueLine line in data)
        {
            currentDialogue.Enqueue(line);
        }

        LoadDialogueLine(currentDialogue.Dequeue());    
    }

    public void StartDialogue(string dialogueName, UnityEvent<EventData> dialogueEndEvent, EventData eventData)
    {
        dialogueEventEnd = dialogueEndEvent;

        this.dialogueEventData = eventData;
        Debug.Log(dialogueName);
        StartDialogue(dialogueName);
    }

    private void LoadDialogueLine(DialogueLine line)
    {
        currentLine = line;
        dialogueTitle.text = line.Title;
        textLoadCoroutine = StartCoroutine(ShowText(line.Content));
    }

    public void SkipDialogueLoading()
    {
        if (currentText != currentLine.Content)
            InterruptDialogueLineLoad();
        else
            NextDialogueLine();
    }

    public void StopDialogue()
    {
        clockController.enabled = true;

        dialogueObject.SetActive(false);
        StopAllCoroutines();
        inDialogue = false;
        OnDialogueEnd.Invoke();
        dialogueEventEnd.Invoke(dialogueEventData);
    }

    private void InterruptDialogueLineLoad()
    {
        if (textLoadCoroutine != null)
            StopCoroutine(textLoadCoroutine);

        currentText = currentLine.Content;
        dialogueContent.text = currentLine.Content;
    }

    private void NextDialogueLine()
    {
        InterruptDialogueLineLoad();
        if (currentDialogue.Count > 0) 
            LoadDialogueLine(currentDialogue.Dequeue());
        else
            StopDialogue();
    }

    private IEnumerator ShowText(string text)
    {
        currentText = "";
        int index = 0;

        while (currentText != text)
        {
            currentText += text[index];
            index++;
            dialogueContent.text = currentText;
            yield return new WaitForFixedUpdate();
        }
    }
}
