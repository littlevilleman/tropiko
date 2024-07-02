using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/AudioConfig", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private List<SoundData> clips;

        [SerializeField] private AudioClip mainMenuClip;
        [SerializeField] private AudioClip gameMusicClip;

        public AudioClip GetSoundClip(ESound sound)
        {
            return clips.Where(x => x.sound == sound).FirstOrDefault().clip;
        }

        internal AudioClip GetMusicClip()
        {
            return gameMusicClip;
        }
    }

    [Serializable]
    public class SoundData
    {
        public ESound sound;
        public AudioClip clip;
    }
}