using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTest
{
	private GameObject go;
	private InventoryController controller;

	[SetUp]
	public void SetUp()
	{
		go = new GameObject("InventoryController_Test");
		go.SetActive(false);
		controller = go.AddComponent<InventoryController>();

		var dataField = typeof(InventoryController).GetProperty("Data", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

		InventoryData data = new InventoryData();
		for (int i = 0; i < 23; i++)
		{
			data.AddData(new InventorySlotData(i));
		}

		dataField.SetValue(controller, data);

		controller.Data.Slots[0].ItemInSlot = ScriptableObject.CreateInstance<BasicItem>(); ;
		controller.Data.Slots[0].Count = 3;
		controller.Data.Slots[1].ItemInSlot = ScriptableObject.CreateInstance<BasicItem>();
		controller.Data.Slots[1].Count = 1;
	}

	[TearDown]
	public void TearDown()
	{
		Object.DestroyImmediate(go);
	}

	[UnityTest]
	public IEnumerator Save_Returns_Correct_Slots()
	{
		List<InventorySlotInfo> result = controller.Save();

		Assert.That(result.Count, Is.EqualTo(controller.Data.Slots.Count));
		Assert.That(result[0].slotIndex, Is.EqualTo(controller.Data.Slots[0].Index));
		Assert.That(result[0].count, Is.EqualTo(3));
		Assert.That(result[1].count, Is.EqualTo(1));

		yield return null;
	}

	[UnityTest]
	public IEnumerator GetSelectedItem_ReturnsItem()
	{
        BasicItem wood = ScriptableObject.CreateInstance<BasicItem>();
		wood.name = "Wood";
		controller.Data.Slots[0].ItemInSlot = wood;

        var selectedField = typeof(InventoryController).GetField("selectedSlotIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		selectedField.SetValue(controller, 0);

		var selected = controller.GetSelectedItem();

        Assert.That(selected, Is.Not.Null);
        Assert.That(selected.name, Is.EqualTo("Wood"));

		yield return null;
    }
}
