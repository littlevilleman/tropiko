using UnityEngine;


namespace Client
{
    public enum ESound
    {
        Locate, Combo_01, Combo_02, Combo_03, Combo_04, Defeat, GeneratePiece, RotatePiece, GenerateTomb, BreakTomb
    }

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] public AudioSource MusicSource;
        [SerializeField] public AudioSource[] Sources;

        public void PlaySound(ESound sound)
        {
            AudioSource source = FindAvailableSource();
            if (source != null)
            {
                source.clip = GameManager.Instance.Config.GetSoundClip(sound);
                source.Play();
            }
        }

        private AudioSource FindAvailableSource()
        {
            foreach (AudioSource source in Sources)
                if (!source.isPlaying)
                    return source;

            return null;
        }

        public void PlayGameMusic()
        {
            MusicSource.clip = GameManager.Instance.Config.GetMusicClip();
            MusicSource.loop= true;
            MusicSource.Play();
        }
    }

}