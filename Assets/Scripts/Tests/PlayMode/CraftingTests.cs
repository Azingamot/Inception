using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class CraftingTests
{
    private GameObject go;
    private InventoryController controller;
    private CraftItem craftItem;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject("InventoryController_Test");
        go.SetActive(false);
        controller = go.AddComponent<InventoryController>();
        craftItem = go.AddComponent<CraftItem>();

        InventoryUI ui = go.AddComponent<InventoryUI>();

        var uiField = typeof(InventoryController).GetField("ui", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        uiField.SetValue(controller, ui);

        var dataField = typeof(InventoryController).GetProperty("Data", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        var instanceProperty = typeof(InventoryController).GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);

        instanceProperty.SetValue(controller, controller);

        InventoryData data = new InventoryData();
        for (int i = 0; i < 23; i++)
        {
            data.AddData(new InventorySlotData(i));
        }

        dataField.SetValue(controller, data);

        controller.Data.Slots[0].ItemInSlot = Resources.Load<BasicItem>("Items/Basic/Wood Item");
        controller.Data.Slots[0].Count = 6;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [UnityTest]
    public IEnumerator CraftItem_ReturnsTrue()
    {
        CraftRecipe recipe = Resources.Load<CraftRecipe>("Recipes/Default/CraftingTable");
        bool result = craftItem.CreateItem(recipe, null, Vector2.zero);

        Assert.IsTrue(result);

        Assert.That(controller.Data.Slots[0].ItemInSlot, Is.Null);

        yield return null;
    }
}
