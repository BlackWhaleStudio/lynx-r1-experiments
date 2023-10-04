using UnityEngine;

namespace Lynx
{
    [CreateAssetMenu(fileName = "LynxThemeColorSet", menuName = "ScriptableObjects/LynxThemeColorSet", order = 1)]
    public class LynxThemeColorSetSO : ScriptableObject
    {
        public string themeName;
        public enum ColorType
        {
            Surface1,
            Surface2,
            Surface3,
            Surface4,
            OnSurface,
            Primary,
            PrimaryLight,
            PrimaryDark,
            OnPrimary,
            Secondary,
            SecondaryLight,
            SecondaryDark,
            OnSecondary,
            Tertiary,
            TertiaryLight,
            TertiaryDark,
            OnTertiary,
        }

        [Header("Surface")]
        public Color Surface1;
        public Color Surface2;
        public Color Surface3;
        public Color Surface4;
        public Color OnSurface;

        [Header("Primary")]
        public Color Primary;
        public Color PrimaryLight;
        public Color PrimaryDark;
        public Color OnPrimary;

        [Header("Secondary")]
        public Color Secondary;
        public Color SecondaryLight;
        public Color SecondaryDark;
        public Color OnSecondary;

        [Header("Tertiary")]
        public Color Tertiary;
        public Color TertiaryLight;
        public Color TertiaryDark;
        public Color OnTertiary;



        private void OnValidate()
        {
            LynxThemeManager.CheckLynxThemeManagerInstance();
            if (LynxThemeManager.Instance) LynxThemeManager.Instance.UpdateLynxThemedComponents();
        }



        public Color GetColorFromEnum(ColorType colorType)
        {
            if (colorType == ColorType.Surface1) return Surface1;
            else if (colorType == ColorType.Surface2) return Surface2;
            else if (colorType == ColorType.Surface3) return Surface3;
            else if (colorType == ColorType.Surface4) return Surface4;
            else if (colorType == ColorType.OnSurface) return OnSurface;
            else if (colorType == ColorType.Primary) return Primary;
            else if (colorType == ColorType.PrimaryLight) return PrimaryLight;
            else if (colorType == ColorType.PrimaryDark) return PrimaryDark;
            else if (colorType == ColorType.OnPrimary) return OnPrimary;
            else if (colorType == ColorType.Secondary) return Secondary;
            else if (colorType == ColorType.SecondaryLight) return SecondaryLight;
            else if (colorType == ColorType.SecondaryDark) return SecondaryDark;
            else if (colorType == ColorType.OnSecondary) return OnSecondary;
            else if (colorType == ColorType.Tertiary) return Tertiary;
            else if (colorType == ColorType.TertiaryLight) return TertiaryLight;
            else if (colorType == ColorType.TertiaryDark) return TertiaryDark;
            else if (colorType == ColorType.OnTertiary) return OnTertiary;
            else return Color.white;
        }

    }
}