using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringsGenerator : MonoBehaviour
{
    [SerializeField] private List<NameString> nameData;

    public void Generate()
    {
        List<Item> items = LoadItems();

        foreach (string language in TranslationHandler.LanguagesPathMap.Keys)
        {
            StringsStorage storage = NamesHelper.LoadStorage(language);

            foreach (Item item in items)
            {
                if (!storage.Contains(item.name))
                {
                    storage.Add(item.name, item.name);
                    storage.Add(item.name + "_description", "");
                }
            }

            NamesHelper.CreateFile(language, storage);
        }
    }

    public void AddData()
    {
        NamesHelper.WriteToFile(nameData);
    }

    private List<Item> LoadItems()
    {
        return Resources.LoadAll<Item>("Items").ToList();
    }
}
