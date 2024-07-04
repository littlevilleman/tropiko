using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        [SerializeField] public SoundManager Audio;
        [SerializeField] public ConfigManager Config;
        [SerializeField] public UIManager UI;
        [SerializeField] public CameraManager Camera;
        [SerializeField] public MatchManager Match;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            instance = this;
        }

        void Start()
        {
            UI.DisplayScreen<HomeScreen>();
        }
    }
}