using Config;
using Core;
using System;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField] private TokenConfig[] tokens;
        [SerializeField] private AudioConfig sounds;

        public TokenConfig GetTokenConfig(ETokenType type)
        {
            return tokens.Where(x => x.Type == type).FirstOrDefault();
        }

        public AudioClip GetSoundClip(ESound sound)
        {
            return sounds.GetSoundClip(sound);
        }

        public AudioClip GetMusicClip()
        {
            return sounds.GetMusicClip();
        }
    }
}