using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueData: IEnumerable<DialogueLine>
{
    public List<DialogueLine> DialogueLines;

    public IEnumerator<DialogueLine> GetEnumerator()
    {
        foreach (DialogueLine line in DialogueLines)
            yield return line;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}