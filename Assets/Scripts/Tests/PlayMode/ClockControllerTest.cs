using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class ClockControllerTest
{
    private GameObject go;
    private ClockController controller;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject("ClockController_Test");
        go.SetActive(false);
        controller = go.AddComponent<ClockController>();
    }

    [UnityTest]
    public IEnumerator ClockController_TimeChanges()
    {
        ClockController.Hours = 22;

        var onTimeChangedField = typeof(ClockController).GetField("onTimeChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        onTimeChangedField.SetValue(controller, new UnityEvent<ClockContext>());

        var controlMethod = typeof(ClockController).GetMethod("ControlDayTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        controlMethod.Invoke(controller, null);

        Assert.That(ClockController.DayTime == DayTime.Night);

        ClockController.Hours = 8;

        controlMethod.Invoke(controller, null);

        Assert.That(ClockController.DayTime == DayTime.Day);

        yield return null;
    }
}
