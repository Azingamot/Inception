
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public struct TilemapSaveData
{
    public string name;
    public float[] position;
    public List<TileData> tiles;
}