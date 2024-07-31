using System;
using UnityEngine;
using System.Collections;

namespace Utils
{
    public class AudioManager : MonoBehaviour
    {
        public Sound themeSound;
        public bool skipAllSounds = false;

        Sound[] Sounds { get; set; }
        private Sound CurrentSound { get; set; }
        public static AudioManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            Sounds = Resources.LoadAll<Sound>("Sounds");
            foreach (Sound sound in Sounds)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();

                source.clip = sound.clip;
                source.volume = sound.volume;
                source.pitch = sound.pitch;
                source.loop = sound.loop;

                sound.source = source;
            }
        }

        void Start() => Play(themeSound, setAsCurrentSound: true);

        public void Play(string name, bool pauseTheme = false)
        {
            if (skipAllSounds) return;

            Sound s = Array.Find(Sounds, sound => sound.name == name);
            if (!s)
            {
                Debug.LogWarning($"Sound {name} not found");
                return;
            }
            if (pauseTheme) PauseThemeForTime(s.source.clip.length);
            s.source.Play();
        }

        public void Play(Sound sound, bool setAsCurrentSound = false, bool stopCurrentSound = true)
        {

            if (skipAllSounds) return;

            if (stopCurrentSound && CurrentSound)
                CurrentSound.source.Pause();
            if (setAsCurrentSound)
                CurrentSound = sound;
            sound.source.Play();
        }

        public void Mute()
        {
            Sounds.ForEach(s => s.source.mute = true);
        }

        public void UnMute()
        {
            Sounds.ForEach(s => s.source.mute = false);
        }

        public void ToggleMute()
        {
            Sounds.ForEach(s => s.source.mute = !s.source.mute);
        }

        public void PauseCurrentSound()
        {
            CurrentSound.source.Pause();
        }

        public void PauseCurrentSoundForTime(float time)
        {
            CurrentSound.source.Pause();
            StartCoroutine(WaitAndUnpause(CurrentSound.source, time));
        }

        public void PauseThemeForTime(float time)
        {
            themeSound.source.Pause();
            StartCoroutine(WaitAndUnpause(CurrentSound.source, time));
        }

        IEnumerator WaitAndUnpause(AudioSource source, float time)
        {
            yield return new WaitForSeconds(time);
            if (!skipAllSounds)
                source.UnPause();
        }
    }
}