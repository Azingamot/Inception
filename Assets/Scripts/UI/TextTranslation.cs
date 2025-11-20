using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextTranslation : MonoBehaviour
{
    [SerializeField] private string path;

    private void Awake()
    {
        GetComponent<TMP_Text>().text = NamesHelper.ReceiveName(path);
    }
}
