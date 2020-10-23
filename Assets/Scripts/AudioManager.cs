using SaveSystem;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        if (!EasySave.HasKey<bool>("Mute"))
        {
            EasySave.Save("Mute", false);
        }
        else
        {
            isAudioOff = EasySave.Load<bool>("Mute");
        }
    }
    [Header("Sounds")]
    [SerializeField] private AudioClip blockPlaceSound;
    [SerializeField] private AudioClip wordDeleteSound;
    [SerializeField] private AudioClip blocksApperanceSound;
    [SerializeField] private AudioClip blockReturnSound;
    [SerializeField] private AudioClip tapSound;
    [Header("Sources")]
    [SerializeField] private AudioSource soundsSource;

    private bool isAudioOff
    {
        get => soundsSource.mute;
        set => soundsSource.mute = value;
    }


    public void BlockPlace() => soundsSource.PlayOneShot(blockPlaceSound);
    public void WordDelte() => soundsSource.PlayOneShot(wordDeleteSound);
    public void BlocksApperance() => soundsSource.PlayOneShot(blocksApperanceSound);
    public void BlockReturn() => soundsSource.PlayOneShot(blockReturnSound);
    public void Tap() => soundsSource.PlayOneShot(tapSound);

    public bool ToggleAudio()
    {
        isAudioOff = !isAudioOff;
        return isAudioOff;
    }
}