using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool isLooped;
        [Range(0f, 5f)] public float fadeDuration = 1f;
        
        [HideInInspector] public AudioSource source;
    }

    [Header("Sound Settings")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private void Awake()
    {
        InitializeSounds();
    }

    private void InitializeSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLooped;
            
            soundDictionary[s.name] = s;
        }
    }
    
    public void PlayOneShot(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null && s.clip != null)
            {
                s.source.PlayOneShot(s.clip, s.volume);
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' has no audio source or clip assigned.");
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
        }
    }
    
    public void PlayLooped(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null && s.clip != null)
            {
                if (!s.source.isPlaying)
                {
                    s.source.loop = true;
                    s.source.volume = 0f;
                    s.source.Play();
                    StartCoroutine(FadeInCoroutine(s.source, s.volume, s.fadeDuration));
                }
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' has no audio source or clip assigned.");
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
        }
    }


    public void StopLooped(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null && s.source.isPlaying)
            {
                StartCoroutine(FadeOutCoroutine(s.source, s.fadeDuration));
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
        }
    }
    
    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }
    
    public bool IsPlaying(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            return s.source != null && s.source.isPlaying;
        }
        return false;
    }


    public void SetVolume(string soundName, float volume)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null)
            {
                s.volume = Mathf.Clamp01(volume);
                s.source.volume = s.volume;
            }
        }
    }
    
    public void FadeOut(string soundName, float duration)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null && s.source.isPlaying)
            {
                StartCoroutine(FadeOutCoroutine(s.source, duration));
            }
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }

    private System.Collections.IEnumerator FadeInCoroutine(AudioSource source, float targetVolume, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
    
    public void SetPitch(string soundName, float pitch)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            if (s.source != null)
            {
                s.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
                s.source.pitch = s.pitch;
            }
        }
    }

}
