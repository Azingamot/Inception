[System.Serializable]
public class EventHappenData
{
    public string UID;
    public bool Happen = false;

    public EventData EventData { get; set; }
}
