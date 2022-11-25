using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Sources
{
    public class AudioManager : MonoBehaviour
    {
        private AudioMixerGroup soundGroup;
        private AudioMixerGroup _musicGroup;

        private Dictionary<string, AudioData> sounds = new Dictionary<string, AudioData>();
        private Dictionary<string, AudioData> music = new Dictionary<string, AudioData>();

        private List<AudioSource> soundSources = new List<AudioSource>();

        private AudioSource musicSource1;
        private AudioSource musicSource2;

        private bool isPlayAllMusic = false;

        public bool IsSoundMute => PlayerPrefs.GetInt("SoundVolume", 1) != 1;
        public bool IsMusicMute => PlayerPrefs.GetInt("MusicVolume", 1) != 1;

        [Inject]
        private void Construct(AudioConfig audioConfig)
        {
            if (audioConfig == null)
            {
                Debug.LogError("Can't find audio list");
                return;
            }

            soundGroup = audioConfig.SoundMixerGroup;
            _musicGroup = audioConfig.MusicMixerGroup;

            foreach (AudioData sound in audioConfig.Sounds)
                sounds[sound.Clip.name] = sound;

            foreach (AudioData music in audioConfig.Music)
                this.music[music.Clip.name] = music;

            soundSources.Add(gameObject.AddComponent<AudioSource>());
            soundSources[0].outputAudioMixerGroup = soundGroup;

            musicSource1 = gameObject.AddComponent<AudioSource>();
            musicSource1.outputAudioMixerGroup = _musicGroup;
            musicSource1.loop = true;

            musicSource2 = gameObject.AddComponent<AudioSource>();
            musicSource2.outputAudioMixerGroup = _musicGroup;
            musicSource2.loop = false;

            SetSound2DIsOn(PlayerPrefs.GetInt("SoundVolume", 1) == 1);
            SetMusic2DIsOn(PlayerPrefs.GetInt("MusicVolume", 1) == 1);
        }

        #region Sound
        public void PlaySound(AudioClip clip)
        {
            PlaySound(clip.name);
        }

        public void PlaySound(string soundName)
        {
            if (!sounds.ContainsKey(soundName))
            {
                Debug.LogError("There is no sound with name " + soundName);
                return;
            }

            AudioData audio = sounds[soundName];
            var source = GetAudioSource();
            source.PlayOneShot(audio.Clip, audio.Volume);
        }

        private AudioSource GetAudioSource()
        {
            if (soundSources.Count > 0)
                foreach (var sound in soundSources)
                    if (!sound.isPlaying)
                    {
                        var returned = sound;
                        return returned;
                    }

            var audioSource = gameObject.AddComponent<AudioSource>();
            soundSources.Add(audioSource);
            audioSource.outputAudioMixerGroup = soundGroup;
            return audioSource;
        }
        #endregion Sound

        #region Music
        public void PlayMusic1(string musicName)
        {
            musicSource1.clip = music[musicName].Clip;
            musicSource1.volume = music[musicName].Volume;
            musicSource1.Play();
        }

        public void PlayMusic2(string musicName)
        {
            musicSource2.loop = true;
            StopAllMusic();
            musicSource2.clip = music[musicName].Clip;
            musicSource2.volume = music[musicName].Volume;
            musicSource2.Play();
        }

        public void PlayAllMusic()
        {
            isPlayAllMusic = true;
            StartCoroutine(PlayAllMusicRoutine());
        }

        private void StopAllMusic()
        {
            isPlayAllMusic = false;
        }

        private IEnumerator PlayAllMusicRoutine()
        {
            if (music == null || music.Count == 0)
            {
                Debug.LogError("There is no music");
                yield break;
            }

            musicSource2.loop = false;
            while (isPlayAllMusic)
            {
                foreach (var audioData in music.Values)
                {
                    musicSource2.clip = audioData.Clip;
                    musicSource2.volume = audioData.Volume;
                    musicSource2.Play();

                    while (musicSource2.isPlaying)
                        yield return null;
                }
            }
        }
        #endregion Music

        #region Volume
        public void SetSound2DIsOn(bool isOn)
        {
            Debug.Log("Sound: " + isOn);
            PlayerPrefs.SetInt("SoundVolume", isOn ? 1 : 0);
            soundGroup.audioMixer.SetFloat("SoundVolume", isOn ? 0f : -80f);
        }

        public void SetMusic2DIsOn(bool isOn)
        {
            Debug.Log("Music: " + isOn);
            PlayerPrefs.SetInt("MusicVolume", isOn ? 1 : 0);
            _musicGroup.audioMixer.SetFloat("MusicVolume", isOn ? 0f : -80f);
        }

        public void SetSoundVolume(float volume)
        {
            //Debug.Log("Sound: " + isOn);
            soundGroup.audioMixer.SetFloat("SoundVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            //Debug.Log("Music: " + isOn);
            _musicGroup.audioMixer.SetFloat("MusicVolume", volume);
        }
        #endregion Volume
    }
}

