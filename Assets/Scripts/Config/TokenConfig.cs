using Core;
using UnityEditor.Animations;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/TokenConfig", order = 1)]
    public class TokenConfig : ScriptableObject
    {
        public string Name;
        public ETokenType Type;
        public Color color;
        public AnimatorController animator;
    }
}