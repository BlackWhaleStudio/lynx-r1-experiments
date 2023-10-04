using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lynx
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class LynxThemedImage : LynxThemed
    {
        [Header("LynxThemedImage")]
        public LynxThemeColorSetSO.ColorType colorType;

        public Color imageComponentBaseColor;
        public List<Color> lynxThemedImageColorList;

        private Image image;

        private bool imageBaseColorSaved = false;
        private bool imageBaseColorReset = false;



        protected override void Awake()
        {
            base.Awake();
            image = GetComponent<Image>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //LynxThemeManager.Instance.ThemeUpdateEvent -= UpdateTheme;
        }

        private void OnValidate()
        {
            //Debug.Log("LynxThemedImage.OnValidate()");
            image = GetComponent<Image>();
            UpdateLynxThemedColorsList();

            if (useLynxTheme)
            {
                if (imageBaseColorReset) imageBaseColorReset = false;
                if (!imageBaseColorSaved) SaveImageBaseColor();
                SetImageColorToCurrentThemeColors();
            }
            else
            {
                if (imageBaseColorSaved) imageBaseColorSaved = false;
                if (!imageBaseColorReset) ResetImageBaseColor();
                //SetImageColorsToSavedBaseColors();
            }

        }



        public override void UpdateTheme()
        {
            if (useLynxTheme) SetImageColorToCurrentThemeColors();
        }
        public override void UpdateLynxThemedColorsList()
        {
            lynxThemedImageColorList = new List<Color>();
            CheckLynxThemeManagerInstance();
            if (LynxThemeManager.Instance == null || LynxThemeManager.Instance.lynxThemeColorSets.Count == 0)
            {
                useLynxTheme = false;
                return;
            }

            for (int i = 0; i < LynxThemeManager.Instance.lynxThemeColorSets.Count; i++)
            {
                LynxThemeColorSetSO lynxThemeColorSet = LynxThemeManager.Instance.lynxThemeColorSets[i];
                Color imageColor = lynxThemeColorSet.GetColorFromEnum(colorType);

                lynxThemedImageColorList.Add(imageColor);
            }
        }


        public void SetImageColor(Color imageColor)
        {
            if (image == null) image = GetComponent<Image>();
            image.color = imageColor;
        }
        public void SetImageColorsToSavedBaseColors()
        {
            //Debug.Log("LynxThemedImage.SetImageColorsToSavedBaseColors()");
            SetImageColor(imageComponentBaseColor);
        }
        public void SetImageColorToCurrentThemeColors()
        {
            //Debug.Log("LynxThemedImage.SetImageColorsToCurrentThemeColors()");
            CheckLynxThemeManagerInstance();
            SetImageColor(lynxThemedImageColorList[LynxThemeManager.Instance.lynxThemeColorSetCurrentIndex]);
        }

        public void SaveImageBaseColor()
        {
            imageComponentBaseColor = image.color;

            imageBaseColorSaved = true;
        }
        public void ResetImageBaseColor()
        {
            SetImageColorsToSavedBaseColors();
            imageBaseColorReset = true;
        }
    }
}