using System.Collections.Generic;
using UnityEngine;


namespace Lynx
{
    [ExecuteInEditMode]
    public class LynxThemeManager : MonoBehaviour
    {
        //INSPECTOR
        public List<LynxThemeColorSetSO> lynxThemeColorSets;
        public int lynxThemeColorSetCurrentIndex;


        //PUBLIC
        [HideInInspector] public LynxThemeColorSetSO lynxThemeColorSetCurrent;

        public delegate void LynxThemeUpdated();
        public LynxThemeUpdated ThemeUpdateEvent;

        //Singleton
        public static LynxThemeManager Instance { get; private set; }



        private void Awake()
        {
            SetupSingleton();
        }

        private void OnValidate()
        {
            if (Instance == null) SetupSingleton();

            if (lynxThemeColorSetCurrentIndex > lynxThemeColorSets.Count)
            {
                lynxThemeColorSetCurrentIndex = 0;
                lynxThemeColorSetCurrent = (lynxThemeColorSets.Count > 0) ? lynxThemeColorSets[0] : null;
            }

            UpdateLynxThemedComponents();
        }



        public void SetupSingleton()
        {
            //Debug.Log("LynxThemeManager.SetupSingleton()");
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }
        public static void CheckLynxThemeManagerInstance()
        {
            if (Instance != null) return;
            FindObjectOfType<LynxThemeManager>()?.SetupSingleton();
        }


        public void UpdateTheme(int themeIndex)
        {
            //Debug.Log("LynxThemeManager.UpdateTheme()");
            if (themeIndex > lynxThemeColorSets.Count) return;

            lynxThemeColorSetCurrent = lynxThemeColorSets[themeIndex];
            lynxThemeColorSetCurrentIndex = themeIndex;
            ThemeUpdateEvent.Invoke();
        }
        public void UpdateLynxThemedComponents()
        {
            //Debug.Log("LynxThemeManager.UpdateLynxThemedComponents()");
            LynxThemed[] list = FindObjectsOfType(typeof(LynxThemed)) as LynxThemed[];
            foreach (LynxThemed lynxThemeComponent in list)
            {
                lynxThemeComponent.UpdateLynxThemedColorsList();
                lynxThemeComponent.UpdateTheme();
            }
        }

    }
}