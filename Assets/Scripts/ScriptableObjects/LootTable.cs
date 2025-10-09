using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/LootTable")]
public class LootTable: ScriptableObject, IEnumerable<LootElement>
{
    public List<LootElement> lootElements;

    public IEnumerator<LootElement> GetEnumerator()
    {
        for (int i = 0; i < lootElements.Count; i++)
        {
            yield return lootElements[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
}
