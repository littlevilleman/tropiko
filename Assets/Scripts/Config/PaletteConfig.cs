using UnityEngine;


namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/PaletteConfig", order = 1)]
    public class PaletteConfig : ScriptableObject
    {
        [SerializeField] private Color[] colors;

        public Color GetColor(int index)
        {
            return colors[index];
        }
    }
}
