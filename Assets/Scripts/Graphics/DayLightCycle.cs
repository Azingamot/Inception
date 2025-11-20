using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayLightCycle : MonoBehaviour
{
    [SerializeField] private List<DaylightState> states = new();
    [SerializeField] private Light2D sunlight;
    private DaylightState currentState;

    private void Awake()
    {
        currentState = states[0];
        ChangeState(currentState);
    }

    public void OnClockTick(ClockContext context)
    {
        foreach (DaylightState state in states)
        {
            if (!currentState.Equals(state) && context.Hours >= state.minHours && context.Hours <= state.maxHours)
            {
                currentState = state;
                ChangeState(state);
            }
        }
    }

    private void ChangeState(DaylightState state)
    {
        StartCoroutine(SmoothColorChange(state.color));
    }

    private IEnumerator SmoothColorChange(Color color)
    {
        float elapsedTime = 0;
        while (sunlight.color != color)
        {
            elapsedTime += Time.deltaTime;
            sunlight.color = Color.Lerp(sunlight.color, color, elapsedTime * 0.1f);
            yield return new WaitForFixedUpdate();
        }
    }
}
