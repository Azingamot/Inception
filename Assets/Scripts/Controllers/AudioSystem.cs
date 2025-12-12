using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ClipType
{
    None,
    HeavySwing,
    LightSwing,
    Eat,
    Drink,
    Place,
    Explode,
    PlayerHurt,
    EnemyHurt
}

public enum AudioType
{
    SFX,
    Music
}

public class AudioSystem : MonoBehaviour
{
    private static AudioSystem instance;

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Clips")]
    [SerializeField] private List<SoundList> soundList;

    [Header("Music")]
    [SerializeField] private AudioClip daytimeMusic;
    [SerializeField] private AudioClip nighttimeMusic;

    [Header("Pitch Randomizer")]
    [SerializeField] private float lowerThreshold;
    [SerializeField] private float upperThreshold;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        DaytimeChanged(ClockController.GetClockContext());
    }

    public static void PlaySound(ClipType clipType, float volume = 1, AudioType audioType = AudioType.SFX)
    {
        AudioClip clip = FindClip(clipType);

        if (clip == null)
            return;

        if (audioType == AudioType.SFX)
            PlaySFX(clip, volume);
        else
            PlayMusic(clip, volume);
    }

    public void DaytimeChanged(ClockContext context)
    {
        if (context.DayTime == DayTime.Day)
            PlayMusic(daytimeMusic, 1);
        else
            PlayMusic(nighttimeMusic, 1);
    }

    private static void PlaySFX(AudioClip clip, float volume)
    {
        instance.sfxSource.pitch = UnityEngine.Random.Range(instance.lowerThreshold, instance.upperThreshold);
        instance.sfxSource.PlayOneShot(clip,volume);
    }

    private static void PlayMusic(AudioClip clip, float volume)
    {
        instance.StartCoroutine(MusicFadeOut(clip));
    }

    private static IEnumerator MusicFadeOut(AudioClip clip)
    {
        float volume = 1;
        float elapsedTime = 0;
        while (volume != 0)
        {
            elapsedTime += Time.deltaTime;
            volume = Mathf.Lerp(volume, 0, elapsedTime);
            yield return null;
        }
        instance.musicSource.clip = clip;
        instance.musicSource.Play();
        elapsedTime = 0;
        while (volume != 1)
        {
            elapsedTime += Time.deltaTime;
            volume = Mathf.Lerp(volume, 1, elapsedTime);
            yield return null;
        }
    }

    private static AudioClip FindClip(ClipType clipType)
    {
        SoundList listElement = instance.soundList.FirstOrDefault(u => u.ClipType == clipType);

        if (listElement.AudioClip.Count > 0)
            return listElement.AudioClip[UnityEngine.Random.Range(0, listElement.AudioClip.Count)];
        return null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (soundList.Count != sizeof(ClipType))
        {
            foreach (ClipType value in Enum.GetValues(typeof(ClipType)))
            {
                if(!soundList.Any(u => u.ClipType == value))
                    soundList.Add(new SoundList() {Name = value.ToString(), ClipType = value, AudioClip = null });
            }
        }
    }
#endif
}

[System.Serializable]
public struct SoundList
{
    [HideInInspector] public string Name;
    [HideInInspector] public ClipType ClipType;
    public List<AudioClip> AudioClip;
}