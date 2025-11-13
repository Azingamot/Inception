using System.Collections.Generic;
using UnityEngine;

public class PauseBarUI : MonoBehaviour
{
	[SerializeField] private GameObject questsUI, settingsUI;
	private List<PauseBarButton> barButtons = new();

	private void OnEnable()
	{
		if (barButtons.Count == 0)
			LoadButtons();
	}

	public void DeselectButtons(PauseBarButton button)
	{
		foreach (PauseBarButton barButton in barButtons)
		{
			if (!barButton.Equals(button))
				barButton.ChangeSelection(false);
		}
	}

	private void LoadButtons()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).TryGetComponent<PauseBarButton>(out PauseBarButton button))
			{
				barButtons.Add(button);
				button.Initialize(this);
			}
		}
	}

	public void ChangeQuestsSelection(bool selected)
	{
		questsUI.SetActive(selected);
		if (selected)
			ChangeSettingsSelection(false);
	}

	public void ChangeSettingsSelection(bool selected)
	{
		settingsUI.SetActive(selected);
		if (selected)
			ChangeQuestsSelection(false);
	}
}
