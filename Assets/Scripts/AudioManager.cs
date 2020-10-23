using UnityEngine.Audio;
using System;
using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

    [SerializeField] private Sound[] sounds;

    private bool isAudioOn;

    public bool ToggleAudio()
    {
        isAudioOn = !isAudioOn;
        return isAudioOn;
    }

	private void Awake()
	{
		if (instance != null) Destroy(gameObject);
		else instance = this;

		foreach (var sound in sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;
        }
	}

	public void Play(string sound)
	{
		if (!isAudioOn) return;
		var currentSound = Array.Find(sounds, item => item.name == sound);

		if (currentSound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		currentSound.source.volume = currentSound.volume * (1f + UnityEngine.Random.Range(-currentSound.volumeVariance / 2f, currentSound.volumeVariance / 2f));
		currentSound.source.pitch = currentSound.pitch * (1f + UnityEngine.Random.Range(-currentSound.pitchVariance / 2f, currentSound.pitchVariance / 2f));
		currentSound.source.Play();
	}

	public void Stop(string sound)
	{
		var currentSound = Array.Find(sounds, item => item.name == sound);
		if (currentSound == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		currentSound.source.Stop();
	}
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = .75f;
    [Range(0f, 1f)] public float volumeVariance = .1f;

    [Range(.1f, 3f)] public float pitch = 1f;
    [Range(0f, 1f)] public float pitchVariance = .1f;

    public bool loop = false;

    [HideInInspector] public AudioSource source;

}
