using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringsGenerator : MonoBehaviour
{
    public void Generate()
    {
        List<Item> items = LoadItems();
        StringsStorage storage = NamesHelper.LoadStorage();

        foreach (Item item in items)
        {
            if (!storage.Contains(item.name))
            {
                storage.Add(item.name, item.name);
                storage.Add(item.name + "_description", "");
            }
        }

        NamesHelper.CreateFile(storage);
    }

    private List<Item> LoadItems()
    {
        return Resources.LoadAll<Item>("Items").ToList();
    }
}
