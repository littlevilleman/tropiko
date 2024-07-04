using Config;
using Core;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField] private TokenConfig[] tokens;
        [SerializeField] private AudioConfig sounds;
        [SerializeField] private PaletteConfig palette;
        [SerializeField] private MatchConfig match;

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

        public Color GetGameColor(int index)
        {
            return palette.GetColor(index);
        }

        public MatchConfig GetMatchConfig()
        {
            return match;
        }
    }
}