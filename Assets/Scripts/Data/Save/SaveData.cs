using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class SaveData
{
    public string Name;
    public List<ObjectPosition> ObjectPositions = new();
    public PlayerData PlayerData;
    public ClockContext ClockContext;
    public List<InventorySlotInfo> InventorySlotsInfo = new();
    public WorldSaveData WorldSaveData;
    public List<EventHappenData> EventHappens = new();
    public List<ActiveQuest> ActiveQuests = new();
    public List<EnemyPosition> EnemyPositions = new();
}