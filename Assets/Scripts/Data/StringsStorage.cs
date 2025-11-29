using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class StringsStorage
{
    public List<NameString> NameStrings = new List<NameString>();

    public void Add(string name, string value)
    {
        NameStrings.Add(new NameString(name, value));
    }

    public string Get(string name)
    {
        return NameStrings.FirstOrDefault(x => x.Name == name).Value;
    }

    public bool Contains(string name)
    {
        return NameStrings.Any(x => x.Name == name);
    }
}
