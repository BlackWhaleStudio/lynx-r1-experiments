using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Lynx
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Button))]
    public class LynxThemedButton : LynxThemed
    {
        [Header("LynxThemedButton")]
        public LynxThemeColorSetSO.ColorType normalColorType;
        public LynxThemeColorSetSO.ColorType highlightedColorType;
        public LynxThemeColorSetSO.ColorType pressedColorType;
        public LynxThemeColorSetSO.ColorType selectedColorType;
        public LynxThemeColorSetSO.ColorType disabledColorType;

        [System.Serializable]
        public class LynxButtonColors
        {
            public Color ColorNormal;
            public Color ColorHighlighted;
            public Color ColorPressed;
            public Color ColorSelected;
            public Color ColorDisabled;
        }
        public LynxButtonColors buttonComponentBaseColors;
        public List<LynxButtonColors> lynxThemedButtonColorsList;

        private Button button;

        private bool buttonBaseColorsSaved = false;
        private bool buttonBaseColorsReset = false;



        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //LynxThemeManager.Instance.ThemeUpdateEvent -= UpdateTheme;
        }

        private void OnValidate()
        {
            //Debug.Log("LynxThemedButton.OnValidate()");
            button = GetComponent<Button>();
            UpdateLynxThemedColorsList();

            if (useLynxTheme)
            {
                if (buttonBaseColorsReset) buttonBaseColorsReset = false;
                if (!buttonBaseColorsSaved) SaveButtonBaseColors();
                SetButtonColorsToCurrentThemeColors();
            }
            else
            {
                if (buttonBaseColorsSaved) buttonBaseColorsSaved = false;
                if (!buttonBaseColorsReset) ResetButtonBaseColors();
                //SetButtonColorsToSavedBaseColors();
            }
        }



        public void SetButtonColors(LynxButtonColors lynxButtonColors)
        {
            if (button == null) button = GetComponent<Button>();

            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = lynxButtonColors.ColorNormal;
            colorBlock.highlightedColor = lynxButtonColors.ColorHighlighted;
            colorBlock.pressedColor = lynxButtonColors.ColorPressed;
            colorBlock.selectedColor = lynxButtonColors.ColorSelected;
            colorBlock.disabledColor = lynxButtonColors.ColorDisabled;

            colorBlock.colorMultiplier = button.colors.colorMultiplier;
            colorBlock.fadeDuration = button.colors.fadeDuration;
            button.colors = colorBlock;
        }
        public void SetButtonColorsToSavedBaseColors()
        {
            //Debug.Log("LynxThemedButton.SetButtonColorsToSavedBaseColors()");
            SetButtonColors(buttonComponentBaseColors);
        }
        public void SetButtonColorsToCurrentThemeColors()
        {
            //Debug.Log("LynxThemedButton.SetButtonColorsToCurrentThemeColors()");
            CheckLynxThemeManagerInstance();
            SetButtonColors(lynxThemedButtonColorsList[LynxThemeManager.Instance.lynxThemeColorSetCurrentIndex]);
        }


        public override void UpdateTheme()
        {
            if (useLynxTheme) SetButtonColorsToCurrentThemeColors();
        }
        public override void UpdateLynxThemedColorsList()
        {
            lynxThemedButtonColorsList = new List<LynxButtonColors>();
            CheckLynxThemeManagerInstance();
            if (LynxThemeManager.Instance == null || LynxThemeManager.Instance.lynxThemeColorSets.Count == 0)
            {
                useLynxTheme = false;
                return;
            }


            for (int i = 0; i < LynxThemeManager.Instance.lynxThemeColorSets.Count; i++)
            {
                LynxThemeColorSetSO lynxThemeColorSet = LynxThemeManager.Instance.lynxThemeColorSets[i];
                LynxButtonColors lynxThemedButtonColors = new LynxButtonColors();
                lynxThemedButtonColors.ColorNormal = lynxThemeColorSet.GetColorFromEnum(normalColorType);
                lynxThemedButtonColors.ColorHighlighted = lynxThemeColorSet.GetColorFromEnum(highlightedColorType);
                lynxThemedButtonColors.ColorPressed = lynxThemeColorSet.GetColorFromEnum(pressedColorType);
                lynxThemedButtonColors.ColorSelected = lynxThemeColorSet.GetColorFromEnum(selectedColorType);
                lynxThemedButtonColors.ColorDisabled = lynxThemeColorSet.GetColorFromEnum(disabledColorType);
                lynxThemedButtonColorsList.Add(lynxThemedButtonColors);
            }
        }


        public void SaveButtonBaseColors()
        {
            buttonComponentBaseColors.ColorNormal = button.colors.normalColor;
            buttonComponentBaseColors.ColorHighlighted = button.colors.highlightedColor;
            buttonComponentBaseColors.ColorPressed = button.colors.pressedColor;
            buttonComponentBaseColors.ColorSelected = button.colors.selectedColor;
            buttonComponentBaseColors.ColorDisabled = button.colors.disabledColor;
            buttonBaseColorsSaved = true;
        }
        public void ResetButtonBaseColors()
        {
            SetButtonColorsToSavedBaseColors();
            buttonBaseColorsReset = true;
        }
    }
}