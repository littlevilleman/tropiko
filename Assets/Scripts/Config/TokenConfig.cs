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
        public Color borderColor;
        public AnimatorController animator;
        public ETokenType Type => type;

        public IComboStrategy Combo => type == ETokenType.BOMB ? new TypeComboStrategy() : new LineComboStrategy();
        public IBreakStrategy Break => type == ETokenType.TOMB ? new CountBreakStrategy(1) : new BasicBreakStrategy();
        public IFallStrategy Fall => new BasicFallStrategy();
    }
}