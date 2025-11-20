using System.Threading.Tasks;
using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    public async void Save()
    {
        await SaveSystem.Save();
        TextNotificationUI.Instance.Notify("Saved!");
    }

    public void Load()
    {
        SaveSystem.Load();
    }
}
