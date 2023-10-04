//   ==============================================================================
//   | LynxInterfaces (2023)                                                      |
//   |======================================                                      |
//   | LynxUI Editor Script                                                       |
//   | Script that displays interface creation shortcuts.                         |
//   ==============================================================================

#if LYNX_XRI
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
#endif

namespace Lynx.UI
{
    public class LynxUIEditor
    {
        private const string STR_SIMPLE_BUTTON = "LynxSimpleButton.prefab";
        private const string STR_TOGGLE_BUTTON = "LynxToggleButton.prefab";
        private const string STR_TIMER_BUTTON = "LynxTimerButton.prefab";
        private const string STR_SWITCH_BUTTON = "LynxSwitchButton.prefab";

#if LYNX_XRI
        /// <summary>
        /// Add a Canvas, setup for the Handtracking.
        /// </summary>
        [MenuItem("GameObject/Lynx/UI/Handtracking Canvas", false, 210)]
        public static void AddHandtrackingCanvas()
        {
            if (GameObject.FindObjectOfType<EventSystem>() == null)
            {
                InstantiateEventSystem();
            }

            InstantiateCanvas();
        }

        /// <summary>
        /// Call this function to instiante an Event System.
        /// </summary>
        /// <returns>New Event System GameObject.</returns>
        private static GameObject InstantiateEventSystem()
        {
            // Create a new GameObject to hold the EventSystem component
            GameObject eventSystemObject = new GameObject("EventSystem");

            // Add the EventSystem component to the GameObject
            eventSystemObject.AddComponent<EventSystem>();

            // Add the StandaloneInputModule component to the GameObject
            eventSystemObject.AddComponent<StandaloneInputModule>();

            return eventSystemObject;
        }

        /// <summary>
        /// Call this function to instantiate a Canvas setup for Handtracking.
        /// </summary>
        /// <returns>New Canvas GameObject.</returns>
        private static GameObject InstantiateCanvas()
        {
            // Create empty GameObject
            GameObject canvasObject = new GameObject("Handtracking Canvas");
            canvasObject.transform.position = new Vector3(0f, Camera.main.transform.position.y, 0.4f);
            canvasObject.transform.rotation = Quaternion.identity;
            canvasObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

            // Add Canvas and assign values
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            // Add CanvasScaler
            canvasObject.AddComponent<CanvasScaler>();

            // Add GraphicRaycaster
            canvasObject.AddComponent<GraphicRaycaster>();

            // Add TrackedDeviceGraphicRaycaster
            canvasObject.AddComponent<TrackedDeviceGraphicRaycaster>();

            Undo.RegisterCreatedObjectUndo(canvasObject, "Instantiated Canvas");

            return canvasObject;
        }

        /// <summary>
        /// Add a Simple Button in the scene.
        /// </summary>
        [MenuItem("GameObject/Lynx/UI/Simple Button", false, 211)]
        public static void AddSimpleButton()
        {
            InstantiatePrefab(STR_SIMPLE_BUTTON);
        }

        /// <summary>
        /// Add a Toggle Button in the scene.
        /// </summary>
        [MenuItem("GameObject/Lynx/UI/Toggle Button", false, 212)]
        public static void AddToggleButton()
        {
            InstantiatePrefab(STR_TOGGLE_BUTTON);
        }

        /// <summary>
        /// Add a Timer Button in the scene.
        /// </summary>
        [MenuItem("GameObject/Lynx/UI/Timer Button", false, 213)]
        public static void AddTimerButton()
        {
            InstantiatePrefab(STR_TIMER_BUTTON);
        }

        /// <summary>
        /// Add a Switch Button in the scene.
        /// </summary>
        [MenuItem("GameObject/Lynx/UI/Switch Button", false, 214)]
        public static void AddSwitchButton()
        {
            InstantiatePrefab(STR_SWITCH_BUTTON);
        }

        /// <summary>
        /// Call this function to instiantie a UI prefab.
        /// </summary>
        /// <param name="prefab">The UI prefab to instantiate.</param>
        private static void InstantiatePrefab(string prefab)
        {
            string str_gameObject = Directory.GetFiles(Application.dataPath, prefab, SearchOption.AllDirectories)[0].Replace(Application.dataPath, "Assets/");
            GameObject gameObject = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>(str_gameObject), SearchCanvas().transform) as GameObject;
            gameObject.transform.localPosition = Vector3.zero;

            Undo.RegisterCreatedObjectUndo(gameObject, "Instantiated UI");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        }


        /// <summary>
        /// Call this function to search a Canvas, if not, create it.
        /// </summary>
        /// <returns>A Canvas, found or created.</returns>
        private static GameObject SearchCanvas()
        {
            GameObject canvasObject = null;

            if (GameObject.FindObjectOfType<EventSystem>() == null)
            {
                InstantiateEventSystem();
            }

            try
            {
                canvasObject = GameObject.FindObjectOfType<TrackedDeviceGraphicRaycaster>().gameObject;
            }
            catch
            {
                canvasObject = InstantiateCanvas();
            }

            return canvasObject;
        }
#endif
    }
}
