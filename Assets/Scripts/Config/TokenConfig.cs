using Core;
using UnityEditor.Animations;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/TokenConfig", order = 1)]
    public class TokenConfig : ScriptableObject, ITokenConfig
    {
        public string Name;
        public ETokenType type;
        public Color color;
        public AnimatorController animator;
        public ETokenType Type => type;

        public IComboStrategy GetComboStrategy()
        {
            return type == ETokenType.BOMB ? new TypeComboStrategy() : new LineComboStrategy();
        }
    }
}