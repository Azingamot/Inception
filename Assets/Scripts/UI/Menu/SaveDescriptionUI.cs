using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text saveName, saveDate, saveTime;
    [SerializeField] private Image clockImage;
    [SerializeField] private Sprite sunSprite, moonSprite;
    [SerializeField] private TMP_InputField saveNameInput;
    [SerializeField] private GameObject editButton;
    private int currentSaveSelected = -1;
    private SaveLoader loader;
    private SavesUI savesUI;
    private SaveData currentData;

    public void SetDescriptionForSave(int index, SaveLoader loader, SavesUI savesUI)
    {
        this.loader = loader;
        this.savesUI = savesUI;
        currentSaveSelected = index;
        currentData = SaveSystem.ReceiveSaveData(index);

        if (currentData != null)
            LoadDescription(currentData);
        else
            LoadEmptyDescription(index);
    }

    public async void LoadSave()
    {
        if (currentSaveSelected != -1)
        {
            await loader.LoadSaveFile(currentSaveSelected);
        }
    }

    public void RemoveSave()
    {
        savesUI.RemoveSaveFile(currentSaveSelected);
    }

    private void LoadEmptyDescription(int index)
    {
        editButton.SetActive(false);
        saveName.text = "Save №" + index;
        saveDate.text = "No data";
        saveTime.text = "";
        clockImage.sprite = sunSprite;
    }

    private void LoadDescription(SaveData data)
    {
        editButton.SetActive(true);
        saveName.text = data.Name;
        saveDate.text = data.WorldSaveData.saveTime.DateTime.ToString("dd.MM.yyyy HH:mm:ss");
        saveTime.text = ClockText(data.ClockContext);
        clockImage.sprite = data.ClockContext.DayTime == DayTime.Day ? sunSprite : moonSprite;
    }

    private string ClockText(ClockContext clockContext)
    {
        return string.Concat("Day ", clockContext.Days, " ", ClockFormatText(clockContext.Hours), ":", ClockFormatText(clockContext.Minutes), "\n", clockContext.DayTime.ToString());
    }

    private string ClockFormatText(int value)
    {
        return value < 10 ? "0" + value.ToString() : value.ToString();
    }

    public async void ChangeSaveFileName(string fileName)
    {
        await SaveSystem.SetSaveFileName(currentData, currentSaveSelected, fileName);
        saveNameInput.text = "";
        savesUI.LoadSaves();
        LoadDescription(currentData);
    }

    public void EditText()
    {
        if (currentSaveSelected == -1 || SaveSystem.ReceiveSaveData(currentSaveSelected) == null)
            return;

        bool active = saveNameInput.gameObject.activeInHierarchy;
        saveName.gameObject.SetActive(active);
        saveNameInput.gameObject.SetActive(!active);
    }
}
