using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class WorldSaveData
{
    [SerializeField] private long timecode;
    public DateTimeOffset saveTime
    {
        get
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timecode);
        }
        set
        {
            timecode = value.ToUnixTimeMilliseconds();
        }
    }
    public List<TilemapSaveData> tilemaps = new();
}