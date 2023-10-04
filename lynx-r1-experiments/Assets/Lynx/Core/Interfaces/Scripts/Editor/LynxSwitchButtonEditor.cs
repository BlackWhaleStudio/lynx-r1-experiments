//   ==============================================================================
//   | LynxInterfaces (2023)                                                      |
//   |======================================                                      |
//   | LynxSimpleButton Editor Script                                             |
//   | Script to modify the inspector GUI of the LynxSwitchButton Script.         |
//   ==============================================================================

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace Lynx.UI
{
    [CustomEditor(typeof(Lynx.UI.LynxSwitchButton))]
    public class LynxSwitchButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle bold = new GUIStyle();
            bold = EditorStyles.boldLabel;

            serializedObject.Update();

            EditorGUILayout.LabelField("Button Parameters", bold);
            EditorGUILayout.Space(10);
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnPress"), EditorGUIUtility.TrTextContent("OnPress", "This event is called when the button is pressed."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnUnpress"), EditorGUIUtility.TrTextContent("OnUnpress", "This event is called when the button is released."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnToggle"), EditorGUIUtility.TrTextContent("OnToggle", "This event is called when the button is toggled."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnUntoggle"), EditorGUIUtility.TrTextContent("OnUntoggle", "This event is called when the button is untoggled."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_disableSelectState"), EditorGUIUtility.TrTextContent("Disable Select State", "If checked, the select state of the button is disable."));
            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Switch Button Parameters", bold);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_backgroundTarget"), EditorGUIUtility.TrTextContent("Background Target", "Switch button background, target image."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_slider"), EditorGUIUtility.TrTextContent("Slider", "Slider to manage the binary value of the slider."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_lerpSpeed"), EditorGUIUtility.TrTextContent("Slide Speed", "Speed for the slide animation."));
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_animation"), EditorGUIUtility.TrTextContent("Animation", "Pressing animation parameters."));
            EditorGUILayout.Space(20);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

