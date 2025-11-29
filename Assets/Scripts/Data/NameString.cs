using UnityEngine;

[System.Serializable]
public struct NameString
{
    public string Name;
    public string Value;

    public NameString(string name, string value)
    {
        this.Name = name;
        this.Value = value;
    }
}
