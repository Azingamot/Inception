using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Default Values")]
    [SerializeField] private float defaultSfx = 0;
    [SerializeField] private float defaultMusic = 0;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Language")]
    [SerializeField] private TMP_Dropdown languageDropdown;

    [Header("Fullscreen")]
    [SerializeField] private Toggle fullscreenTogle;

    private List<string> languages = new();
    private List<Resolution> resolutions = new();

    private void Awake()
    {
        fullscreenTogle.isOn = Screen.fullScreen;

        LoadLanguages();
        LoadLanguagesDropdown();
        LoadResolutions();

        SetSFX(ReceiveFloatFromPlayerPrefs("SFX", defaultSfx));
        SetMusic(ReceiveFloatFromPlayerPrefs("Music", defaultMusic));
        SetLanguage(ReceiveStringFromPlayerPrefs("Language", "English"));
        SetResolution(ReceiveIntFromPlayerPrefs("Resolution", CurrentResolutionIndex()));
    }

    private void LoadResolutions()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions.ToList();
        foreach (var resolution in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + "x" + resolution.height));
        }
    }

    private int CurrentResolutionIndex()
    {
        return resolutions.FindIndex(u => u.height == Screen.currentResolution.height && u.width == Screen.currentResolution.width);
    }

    public void SetResolution(int index)
    {
        if (index == -1 || index > resolutions.Count - 1)
            return;
        Resolution selected = resolutions[index];
        Screen.SetResolution(selected.width, selected.height, Screen.fullScreen);
        Debug.Log(selected);
        resolutionDropdown.value = index;
        PlayerPrefs.SetInt("Resolution",index);
    }

    private void LoadLanguages()
    {
        foreach (string language in TranslationHandler.LanguagesPathMap.Keys)
        {
            languages.Add(language);
        }
    }

    private void LoadLanguagesDropdown()
    {
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(languages);
    }

    public void SetSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFX", volume);
        sfxMixer.SetFloat("Volume", volume);
        sfxSlider.value = volume;
    }

    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
        musicMixer.SetFloat("Volume", volume);
        musicSlider.value = volume;
    }

    private void SetLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);
        languageDropdown.value = languages.FindIndex(u => u == language);
    }

    public void SetLanguage(int index)
    {
        SetLanguage(languageDropdown.options[index].text);
    }

    public void SetFullscreen(bool fullScreen)
    {
        Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.MaximizedWindow;
    }

    private float ReceiveFloatFromPlayerPrefs(string floatName, float defaultValue = 0)
    {
        if (PlayerPrefs.HasKey(floatName))
            return PlayerPrefs.GetFloat(floatName);
        else 
            return defaultValue;
    }

    private int ReceiveIntFromPlayerPrefs(string intName, int defaultValue = -1)
    {
        if (PlayerPrefs.HasKey(intName))
            return PlayerPrefs.GetInt(intName);
        else
            return defaultValue;
    }

    private string ReceiveStringFromPlayerPrefs(string stringName, string defaultValue = null)
    {
        if (PlayerPrefs.HasKey(stringName))
            return PlayerPrefs.GetString(stringName);
        else
            return defaultValue;
    }
}
