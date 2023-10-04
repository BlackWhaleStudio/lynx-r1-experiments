using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lynx
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LynxThemedTextMeshProUGUI : LynxThemed
    {
        [Header("LynxThemedText")]
        public LynxThemeColorSetSO.ColorType colorType;

        public Color textComponentBaseColor;
        public List<Color> lynxThemedTextColorList;

        private TextMeshProUGUI text;

        private bool textBaseColorSaved = false;
        private bool textBaseColorReset = false;



        protected override void Awake()
        {
            base.Awake();
            text = GetComponent<TextMeshProUGUI>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //LynxThemeManager.Instance.ThemeUpdateEvent -= UpdateTheme;
        }

        private void OnValidate()
        {
            //Debug.Log("LynxThemedText.OnValidate()");
            text = GetComponent<TextMeshProUGUI>();
            UpdateLynxThemedColorsList();

            if (useLynxTheme)
            {
                if (textBaseColorReset) textBaseColorReset = false;
                if (!textBaseColorSaved) SaveTextBaseColor();
                SetTextColorToCurrentThemeColors();
            }
            else
            {
                if (textBaseColorSaved) textBaseColorSaved = false;
                if (!textBaseColorReset) ResetTextBaseColor();
                //SetTextColorsToSavedBaseColors();
            }
        }



        public void SetTextColor(Color imageColor)
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
            text.color = imageColor;
        }
        public void SetTextColorsToSavedBaseColors()
        {
            //Debug.Log("LynxThemedText.SetTextColorsToSavedBaseColors()");
            SetTextColor(textComponentBaseColor);
        }
        public void SetTextColorToCurrentThemeColors()
        {
            //Debug.Log("LynxThemedText.SetTextColorsToCurrentThemeColors()");
            CheckLynxThemeManagerInstance();
            SetTextColor(lynxThemedTextColorList[LynxThemeManager.Instance.lynxThemeColorSetCurrentIndex]);
        }


        public override void UpdateTheme()
        {
            if (useLynxTheme) SetTextColorToCurrentThemeColors();
        }
        public override void UpdateLynxThemedColorsList()
        {
            lynxThemedTextColorList = new List<Color>();
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

                lynxThemedTextColorList.Add(imageColor);
            }
        }


        public void SaveTextBaseColor()
        {
            textComponentBaseColor = text.color;

            textBaseColorSaved = true;
        }
        public void ResetTextBaseColor()
        {
            SetTextColorsToSavedBaseColors();
            textBaseColorReset = true;
        }

    }
}